using System.Diagnostics;
using System.Threading.Tasks;
using ChristmasPickMessages;
using ChristmasPickMessages.Messages;
using ChristmasPickNotifier.Notifier.Email;
using Xunit;
using Xunit.Abstractions;
using xUnitTestSecrets;

namespace ChristmasPickNotifier.uTest.Notifier
{
    /// <summary>
    /// I do not pay for SendGrid but may some day and want to keep the tests.
    /// </summary>
    [Collection("Secrets")] 
    public class SendGridPlainTextEmailTests : IClassFixture<SecretsConfiguration>
    {
        private readonly string _sendGridApiKey;
        public SendGridPlainTextEmailTests(ITestOutputHelper logger, SecretsConfiguration cfg)
        {
            Debugger.Break();
            cfg.DefaultConfiguration();
            _sendGridApiKey = cfg.GetSecret("sendgridApiKey");
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

        // dotnet test --configuration debug --filter FullyQualifiedName~ChristmasPickNotifier.uTest.Notifier.SendGridPlainTextEmailTests.GivenNotifyPickMessageWhenMail
        [Fact(Skip = "Sendgrid no long supports free sending of emails")]
        public async Task GivenNotifyPickMessageWhenMailTesterIsUsedThenEmailIsSent()
        {
            // Arrange
            var sut = new SendGridNotifyPickIsAvalable(_sendGridApiKey);
            var content = CreateTestEmailMessage("test-xdq1u@srv1.mail-tester.com");
            var testEnvelope = new Envelope(content);
            // Act
            var actual = await sut.Notify(testEnvelope);
            // Assert
            Assert.True(actual.IsSuccess(), $"Expected email to send but it failed. <{actual.Message}>");
        }

        //ins-uhnyhrmj@isnotspam.com
        // To view report http://isnotspam.com/newlatestreport.php?email=ins-uhnyhrmj%40isnotspam.com
        [Fact(Skip = "Sendgrid no long supports free sending of emails")]
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

        // dotnet test --configuration debug --filter FullyQualifiedName~ChristmasPickNotifier.uTest.Notifier.SendGridPlainTextEmailTests.GivenNotifyPickMessageWhenGmailIsUsedThenEmailIsSent
        [Fact(Skip = "Sendgrid no long supports free sending of emails")]
        public async Task GivenNotifyPickMessageWhenGmailIsUsedThenEmailIsSent()
        {
            Debugger.Break();
            // Arrange
            var sut = new SendGridNotifyPickIsAvalable(_sendGridApiKey);
            var content = CreateTestEmailMessage("ironmoose12@gmail.com");
            var testEnvelope = new Envelope(content);
            // Act
            var actual = await sut.Notify(testEnvelope);
            // Assert
            Assert.True(actual.IsSuccess(), $"Expected email to send but it failed. <{actual.Message}>");
        }

        [Theory(Skip = "Sendgrid no long supports free sending of emails")]
        [InlineData(new object[] { "cgehred@icloud.com" })]
        [InlineData(new object[] { "gehredp@gmail.com" })]
        [InlineData(new object[] { "ragsn3@gmail.com" })]
        [InlineData(new object[] { "meggella2001@yahoo.com" })]
        public async Task GivenNotifyPickMessageWhenVariousEmailThenEmailsAreSent(string testEmailAddress)
        {
            // Arrange
            var sut = new SendGridNotifyPickIsAvalable(_sendGridApiKey);
            var content = CreateTestEmailMessage(testEmailAddress);
            var testEnvelope = new Envelope(content);
            // Act
            var actual = await sut.Notify(testEnvelope);
            // Assert
            Assert.True(actual.IsSuccess(), $"Expected email to send but it failed. <{actual.Message}>");
        }

    }
}
