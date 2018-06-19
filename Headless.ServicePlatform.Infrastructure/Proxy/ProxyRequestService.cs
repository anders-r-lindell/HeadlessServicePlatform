using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Headless.ServicePlatform.Infrastructure.Proxy
{
    public class ProxyRequestService : IProxyRequestService
    {
        private readonly IProxyHttpClient _proxyHttpClient;

        public ProxyRequestService(IProxyHttpClient proxyHttpClient)
        {
            _proxyHttpClient = proxyHttpClient;
        }

        public async Task<ApiProxyResponse> ProxyRequest(HttpContext context, Uri destinationUri)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }
            if (destinationUri == null)
            {
                throw new ArgumentNullException(nameof(destinationUri));
            }

            if (context.WebSockets.IsWebSocketRequest)
            {
                throw new NotSupportedException("Web socket requests not supported by proxy API");
            }

            using (var requestMessage = CreateProxyHttpRequest(context, destinationUri))
            {
                var prepareRequestHandler = _proxyHttpClient.Options.PrepareRequest;
                if (prepareRequestHandler != null)
                {
                    await prepareRequestHandler(context.Request, requestMessage);
                }

                using (var responseMessage = await SendProxyHttpRequest(context, requestMessage))
                {
                    return await CreateApiProxyResponseFromProxyRequestResponse(responseMessage);
                }
            }
        }

        private async Task<ApiProxyResponse> CreateApiProxyResponseFromProxyRequestResponse(HttpResponseMessage responseMessage)
        {
            if (responseMessage == null)
            {
                throw new ArgumentNullException(nameof(responseMessage));
            }

            var headers = new Dictionary<string, string[]>();

            foreach (var header in responseMessage.Headers)
            {
                headers.Add(header.Key, header.Value.ToArray());
            }

            foreach (var header in responseMessage.Content.Headers)
            {
                headers.Add(header.Key, header.Value.ToArray());
            }

            var body = await responseMessage.Content.ReadAsByteArrayAsync();

            return new ApiProxyResponse
            {
                StatusCode = (int)responseMessage.StatusCode,
                Headers = headers,
                Body = body
            };
        }

        private Task<HttpResponseMessage> SendProxyHttpRequest(HttpContext context, HttpRequestMessage requestMessage)
        {
            if (requestMessage == null)
            {
                throw new ArgumentNullException(nameof(requestMessage));
            }

            var response = _proxyHttpClient.Client.SendAsync(requestMessage, HttpCompletionOption.ResponseHeadersRead,
                context.RequestAborted);
            return response;
        }

        private HttpRequestMessage CreateProxyHttpRequest(HttpContext context, Uri uri)
        {
            var request = context.Request;

            var requestMessage = new HttpRequestMessage();
            var requestMethod = request.Method;
            if (!HttpMethods.IsGet(requestMethod) &&
                !HttpMethods.IsHead(requestMethod) &&
                !HttpMethods.IsDelete(requestMethod) &&
                !HttpMethods.IsTrace(requestMethod))
            {
                var streamContent = new StreamContent(request.Body);
                requestMessage.Content = streamContent;
            }

            // Copy the request headers
            foreach (var header in request.Headers)
            {
                if (!requestMessage.Headers.TryAddWithoutValidation(header.Key, header.Value.ToArray()))
                {
                    requestMessage.Content?.Headers.TryAddWithoutValidation(header.Key, header.Value.ToArray());
                }
            }

            requestMessage.Headers.Host = uri.Authority;
            requestMessage.RequestUri = uri;
            requestMessage.Method = new HttpMethod(request.Method);

            return requestMessage;
        }
    }
}