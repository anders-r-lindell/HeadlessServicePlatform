using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Headless.ServicePlatform.Infrastructure.Proxy
{
    public interface IProxyRequestService
    {
        Task<ApiProxyResponse> ProxyRequest(HttpContext context, Uri destinationUri);
    }
}