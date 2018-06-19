using System.Net.Http;

namespace Headless.ServicePlatform.Infrastructure.Proxy
{
    public interface IProxyHttpClient
    {
        SharedProxyOptions Options { get; }

        HttpClient Client { get; }
    }
}