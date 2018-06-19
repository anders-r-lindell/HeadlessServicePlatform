using Microsoft.Extensions.Configuration;

namespace Headless.ServicePlatform.Infrastructure.Configuration
{
    public class ApiProxyConfiguration : IApiProxyConfiguration
    {
        private readonly IConfiguration _configuration;

        public ApiProxyConfiguration(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public bool DebugMode
        {
            get
            {
                var configValue = _configuration["Headless:ServicePlatform:DebugMode"];
                if (string.IsNullOrWhiteSpace(configValue))
                {
                    return false;
                }

                return bool.Parse(configValue);
            }
        }
    }
}