using Microsoft.Extensions.Configuration;

namespace Headless.ServicePlatform.Api.Contentful
{
    public class ContentfulApiConfiguration : ContentfulApi, IContentfulApiConfiguration
    {
        private readonly IConfiguration _configuration;

        public ContentfulApiConfiguration(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string AccessToken => _configuration["Headless:ServicePlatform:Api:Contentful:AccessToken"];
    }
}