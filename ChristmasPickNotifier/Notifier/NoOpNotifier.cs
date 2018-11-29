using System.Threading.Tasks;
using ChristmasPickMessages;

namespace ChristmasPickNotifier.Notifier
{
    public class NoOpNotifier : INotifier
    {
        public Task<NotifierResult> Notify(IEnvelope message)
        {
            return Task.FromResult(NotifierResultFactory.Success);
        }
    }

}
