using System;
using Headless.ServicePlatform.Infrastructure.Authentication;
using Headless.ServicePlatform.Infrastructure.Caching;
using Headless.ServicePlatform.Infrastructure.Interceptor;
using Headless.ServicePlatform.Infrastructure.Proxy;
using Microsoft.Extensions.DependencyInjection;

namespace Headless.ServicePlatform.Api.Contentful
{
    public static class ContentfulApiServiceCollectionExtensions
    {
        public static IServiceCollection AddContentfulApiProxy(this IServiceCollection services)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            services.AddSingleton<IApiProxy, ContentfulApiProxy>();
            services.AddSingleton<IContentfulApiConfiguration, ContentfulApiConfiguration>();
            services.AddSingleton<IApiProxyRequestAuthenticationProvider, ContentfulApiProxyRequestAuthenticationProvider>();
            services.AddSingleton<IApiProxyRequestCachingProvider, ContentfulApiProxyRequestCachingProvider>();
            services.AddSingleton<IApiProxyResponseInterceptorProvider, ContentfulApiProxyResponseInterceptorProvider>();

            return services;
        }
    }
}