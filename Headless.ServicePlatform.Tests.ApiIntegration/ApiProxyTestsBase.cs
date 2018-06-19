using System.Text.RegularExpressions;
using System.Threading.Tasks;
using FakeItEasy;
using Headless.ServicePlatform.Api.RESTCountries;
using Headless.ServicePlatform.Infrastructure.Authentication;
using Headless.ServicePlatform.Infrastructure.Caching;
using Headless.ServicePlatform.Infrastructure.Configuration;
using Headless.ServicePlatform.Infrastructure.Interceptor;
using Headless.ServicePlatform.Infrastructure.Proxy;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using Headless.ServicePlatform.Api.Contentful;

namespace Headless.ServicePlatform.Tests.ApiIntegration
{
    public class ApiProxyTestsBase
    {
        protected TestServer TestServer;

        [SetUp]
        public void SetUpFixture()
        {
            var configurationFake = A.Fake<IConfiguration>();
            A.CallTo(() => configurationFake["Headless:ServicePlatform:DebugMode"]).Returns("True");
            A.CallTo(() => configurationFake["Headless:ServicePlatform:Api:RESTCountries:Scheme"]).Returns("https");
            A.CallTo(() => configurationFake["Headless:ServicePlatform:Api:RESTCountries:Host"]).Returns("restcountries.eu");
            A.CallTo(() => configurationFake["Headless:ServicePlatform:Api:RESTCountries:InboundPathBase"]).Returns("/api/countries/");
            A.CallTo(() => configurationFake["Headless:ServicePlatform:Api:RESTCountries:OutboundPathBase"]).Returns("/rest/v2/");
            A.CallTo(() => configurationFake["Headless:ServicePlatform:Api:Contentful:Scheme"]).Returns("https");
            A.CallTo(() => configurationFake["Headless:ServicePlatform:Api:Contentful:Host"]).Returns("cdn.contentful.com");
            A.CallTo(() => configurationFake["Headless:ServicePlatform:Api:Contentful:InboundPathBase"]).Returns("/api/contentful/");
            A.CallTo(() => configurationFake["Headless:ServicePlatform:Api:Contentful:OutboundPathBase"]).Returns("");
            A.CallTo(() => configurationFake["Headless:ServicePlatform:Api:Contentful:AccessToken"]).Returns("fdb4e7a3102747a02ea69ebac5e282b9e44d28fb340f778a4f5e788625a61abe");

            var builder = new WebHostBuilder()
                .Configure(app =>
                    {
                        foreach (var apiProxy in app.ApplicationServices.GetServices<IApiProxy>())
                        {
                            app.MapWhen(
                                httpContext => new Regex($"^{apiProxy.Options.InboundPathBase}").IsMatch(
                                    httpContext.Request.Path.Value),
                                config =>
                                {
                                    ApiProxyExtensions.UseApiProxyResponse(config, apiProxy);
                                    ApiProxyRequestAuthenticationExtensions.UseApiProxyRequestAuthentication(config, apiProxy);
                                    ApiProxyRequestCachingExtensions.UseApiProxyRequestCaching(config, apiProxy);
                                    ApiProxyResponseInterceptorExtensions.UseApiProxyResponseInterceptor(config, apiProxy);
                                    ApiProxyExtensions.UseApiProxyRequest(config, apiProxy);
                                });
                        }
                    }
                ).ConfigureServices(services =>
                {
                    services.AddMemoryCache();
                    services.AddSingleton(configurationFake);

                    services.AddApiProxyService(options =>
                    {
                        options.PrepareRequest = (originalRequest, message) =>
                        {
                            message.Headers.Add("X-Forwarded-Host", originalRequest.Host.Host);
                            return Task.FromResult(0);
                        };
                    });

                    services.AddSingleton<IApiProxyOptionsConfiguration, ApiProxyOptionsConfiguration>();
                    services.AddSingleton<IApiProxyUriResolver, ApiProxyUriResolver>();

                    services.AddRESTCountriesApiProxy()
                        .AddContentfulApiProxy();
                });

            TestServer = new TestServer(builder);
        }
    }
}