using Headless.ServicePlatform.Infrastructure.Configuration;
using Headless.ServicePlatform.Infrastructure.Proxy;

namespace Headless.ServicePlatform.Api.Contentful
{
    public class ContentfulApiProxy : ContentfulApi, IApiProxy
    {
        private readonly IApiProxyOptionsConfiguration _apiProxyOptionsConfiguration;

        public ContentfulApiProxy(IApiProxyOptionsConfiguration apiProxyOptionsConfiguration)
        {
            _apiProxyOptionsConfiguration = apiProxyOptionsConfiguration;
        }

        public ApiProxyOptions Options => _apiProxyOptionsConfiguration.Options(Identifier);
    }
}