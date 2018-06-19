using System;
using Headless.ServicePlatform.Infrastructure.Proxy;
using Microsoft.Extensions.DependencyInjection;

namespace Headless.ServicePlatform.Api.Moltin
{
    public static class MoltinApiProxyServiceCollectionExtensions
    {
        public static IServiceCollection AddMoltinApiProxy(this IServiceCollection services)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            services.AddSingleton<IApiProxy, MoltinApiProxy>();

            return services;
        }
    }
}