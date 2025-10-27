using ChristmasPickCommon.Configuration;

namespace ChristmasPickUtil.Configuration
{
    public class ChristmasPickUtilConfiguration : IChristmasPickUtilConfiguration
    {
        private readonly IProvideConfiguration _cfgProvider;
        public ChristmasPickUtilConfiguration(IProvideConfiguration provideConfiguration)
        {
            _cfgProvider = provideConfiguration;
        }
        public string GetConfiguration(CfgKey configurationKey)
        {
            return _cfgProvider.GetConfiguration(configurationKey);
        }
    }
}
