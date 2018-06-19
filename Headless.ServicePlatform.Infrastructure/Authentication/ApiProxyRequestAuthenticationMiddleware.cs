using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Headless.ServicePlatform.Infrastructure.Proxy;
using Microsoft.AspNetCore.Http;

namespace Headless.ServicePlatform.Infrastructure.Authentication
{
    public class ApiProxyRequestAuthenticationMiddleware
    {
        private readonly IApiProxy _apiProxy;
        private readonly RequestDelegate _next;

        public ApiProxyRequestAuthenticationMiddleware(RequestDelegate next, IApiProxy apiProxy)
        {
            if (apiProxy == null)
            {
                throw new ArgumentNullException(nameof(apiProxy));
            }
            if (string.IsNullOrWhiteSpace(apiProxy.Identifier))
            {
                throw new ArgumentException("Api proxy must specify identifier.", nameof(apiProxy.Identifier));
            }

            _next = next;
            _apiProxy = apiProxy;
        }

        public async Task Invoke(HttpContext context, IEnumerable<IApiProxyRequestAuthenticationProvider> apiAuthenticationProviders)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var provider = apiAuthenticationProviders.FirstOrDefault(x => x.Identifier == _apiProxy.Identifier);

            provider?.Authenticate(context);

            await _next.Invoke(context);
        }
    }
}