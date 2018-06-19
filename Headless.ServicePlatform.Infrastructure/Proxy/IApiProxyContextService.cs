using Microsoft.AspNetCore.Http;

namespace Headless.ServicePlatform.Infrastructure.Proxy
{
    public interface IApiProxyContextService
    {
        ApiProxyContext Get(HttpContext context);
    }
}