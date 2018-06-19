using System;
using Headless.ServicePlatform.Infrastructure.Interceptor;
using Headless.ServicePlatform.Infrastructure.Proxy;

namespace Headless.ServicePlatform.Api.Contentful
{
    public class ContentfulApiProxyResponseInterceptorProvider : ContentfulApi, IApiProxyResponseInterceptorProvider
    {
        public void Intercept(Uri inboundUri, Uri outboundUri, ApiProxyResponse apiProxyResponse)
        {
            dynamic body = new {foo = "bar", bar = 100};

            //apiProxyResponse.Body = new byte[0];
        }
    }
}