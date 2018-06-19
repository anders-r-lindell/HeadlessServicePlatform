using System;
using Headless.ServicePlatform.Infrastructure.Proxy;
using Microsoft.Extensions.DependencyInjection;

namespace Headless.ServicePlatform.Api.RESTCountries
{
    public static class RESTCountriesApiServiceCollectionExtensions
    {
        public static IServiceCollection AddRESTCountriesApiProxy(this IServiceCollection services)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            services.AddSingleton<IApiProxy, RESTCountriesApiProxy>();

            return services;
        }
    }
}