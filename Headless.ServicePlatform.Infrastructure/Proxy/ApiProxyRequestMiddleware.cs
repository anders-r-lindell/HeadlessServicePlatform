using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Headless.ServicePlatform.Infrastructure.Proxy
{
    public class ApiProxyRequestMiddleware
    {
        private readonly IApiProxy _apiProxy;
       
        public ApiProxyRequestMiddleware(RequestDelegate next, IApiProxy apiProxy)
        {
            if (apiProxy == null)
            {
                throw new ArgumentNullException(nameof(apiProxy));
            }
            if (string.IsNullOrWhiteSpace(apiProxy.Options.Scheme))
            {
                throw new ArgumentException("Api proxy options must specify scheme.", nameof(apiProxy.Options));
            }
            if (!apiProxy.Options.Host.HasValue)
            {
                throw new ArgumentException("Api proxy options must specify host.", nameof(apiProxy.Options));
            }

            _apiProxy = apiProxy;
        }

        public async Task Invoke(HttpContext context, IApiProxyContextService apiProxyContextService, IProxyRequestService proxyRequestService, IApiProxyUriResolver apiProxyUriResolver)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var apiProxyContext = apiProxyContextService.Get(context);

            var outboundUri = apiProxyUriResolver.ResolveOutboundUri(context, _apiProxy);

            apiProxyContext.Response.ApiProxyResponse = await proxyRequestService.ProxyRequest(context, outboundUri);
            apiProxyContext.Response.HasResponse = true;
        }
    }
}