using Headless.ServicePlatform.Infrastructure.Proxy;
using Microsoft.Extensions.Caching.Memory;

namespace Headless.ServicePlatform.Infrastructure.Caching
{
    public interface IApiProxyResponseCachingService
    {
        ApiProxyResponse Get(string key);

        void Set(string key, ApiProxyResponse value, MemoryCacheEntryOptions options);
    }
}