using System;
using Headless.ServicePlatform.Infrastructure.Proxy;
using Microsoft.AspNetCore.Builder;

namespace Headless.ServicePlatform.Infrastructure.Interceptor
{
    public static class ApiProxyResponseInterceptorExtensions
    {
        public static void UseApiProxyResponseInterceptor(this IApplicationBuilder app, IApiProxy apiProxy)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }
            if (apiProxy == null)
            {
                throw new ArgumentNullException(nameof(apiProxy));
            }

            app.UseMiddleware<ApiProxyResponseInterceptorMiddleware>(apiProxy);
        }
    }
}