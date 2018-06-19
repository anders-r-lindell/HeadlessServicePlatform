using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Headless.ServicePlatform.Api.Contentful;
using Headless.ServicePlatform.Api.Moltin;
using Headless.ServicePlatform.Api.RESTCountries;
using Headless.ServicePlatform.Infrastructure.Authentication;
using Headless.ServicePlatform.Infrastructure.Caching;
using Headless.ServicePlatform.Infrastructure.Configuration;
using Headless.ServicePlatform.Infrastructure.Interceptor;
using Headless.ServicePlatform.Infrastructure.Proxy;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Headless.ServicePlatform.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            services.AddMemoryCache();
            services.AddSingleton(Configuration);

            services.AddApiProxyService(options =>
            {
                options.PrepareRequest = (originalRequest, message) =>
                {
                    message.Headers.Add("X-Forwarded-Host", originalRequest.Host.Host);
                    return Task.FromResult(0);
                };
            });

            services.AddContentfulApiProxy()
                .AddMoltinApiProxy()
                .AddRESTCountriesApiProxy();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            foreach (var apiProxy in app.ApplicationServices.GetServices<IApiProxy>())
            {
                app.MapWhen(
                    httpContext => IsApiProxyRequest(httpContext, apiProxy.Options.InboundPathBase),
                    builder =>
                    {
                        builder.UseApiProxyResponse(apiProxy);
                        builder.UseApiProxyRequestAuthentication(apiProxy);
                        builder.UseApiProxyRequestCaching(apiProxy);
                        builder.UseApiProxyResponseInterceptor(apiProxy);
                        builder.UseApiProxyRequest(apiProxy);
                    });
            }

            app.UseMvc();
        }

        private static bool IsApiProxyRequest(HttpContext httpContext, string inboundPathToMatch)
        {
            return new Regex($"^{inboundPathToMatch}").IsMatch(httpContext.Request.Path.Value);
        }
    }
}