using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.WebUtilities;

namespace Headless.ServicePlatform.Infrastructure.Proxy
{
    public class ApiProxyUriResolver : IApiProxyUriResolver
    {
        private readonly IApiProxyContextService _apiProxyContextService;
        
        public ApiProxyUriResolver(IApiProxyContextService apiProxyContextService)
        {
            _apiProxyContextService = apiProxyContextService;
        }

        public Uri ResolveOutboundUri(HttpContext context, IApiProxy apiProxy)
        {
            var pathWithNoInboundPathBase = context.Request.Path.Value.Replace(apiProxy.Options.InboundPathBase, "/");
            var path = new PathString(pathWithNoInboundPathBase);
            
            var url = UriHelper.BuildAbsolute(apiProxy.Options.Scheme, apiProxy.Options.Host,
                apiProxy.Options.OutboundPathBase, path, context.Request.QueryString);
            
            url = QueryHelpers.AddQueryString(url, _apiProxyContextService.Get(context).Request.AppendQueryStringToApiRequest);

            return new Uri(url);
        }

        public Uri ResolveInboundUri(HttpContext context)
        {
            return new Uri(UriHelper.BuildAbsolute(context.Request.Scheme, context.Request.Host,
                null, context.Request.Path, context.Request.QueryString));
        }
    }
}