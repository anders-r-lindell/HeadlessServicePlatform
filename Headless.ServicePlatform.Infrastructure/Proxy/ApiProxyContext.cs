using System;
using System.Collections.Generic;

namespace Headless.ServicePlatform.Infrastructure.Proxy
{
    public class ApiProxyContext
    {
        public ApiProxyContext()
        {
            Request = new ApiProxyRequestContext();
            Response = new ApiProxyResponseContext();
        }

        public DateTime ExecutionStart { get; set; }

        public DateTime ExecutionEnd { get; set; }

        public ApiProxyRequestContext Request { get; private set; }

        public ApiProxyResponseContext Response { get; private set; }
    }

    public class ApiProxyResponseContext
    {
        public bool HasResponse { get; set; }

        public bool FetchedFromCache { get; set; }

        public ApiProxyResponse ApiProxyResponse { get; set; }
    }

    public class ApiProxyRequestContext
    {
        public ApiProxyRequestContext()
        {
            AppendQueryStringToApiRequest = new Dictionary<string, string>();
        }

        public Dictionary<string, string> AppendQueryStringToApiRequest { get; private set; }
    }
}