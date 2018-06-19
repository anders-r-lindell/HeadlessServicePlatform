using System;
using System.Net.Http;
using Microsoft.Extensions.Options;

namespace Headless.ServicePlatform.Infrastructure.Proxy
{
    public class ProxyHttpClient : IProxyHttpClient
    {
        public ProxyHttpClient(IOptions<SharedProxyOptions> options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            Options = options.Value;
            Client = new HttpClient(Options.MessageHandler ??
                                    new HttpClientHandler {AllowAutoRedirect = false, UseCookies = false});
        }

        public SharedProxyOptions Options { get; }

        public HttpClient Client { get; }
    }
}