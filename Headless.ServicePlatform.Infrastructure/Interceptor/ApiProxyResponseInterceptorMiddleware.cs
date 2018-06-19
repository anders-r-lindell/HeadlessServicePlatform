using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Headless.ServicePlatform.Infrastructure.Proxy;
using Microsoft.AspNetCore.Http;

namespace Headless.ServicePlatform.Infrastructure.Interceptor
{
    public class ApiProxyResponseInterceptorMiddleware
    {
        private readonly IApiProxy _apiProxy;
        private readonly RequestDelegate _next;

        public ApiProxyResponseInterceptorMiddleware(RequestDelegate next, IApiProxy apiProxy)
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

        public async Task Invoke(HttpContext context, IApiProxyContextService apiProxyContextService,
            IEnumerable<IApiProxyResponseInterceptorProvider> apiProxyResponseInterceptorProviders,
            IEnumerable<IApiProxyErrorResponseInterceptorProvider> apiProxyErrorResponseInterceptorProviders,
            IApiProxyUriResolver apiProxyUriResolver)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            await _next.Invoke(context);

            var apiProxyContext = apiProxyContextService.Get(context);

            if (apiProxyContext.Response.ApiProxyResponse.StatusCode >= 400)
            {
                apiProxyErrorResponseInterceptorProviders.FirstOrDefault(x => x.Identifier == _apiProxy.Identifier)
                    ?.Intercept(
                        apiProxyUriResolver.ResolveInboundUri(context),
                        apiProxyUriResolver.ResolveOutboundUri(context, _apiProxy),
                        apiProxyContext.Response.ApiProxyResponse);
            }
            else
            {
                apiProxyResponseInterceptorProviders.FirstOrDefault(x => x.Identifier == _apiProxy.Identifier)
                    ?.Intercept(
                        apiProxyUriResolver.ResolveInboundUri(context),
                        apiProxyUriResolver.ResolveOutboundUri(context, _apiProxy),
                        apiProxyContext.Response.ApiProxyResponse);
            }
        }
    }
}