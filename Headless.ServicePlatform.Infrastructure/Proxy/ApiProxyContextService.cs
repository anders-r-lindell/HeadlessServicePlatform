using Microsoft.AspNetCore.Http;

namespace Headless.ServicePlatform.Infrastructure.Proxy
{
    public class ApiProxyContextService : IApiProxyContextService
    {
        public ApiProxyContext Get(HttpContext context)
        {
            var key = typeof(ApiProxyContext).FullName;

            var apiProxyRequestContext = context.Items[key] as ApiProxyContext;

            if (apiProxyRequestContext == null)
            {
                apiProxyRequestContext = new ApiProxyContext();
                context.Items.Add(key, apiProxyRequestContext);
            }

            return apiProxyRequestContext;
        }
    }
}