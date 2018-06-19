using Headless.ServicePlatform.Infrastructure.Configuration;
using Headless.ServicePlatform.Infrastructure.Proxy;

namespace Headless.ServicePlatform.Api.RESTCountries
{
    public class RESTCountriesApiProxy : RESTCountriesApi, IApiProxy
    {
        private readonly IApiProxyOptionsConfiguration _apiProxyOptionsConfiguration;

        public RESTCountriesApiProxy(IApiProxyOptionsConfiguration apiProxyOptionsConfiguration)
        {
            _apiProxyOptionsConfiguration = apiProxyOptionsConfiguration;
        }

        public ApiProxyOptions Options => _apiProxyOptionsConfiguration.Options(Identifier);
    }
}