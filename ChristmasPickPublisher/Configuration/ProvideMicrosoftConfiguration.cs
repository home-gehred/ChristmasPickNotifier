using System;
using Microsoft.Extensions.Configuration;
using ChristmasPickNotifier.Notifier;
using ChristmasPickCommon.Configuration;

namespace ChristmasPickPublisher.Configuration
{
    public class CfgKey
    {
        public readonly static CfgKey FamilyContacts = new CfgKey("Values:familyContacts");
        public readonly static CfgKey AdultArchivePath = new CfgKey("Values:adultArchivePath");
        public readonly static CfgKey KidArchivePath = new CfgKey("Values:kidArchivePath");
        public readonly static CfgKey PathToEmailTemplate = new CfgKey("Values:pathToEmailTemplate");
        public readonly static CfgKey SendGridApiKey = new CfgKey("sendgrid-api-key");
        public readonly static CfgKey SendInBlueApiKey = new CfgKey("sendinblue-api-key");
        private readonly string name;
        private CfgKey(string configurationKey)
        {
            name = string.IsNullOrEmpty(configurationKey) == false ? configurationKey 
                : throw new ArgumentNullException(nameof(configurationKey));
        }

        public static implicit operator string(CfgKey cfgKey)
        {
            return cfgKey.name;
        }
    }
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
            if (value == null)
            {
                throw new ArgumentNullException($"The configuration key <{configurationKey}> is not found.");
            }
            return value;
        }
    }
}
