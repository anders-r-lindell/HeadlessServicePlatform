using System;
using Headless.ServicePlatform.Infrastructure.Caching;
using Headless.ServicePlatform.Infrastructure.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Headless.ServicePlatform.Infrastructure.Proxy
{
    public static class ApiProxyServiceCollectionExtensions
    { 
        public static IServiceCollection AddApiProxyService(this IServiceCollection services, Action<SharedProxyOptions> configureOptions)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }
            if (configureOptions == null)
            {
                throw new ArgumentNullException(nameof(configureOptions));
            }

            services.Configure(configureOptions);

            services.AddSingleton<IProxyRequestService, ProxyRequestService>();
            services.AddSingleton<IProxyHttpClient, ProxyHttpClient>();
            services.AddSingleton<IApiProxyContextService, ApiProxyContextService>();
            services.AddSingleton<IApiProxyResponseCachingService, ApiProxyResponseCachingService>();
            services.AddSingleton<IApiProxyConfiguration, ApiProxyConfiguration>();
            services.AddSingleton<IApiProxyOptionsConfiguration, ApiProxyOptionsConfiguration>();
            services.AddSingleton<IApiProxyUriResolver, ApiProxyUriResolver>();

            return services;
        }
    }
}