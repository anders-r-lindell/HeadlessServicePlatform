using System;
using Headless.ServicePlatform.Infrastructure.Proxy;

namespace Headless.ServicePlatform.Infrastructure.Interceptor
{
    public interface IApiProxyResponseInterceptorProvider : IApiIdentifier
    {
        void Intercept(Uri inboundUri, Uri outboundUri, ApiProxyResponse apiProxyResponse);
    }
}
