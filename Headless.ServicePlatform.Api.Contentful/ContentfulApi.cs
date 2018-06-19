using Headless.ServicePlatform.Infrastructure;

namespace Headless.ServicePlatform.Api.Contentful
{
    public abstract class ContentfulApi : IApiIdentifier
    {
        public string Identifier => "Contentful";
    }
}