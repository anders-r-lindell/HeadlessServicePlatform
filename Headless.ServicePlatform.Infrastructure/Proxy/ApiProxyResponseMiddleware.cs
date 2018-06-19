using System;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using Headless.ServicePlatform.Infrastructure.Configuration;
using Microsoft.AspNetCore.Http;

namespace Headless.ServicePlatform.Infrastructure.Proxy
{
    public class ApiProxyResponseMiddleware
    {
        private readonly IApiProxy _apiProxy;
        private readonly RequestDelegate _next;

        public ApiProxyResponseMiddleware(RequestDelegate next, IApiProxy apiProxy)
        {
            _apiProxy = apiProxy;
            _next = next;
        }

        public async Task Invoke(HttpContext context, IApiProxyContextService apiProxyContextService, IApiProxyConfiguration apiProxyConfiguration,
            IApiProxyUriResolver apiProxyUriResolver)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var apiProxyContext = apiProxyContextService.Get(context);
            apiProxyContext.ExecutionStart = DateTime.Now;

            await _next.Invoke(context);

            apiProxyContext.ExecutionEnd = DateTime.Now;

            if (apiProxyContext.Response.HasResponse)
            {
                await CopyApiProxyResponseToHttpResponse(context, apiProxyContext, apiProxyUriResolver, apiProxyConfiguration.DebugMode);
            }
        }

        private async Task CopyApiProxyResponseToHttpResponse(HttpContext context, ApiProxyContext apiProxyContext, IApiProxyUriResolver apiProxyUriResolver, bool debugMode)
        {
            var response = context.Response;

            var apiProxyResponse = apiProxyContext.Response.ApiProxyResponse;

            response.StatusCode = apiProxyResponse.StatusCode;

            foreach (var header in apiProxyResponse.Headers)
            {
                response.Headers[header.Key] = header.Value;
            }

            if (debugMode)
            {
                AddDebugInfoToResponse(context, response, apiProxyContext, apiProxyUriResolver);
            }

            // SendAsync removes chunking from the response. This removes the header so it doesn't expect a chunked response.
            response.Headers.Remove("transfer-encoding");

            using (var stream = new MemoryStream(apiProxyResponse.Body))
            {
                stream.Position = 0;
                await stream.CopyToAsync(context.Response.Body, 81920, context.RequestAborted);
            }
        }

        private void AddDebugInfoToResponse(HttpContext context, HttpResponse response, ApiProxyContext apiProxyContext, IApiProxyUriResolver apiProxyUriResolver)
        {
            response.Headers["api-proxy-outbound-url"] = apiProxyUriResolver.ResolveOutboundUri(context, _apiProxy).ToString();
            response.Headers["api-proxy-fetched-from-cache"] = apiProxyContext.Response.FetchedFromCache.ToString().ToLower();
            response.Headers["api-proxy-middleware-execution-time"] = apiProxyContext.ExecutionEnd
                .Subtract(apiProxyContext.ExecutionStart).TotalMilliseconds.ToString(CultureInfo.InvariantCulture);
        }
    }
}