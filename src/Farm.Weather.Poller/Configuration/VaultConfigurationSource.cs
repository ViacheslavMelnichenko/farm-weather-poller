using Microsoft.Extensions.Configuration;

namespace Farm.Weather.Poller.Configuration
{
    public class VaultConfigurationSource : IConfigurationSource
    {
        private readonly string _hostingEnvironmentEnvironmentName;

        public VaultConfigurationSource(string hostingEnvironmentEnvironmentName)
        {
            _hostingEnvironmentEnvironmentName = hostingEnvironmentEnvironmentName;
        }

        public IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            return new VaultConfigurationProvider(_hostingEnvironmentEnvironmentName);
        }
    }
}