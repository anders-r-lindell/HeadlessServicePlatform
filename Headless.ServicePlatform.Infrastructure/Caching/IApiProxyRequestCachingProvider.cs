using System;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;

namespace Headless.ServicePlatform.Infrastructure.Caching
{
    public interface IApiProxyRequestCachingProvider : IApiIdentifier
    {
        bool IsCachable(Uri inboundUri, Uri outboundUri, HttpContext context, out string key,
            out MemoryCacheEntryOptions options);
    }
}