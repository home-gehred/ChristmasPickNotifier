using System;
using Microsoft.Extensions.Configuration;


namespace ChristmasPickNotifier.Notifier.ConfigurationProviders
{
    public class DefaultConfigurationProvider : IProvideConfiguration
    {
        private readonly IConfiguration _configuration; 
        public DefaultConfigurationProvider(IConfiguration configuration)
        { 
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public string GetConfiguration(string configurationKey)
        {
            if (string.IsNullOrEmpty(configurationKey))
            {
                throw new ArgumentNullException(nameof(configurationKey));
            }
            var keyValue = _configuration[configurationKey];
            if (string.IsNullOrEmpty(keyValue)) {
                throw new ApplicationException($"Unable to get value for {configurationKey}. Check dotnet user-secrets list and other configuration settings.");
            }
            return keyValue;
        }
    }
}
