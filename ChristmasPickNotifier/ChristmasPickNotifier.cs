using System;
using ChristmasPickMessages;
using ChristmasPickNotifier.Notifier;
using ChristmasPickNotifier.Notifier.ConfigurationProviders;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace ChristmasPickNotifier
{
    public static class ChristmasPickNotifier
    {
        [FunctionName("ChristmasPickNotifier")]
        public static void Run([QueueTrigger("notifier-q", Connection = "storage-connection")]string myQueueItem, ILogger logger)
        {
            var cfg = new EnvironmentConfigurationProvider();
            INotifierFactory factory = new NotifierFactory(cfg);
            logger.LogInformation($"C# Queue trigger function processed: {myQueueItem}");
            try
            {
                var envelope = JsonConvert.DeserializeObject<Envelope>(myQueueItem);
                var notifier = factory.Create(envelope);

                var result = notifier.Notify(envelope).GetAwaiter().GetResult();
                if (result.IsSuccess())
                {
                    logger.LogInformation($"Send Grid should have the goods.");
                }
                else
                {
                    logger.LogError($"Unable to send message: {result.Message}");
                }
            }
            catch (Exception ex)
            {
                logger.LogError("Encountered an error", ex);
            }
        }
    }
}
