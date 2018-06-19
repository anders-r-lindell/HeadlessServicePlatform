using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Headless.ServicePlatform.Infrastructure.Proxy;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Caching.Memory;

namespace Headless.ServicePlatform.Infrastructure.Caching
{
    public class ApiProxyRequestCachingMiddleware
    {
        private readonly IApiProxy _apiProxy;
        private readonly RequestDelegate _next;

        public ApiProxyRequestCachingMiddleware(RequestDelegate next, IApiProxy apiProxy)
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
            IEnumerable<IApiProxyRequestCachingProvider> apiProxyCachingProviders, 
            IApiProxyResponseCachingService apiProxyResponseCachingService,
            IApiProxyUriResolver apiProxyUriResolver)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var provider = apiProxyCachingProviders.FirstOrDefault(x => x.Identifier == _apiProxy.Identifier);

            if (provider == null)
            {
                // No caching provider defined for Api proxy, call next middleware...
                await _next.Invoke(context);
                return;
            }

            var isCachable = provider.IsCachable(
                apiProxyUriResolver.ResolveInboundUri(context),
                apiProxyUriResolver.ResolveOutboundUri(context, _apiProxy), 
                context, out var key, out var options);

            var apiProxyContext = isCachable ? apiProxyContextService.Get(context) : null;

            if (isCachable)
            {
                // Try get Api proxy response from cache...
                var apiProxyResponse = apiProxyResponseCachingService.Get(key);
                if (apiProxyResponse != null)
                {
                    // Api proxy response found in cache...
                    apiProxyContext.Response.HasResponse = true;
                    apiProxyContext.Response.FetchedFromCache = true;
                    apiProxyContext.Response.ApiProxyResponse = apiProxyResponse;
                    return;
                }
            }

            // Not found in cache or not cachable, call next middleware......
            await _next.Invoke(context);

            if (isCachable)
            {
                if (apiProxyContext.Response.HasResponse && 
                    apiProxyContext.Response.ApiProxyResponse.StatusCode == 200)
                {
                    // Add Api proxy response to cache if status code is 200...
                    apiProxyResponseCachingService.Set(key,
                        apiProxyContext.Response.ApiProxyResponse,
                        options);
                }
            }
        }
    }
}