using System;
using Headless.ServicePlatform.Infrastructure.Proxy;
using Microsoft.AspNetCore.Builder;

namespace Headless.ServicePlatform.Infrastructure.Authentication
{
    public static class ApiProxyRequestAuthenticationExtensions
    {
        public static void UseApiProxyRequestAuthentication(this IApplicationBuilder app, IApiProxy apiProxy)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }
            if (apiProxy == null)
            {
                throw new ArgumentNullException(nameof(apiProxy));
            }

            app.UseMiddleware<ApiProxyRequestAuthenticationMiddleware>(apiProxy);
        }
    }
}