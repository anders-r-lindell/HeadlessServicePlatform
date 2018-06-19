using Headless.ServicePlatform.Infrastructure;

namespace Headless.ServicePlatform.Api.Moltin
{
    public abstract class MoltinApi : IApiIdentifier
    {
        public string Identifier => "Moltin";
    }
}