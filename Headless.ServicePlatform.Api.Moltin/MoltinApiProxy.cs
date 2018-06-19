using Headless.ServicePlatform.Infrastructure.Configuration;
using Headless.ServicePlatform.Infrastructure.Proxy;

namespace Headless.ServicePlatform.Api.Moltin
{
    public class MoltinApiProxy : MoltinApi, IApiProxy
    {
        private readonly IApiProxyOptionsConfiguration _apiProxyOptionsConfiguration;

        public MoltinApiProxy(IApiProxyOptionsConfiguration apiProxyOptionsConfiguration)
        {
            _apiProxyOptionsConfiguration = apiProxyOptionsConfiguration;
        }

        public ApiProxyOptions Options => _apiProxyOptionsConfiguration.Options(Identifier);
    }
}