using System;
using System.Collections.Generic;
using ChristmasPickMessages;
using ChristmasPickMessages.Messages;
using ChristmasPickNotifier.Notifier.NotifierFactories;

namespace ChristmasPickNotifier.Notifier
{
    public interface IProvideConfiguration
    {
        string GetConfiguration(string configurationKey);
    }

    public interface INotifierFactory
    {
        INotifier Create(IEnvelope queueItem);
    }

    public class NotifierFactory : INotifierFactory
    {
        private readonly IDictionary<string, INotifierFactory> _registry;
        public NotifierFactory(IProvideConfiguration cfgProvider)
        {
            if (cfgProvider == null)
            {
                throw new ArgumentNullException(nameof(cfgProvider));
            }
            _registry = new Dictionary<string, INotifierFactory>();
            _registry.Add(typeof(PickAvailableMessage).FullName, new SendGridEmailFactory(cfgProvider));
        }

        public INotifier Create(IEnvelope queueItem)
        {
            var contentType = queueItem.PayloadType;
            if (_registry.ContainsKey(contentType))
            {
                return _registry[contentType].Create(queueItem);
            }
            return new NoOpNotifier();
        }
    }

}
