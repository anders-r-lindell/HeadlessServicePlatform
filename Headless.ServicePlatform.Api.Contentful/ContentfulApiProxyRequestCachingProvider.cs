using System;
using Headless.ServicePlatform.Infrastructure.Caching;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;

namespace Headless.ServicePlatform.Api.Contentful
{
    public class ContentfulApiProxyRequestCachingProvider : ContentfulApi, IApiProxyRequestCachingProvider
    {
        public bool IsCachable(Uri inboundUri, Uri outboundUri, HttpContext context, out string key,
            out MemoryCacheEntryOptions options)
        {
            key = $"{context.Request.Headers["Authorization"]}-{outboundUri}";
            options = new MemoryCacheEntryOptions {
                AbsoluteExpirationRelativeToNow = new TimeSpan(0, 0, 5, 0)
            };

            return true;
        }
    }
}