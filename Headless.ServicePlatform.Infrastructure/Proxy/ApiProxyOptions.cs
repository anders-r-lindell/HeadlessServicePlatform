using Microsoft.AspNetCore.Http;

namespace Headless.ServicePlatform.Infrastructure.Proxy
{
    /// <summary>
    ///     Proxy Options
    /// </summary>
    public class ApiProxyOptions
    {
        /// <summary>
        ///     Destination uri scheme
        /// </summary>
        public string Scheme { get; set; }

        /// <summary>
        ///     Destination uri host
        /// </summary>
        public HostString Host { get; set; }

        /// <summary>
        ///     Destination uri path base that will replace the inbound uri path base
        /// </summary>
        public PathString OutboundPathBase { get; set; }

        /// <summary>
        ///     Inbound uri path base to match this proxy options
        /// </summary>
        public PathString InboundPathBase { get; set; }
    }
}