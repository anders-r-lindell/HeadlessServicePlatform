using Headless.ServicePlatform.Infrastructure.Proxy;

namespace Headless.ServicePlatform.Infrastructure.Configuration
{
    public interface IApiProxyOptionsConfiguration
    {
        ApiProxyOptions Options(string identifier);
    }
}