using Headless.ServicePlatform.Infrastructure.Authentication;
using Microsoft.AspNetCore.Http;

namespace Headless.ServicePlatform.Api.Contentful
{
    public class ContentfulApiProxyRequestAuthenticationProvider : ContentfulApi, IApiProxyRequestAuthenticationProvider
    {
        private readonly IContentfulApiConfiguration _contentfulApiConfiguration;

        public ContentfulApiProxyRequestAuthenticationProvider(IContentfulApiConfiguration contentfulApiConfiguration)
        {
            _contentfulApiConfiguration = contentfulApiConfiguration;
        }

        public void Authenticate(HttpContext context)
        {
            context.Request.Headers.Add("Authorization", $"Bearer {_contentfulApiConfiguration.AccessToken}");
        }
    }
}