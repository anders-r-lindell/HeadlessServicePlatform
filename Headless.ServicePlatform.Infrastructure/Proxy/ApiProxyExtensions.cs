using System;
using Microsoft.AspNetCore.Builder;

namespace Headless.ServicePlatform.Infrastructure.Proxy
{
    public static class ApiProxyExtensions
    {
        public static void UseApiProxyResponse(this IApplicationBuilder app, IApiProxy apiProxy)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }
            if (apiProxy == null)
            {
                throw new ArgumentNullException(nameof(apiProxy));
            }

            app.UseMiddleware<ApiProxyResponseMiddleware>(apiProxy);
        }

        public static void UseApiProxyRequest(this IApplicationBuilder app, IApiProxy apiProxy)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }
            if (apiProxy == null)
            {
                throw new ArgumentNullException(nameof(apiProxy));
            }

            app.UseMiddleware<ApiProxyRequestMiddleware>(apiProxy);
        }
    }
}