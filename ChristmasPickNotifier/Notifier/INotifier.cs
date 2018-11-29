using System.Threading.Tasks;
using ChristmasPickMessages;

namespace ChristmasPickNotifier.Notifier
{
    public interface INotifier
    {
        Task<NotifierResult> Notify(IEnvelope message);
    }
}
