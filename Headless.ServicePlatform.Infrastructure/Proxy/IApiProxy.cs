namespace Headless.ServicePlatform.Infrastructure.Proxy
{
    public interface IApiProxy : IApiIdentifier
    { 
        ApiProxyOptions Options { get; }
    }
}