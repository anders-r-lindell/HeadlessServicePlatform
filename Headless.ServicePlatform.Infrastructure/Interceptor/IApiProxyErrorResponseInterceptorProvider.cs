using System;
using Headless.ServicePlatform.Infrastructure.Proxy;

namespace Headless.ServicePlatform.Infrastructure.Interceptor
{
    public interface IApiProxyErrorResponseInterceptorProvider : IApiIdentifier
    {
        void Intercept(Uri inboundUri, Uri outboundUri, ApiProxyResponse apiProxyResponse);
    }
}