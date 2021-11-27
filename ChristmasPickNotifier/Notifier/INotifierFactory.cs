using System;
using System.Collections.Generic;
using ChristmasPickMessages;
using ChristmasPickMessages.Messages;
using ChristmasPickNotifier.Notifier.NotifierFactories;

namespace ChristmasPickNotifier.Notifier
{
    public interface INotifierFactory
    {
        INotifier Create(IEnvelope queueItem);
    }
}
