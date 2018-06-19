using System;
using Microsoft.AspNetCore.Http;

namespace Headless.ServicePlatform.Infrastructure.Proxy
{
    public interface IApiProxyUriResolver
    {
        Uri ResolveOutboundUri(HttpContext context, IApiProxy apiProxy);

        Uri ResolveInboundUri(HttpContext context);
    }
}