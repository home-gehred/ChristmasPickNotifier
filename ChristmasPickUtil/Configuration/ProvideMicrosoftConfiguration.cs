using ChristmasPickCommon.Configuration;
using Microsoft.Extensions.Configuration;

namespace ChristmasPickUtil.Configuration
{
    public class ProvideMicrosoftConfiguration : IProvideConfiguration
    {
        private readonly IConfiguration configuration;
        public ProvideMicrosoftConfiguration(IConfiguration msCfg)
        {
            configuration = msCfg ?? throw new ArgumentNullException(nameof(msCfg));
        }

        public string GetConfiguration(string configurationKey)
        {
            var value = configuration[configurationKey];
            return value ?? throw new ArgumentNullException($"The configuration key <{configurationKey}> is not found.");
        }
    }

}
