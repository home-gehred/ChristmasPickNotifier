using System;
using ChristmasPickMessages;
using ChristmasPickNotifier.Notifier.Email;

namespace ChristmasPickNotifier.Notifier.NotifierFactories
{
    public class SendGridEmailFactory : INotifierFactory
    {
        private readonly IProvideConfiguration _cfgProvider;
        public SendGridEmailFactory(IProvideConfiguration configurationProvider)
        {
            _cfgProvider = configurationProvider ?? throw new ArgumentNullException(nameof(configurationProvider));
        }

        public INotifier Create(IEnvelope queueItem)
        {
            var sendGridApiKey = _cfgProvider.GetConfiguration("sendgrid-api-key");

            return new SendGridNotifyPickIsAvalable(sendGridApiKey);
        }
    }
}
