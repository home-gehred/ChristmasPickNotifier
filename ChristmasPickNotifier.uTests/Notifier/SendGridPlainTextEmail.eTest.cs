using System.Threading.Tasks;
using ChristmasPickMessages;
using ChristmasPickMessages.Messages;
using ChristmasPickNotifier.Notifier.Email;
using Newtonsoft.Json;
using Xunit;
using Xunit.Abstractions;
using xUnitTestSecrets;

namespace ChristmasPickNotifier.uTest.Notifier
{
    [Collection("Secrets")]
    public class SendGridPlainTextEmailTests : IClassFixture<SecretsConfiguration>
    {
        private readonly string _sendGridApiKey;
        public SendGridPlainTextEmailTests(ITestOutputHelper logger, SecretsConfiguration cfg)
        {
            cfg.DefaultConfiguration();
            _sendGridApiKey = cfg.GetSecret("sendgrigApiKey");
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

        // http://www.mail-tester.com/
        // Your goal tomorrow is to get this to pass
        // http://www.mail-tester.com/test-xdq1u
        // 11/11/2018 7.2 out of 10
        // 11/10/2018 your score is 5.3 out of 10
        [Fact]
        public async Task GivenNotifyPickMessageWhenMailTesterIsUsedThenEmailIsSent()
        {
            // Arrange
            var sut = new SendGridNotifyPickIsAvalable(_sendGridApiKey);
            var content = CreateTestEmailMessage("test-xdq1u@mail-tester.com");
            var testEnvelope = new Envelope(content);
            // Act
            var actual = await sut.Notify(testEnvelope);
            // Assert
            Assert.True(actual.IsSuccess(), $"Expected email to send but it failed. <{actual.Message}>");
        }

        //ins-uhnyhrmj@isnotspam.com
        // To view report http://isnotspam.com/newlatestreport.php?email=ins-uhnyhrmj%40isnotspam.com
        [Fact]
        public async Task GivenNotifyPickMessageWhenIsNotSpamIsUsedThenEmailIsSent()
        {
            // Arrange
            var sut = new SendGridNotifyPickIsAvalable(_sendGridApiKey);
            var content = CreateTestEmailMessage("ins-uhnyhrmj@isnotspam.com");
            var testEnvelope = new Envelope(content);
            // Act
            var actual = await sut.Notify(testEnvelope);
            // Assert
            Assert.True(actual.IsSuccess(), $"Expected email to send but it failed. <{actual.Message}>");
        }

        [Fact]
        public async Task GivenNotifyPickMessageWhenGmailIsUsedThenEmailIsSent()
        {
            // Arrange
            var sut = new SendGridNotifyPickIsAvalable(_sendGridApiKey);
            var content = CreateTestEmailMessage("ironmoose12@gmail.com");
            var testEnvelope = new Envelope(content);
            // Act
            var actual = await sut.Notify(testEnvelope);
            // Assert
            Assert.True(actual.IsSuccess(), $"Expected email to send but it failed. <{actual.Message}>");
        }

    }
}
