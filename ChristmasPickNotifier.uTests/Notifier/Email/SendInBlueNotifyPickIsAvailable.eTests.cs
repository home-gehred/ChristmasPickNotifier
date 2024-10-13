using System;
using System.Diagnostics;
using System.Threading.Tasks;
using ChristmasPickMessages;
using ChristmasPickMessages.Messages;
using ChristmasPickNotifier.Notifier.Email;
using Xunit;
using Xunit.Abstractions;
using xUnitTestSecrets;

namespace ChristmasPickNotifier.uTests.Notifier.Email
{
    [Collection("Secrets")]
    public class SendInBlueNotifyPickIsAvailableTests : IClassFixture<SecretsConfiguration>
    {
        private readonly string _sendInBlueApiKey;
        private readonly ITestOutputHelper _logger;
        public SendInBlueNotifyPickIsAvailableTests(ITestOutputHelper logger, SecretsConfiguration cfg)
        {
            Debugger.Break();
            cfg.DefaultConfiguration();
            _sendInBlueApiKey = cfg.GetSecret("sendInBlueApiKey");
            _logger = logger;
        }

        private PickAvailableMessage CreateTestEmailMessage(string To)
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
            return content;
        }

        // dotnet test --configuration debug --filter ChristmasPickNotifier.uTests.Notifier.Email.SendInBlueNotifyPickIsAvailableTests.GivenNotifyPickMessageWhenGmailIsUsedThenEmailIsSent
        [Fact]
        public async Task GivenNotifyPickMessageWhenGmailIsUsedThenEmailIsSent()
        {
            _logger.WriteLine($"Hey Bob! {_sendInBlueApiKey}");
            // Arrange
            var sut = new SendInBlueNotifyPickIsAvailable(_sendInBlueApiKey);
            var content = CreateTestEmailMessage("ironmoose12@gmail.com");
            var testEnvelope = new Envelope(content);
            // Act
            var actual = await sut.Notify(testEnvelope);
            // Assert
            Assert.True(actual.IsSuccess(), $"Expected email to send but it failed. <{actual.Message}>");
        }

        //ins-uhnyhrmj@isnotspam.com
        // To view report http://isnotspam.com/newlatestreport.php?email=ins-uhnyhrmj%40isnotspam.com
        // dotnet test --configuration debug --filter ChristmasPickNotifier.uTests.Notifier.Email.SendInBlueNotifyPickIsAvailableTests.GivenNotifyPickMessageWhenIsNotSpamIsUsedThenEmailIsSent
        [Fact]
        public async Task GivenNotifyPickMessageWhenIsNotSpamIsUsedThenEmailIsSent()
        {
            // Arrange
            var sut = new SendInBlueNotifyPickIsAvailable(_sendInBlueApiKey);
            var content = CreateTestEmailMessage("ins-uhnyhrmj@isnotspam.com");
            var testEnvelope = new Envelope(content);
            // Act
            var actual = await sut.Notify(testEnvelope);
            // Assert
            Assert.True(actual.IsSuccess(), $"Expected email to send but it failed. <{actual.Message}>");
        }

    }
}
