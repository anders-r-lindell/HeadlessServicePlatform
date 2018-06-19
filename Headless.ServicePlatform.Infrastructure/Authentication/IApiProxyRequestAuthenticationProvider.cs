using Microsoft.AspNetCore.Http;

namespace Headless.ServicePlatform.Infrastructure.Authentication
{
    public interface IApiProxyRequestAuthenticationProvider : IApiIdentifier
    {
        void Authenticate(HttpContext context);
    }
}
