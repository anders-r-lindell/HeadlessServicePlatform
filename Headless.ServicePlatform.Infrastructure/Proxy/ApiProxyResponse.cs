using System.Collections.Generic;

namespace Headless.ServicePlatform.Infrastructure.Proxy
{
    public class ApiProxyResponse
    {
        public int StatusCode { get; set; }

        public Dictionary<string, string[]> Headers { get; set; }

        public byte[] Body { get; set; }
    }
}