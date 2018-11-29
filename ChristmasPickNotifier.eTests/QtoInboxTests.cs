using System;
using System.Threading.Tasks;
using ChristmasPickMessages;
using ChristmasPickMessages.Messages;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using Newtonsoft.Json;
using Xunit;
using Xunit.Abstractions;
using xUnitTestSecrets;

namespace ChristmasPickNotifier.eTest
{
    public class QtoInboxTests : IClassFixture<SecretsConfiguration>, IDisposable
    {
        private readonly ITestOutputHelper _logger;
        private readonly CloudQueue _notifyQueue;
        public QtoInboxTests(ITestOutputHelper logger, SecretsConfiguration cfg)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            cfg.DefaultConfiguration();
            var storageAccountConnectionString = cfg.GetSecret("cloudQStorageAct");
            var storageAccount = CloudStorageAccount.Parse(storageAccountConnectionString);
            var queueClient = storageAccount.CreateCloudQueueClient();
            _notifyQueue = queueClient.GetQueueReference("notifier-q");
        }

        private IEnvelope CreateTestEmailMessage(string To)
        {
            var content = new PickAvailableMessage
            {
                HtmlBody = "<!DOCTYPE html><html><body>Hello world!<p>The intention of this email to understand what it will take to get email that is not considered spam coming from SendGrid. The journey has been extremely educational for me and I dare say enjoyable.</body></html> ",
                PlainTextBody = "Hello world!\r\nThe intention of this email to understand what it will take to get email that is not considered spam coming from SendGrid. The process or I should journey has been extremely educational for me and I dare say enjoyable.",
                Name = "C",
                NotificationType = NotifyType.Email,
                Subject = "Test Message",
                ToAddress = To
            };
            return new Envelope(content);
        }

        [Fact]
        public async Task GivenAValidEmailWhenPickAvailableMessageIsPlacedOnQThenEmailWillBeSent()
        {
            // Arrange
            // Put Content in Envelope
            var testMessage = CreateTestEmailMessage("ironmoose12@gmail.com");
            var message = new CloudQueueMessage(JsonConvert.SerializeObject(testMessage));
            // Act
            _logger.WriteLine("Putting notify message on Q");
            await _notifyQueue.AddMessageAsync(message);
            // Assert
            // How will you test this?
        }

        public void Dispose()
        {
        }
    }
}
