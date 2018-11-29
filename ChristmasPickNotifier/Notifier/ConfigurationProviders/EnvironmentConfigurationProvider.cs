using System;
namespace ChristmasPickNotifier.Notifier.ConfigurationProviders
{
    public class EnvironmentConfigurationProvider : IProvideConfiguration
    {
        public string GetConfiguration(string configurationKey)
        {
            return GetEnvironmentVariable(configurationKey); 
        }

        private string GetEnvironmentVariable(string name)
        {
            return Environment.GetEnvironmentVariable(name, EnvironmentVariableTarget.Process);
        }
    }
}
