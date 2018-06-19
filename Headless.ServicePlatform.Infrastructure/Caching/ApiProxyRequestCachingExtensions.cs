using System;
using Headless.ServicePlatform.Infrastructure.Proxy;
using Microsoft.AspNetCore.Builder;

namespace Headless.ServicePlatform.Infrastructure.Caching
{
    public static class ApiProxyRequestCachingExtensions
    {
        public static void UseApiProxyRequestCaching(this IApplicationBuilder app, IApiProxy apiProxy)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }
            if (apiProxy == null)
            {
                throw new ArgumentNullException(nameof(apiProxy));
            }

            app.UseMiddleware<ApiProxyRequestCachingMiddleware>(apiProxy);
        }
    }
}