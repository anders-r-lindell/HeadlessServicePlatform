using Headless.ServicePlatform.Infrastructure.Proxy;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace Headless.ServicePlatform.Infrastructure.Configuration
{
    public class ApiProxyOptionsConfiguration : IApiProxyOptionsConfiguration
    {
        private readonly IConfiguration _configuration;

        public ApiProxyOptionsConfiguration(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public ApiProxyOptions Options(string identifier)
        {
            return new ApiProxyOptions
            {
                Scheme = Scheme(identifier),
                Host = Host(identifier),
                InboundPathBase = InboundPathBase(identifier),
                OutboundPathBase = OutboundPathBase(identifier)
            };
        }

        private string Scheme(string apiIdentifier)
        {
            return _configuration[$"Headless:ServicePlatform:Api:{apiIdentifier}:Scheme"];
        }

        private HostString Host(string apiIdentifier)
        {
            return new HostString(_configuration[$"Headless:ServicePlatform:Api:{apiIdentifier}:Host"]);
        }

        private string InboundPathBase(string apiIdentifier)
        {
            return _configuration[$"Headless:ServicePlatform:Api:{apiIdentifier}:InboundPathBase"];
        }

        private string OutboundPathBase(string apiIdentifier)
        {
            return _configuration[$"Headless:ServicePlatform:Api:{apiIdentifier}:OutboundPathBase"] ?? "";
        }
    }
}