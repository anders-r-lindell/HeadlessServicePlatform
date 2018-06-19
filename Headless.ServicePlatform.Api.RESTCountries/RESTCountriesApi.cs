using Headless.ServicePlatform.Infrastructure;

namespace Headless.ServicePlatform.Api.RESTCountries
{
    public abstract class RESTCountriesApi : IApiIdentifier
    {
        public string Identifier => "RESTCountries";
    }
}