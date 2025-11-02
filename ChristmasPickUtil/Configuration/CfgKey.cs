namespace ChristmasPickUtil.Configuration
{
    public class CfgKey
    {
        public readonly static CfgKey FamilyContacts = new CfgKey("Values:familyContacts");
        public readonly static CfgKey FamilyPath = new CfgKey("Values:familyArchivePath");
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

}
