using System;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using ChristmasPickNotifier.Notifier;
using Newtonsoft.Json;
using ChristmasPickMessages;

namespace SantasLilHelper
{
    public class ChristmasPickMessenger
    {
        private readonly INotifierFactory _notifierFactory;
        public ChristmasPickMessenger(INotifierFactory notifierFactory)
        {
            _notifierFactory = notifierFactory ?? throw new ArgumentNullException(nameof(notifierFactory));
        }

        [Function("ChristmasPickMessenger")]
        public async Task Run([QueueTrigger("notifier-q", Connection = "AzureWebJobsStorage")] string myQueueItem,
            FunctionContext context)
        {
            var logger = context.GetLogger("ChristmasPickMessenger");
            logger.LogInformation($"C# Queue trigger function processed: {myQueueItem}");
            var queueItemEnvelope = JsonConvert.DeserializeObject<Envelope>(myQueueItem);
            var notifier = _notifierFactory.Create(queueItemEnvelope);
            var result = await notifier.Notify(queueItemEnvelope);
            if (result.IsSuccess())
            {
                logger.LogInformation($"Send Grid should have the goods.");
            }
            else
            {
                logger.LogError($"Unable to send message: {result.Message}");
                throw new ApplicationException($"Unable to send message: {result.Message}");                
            }

        }
    }
}

// This example one worked putting IConfiguration
/*        private readonly IConfiguration _configuration;
        public ChristmasPickMessenger(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }
*/
// End example one