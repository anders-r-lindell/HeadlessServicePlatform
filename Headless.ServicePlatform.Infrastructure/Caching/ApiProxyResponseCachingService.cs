using Headless.ServicePlatform.Infrastructure.Proxy;
using Microsoft.Extensions.Caching.Memory;

namespace Headless.ServicePlatform.Infrastructure.Caching
{
    public class ApiProxyResponseCachingService : IApiProxyResponseCachingService
    {
        private readonly IMemoryCache _memoryCache;

        public ApiProxyResponseCachingService(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public ApiProxyResponse Get(string key)
        {
            return _memoryCache.Get<ApiProxyResponse>(key);
        }

        public void Set(string key, ApiProxyResponse value, MemoryCacheEntryOptions options)
        {
            _memoryCache.Set(key, value, options);
        }
    }
}