using ChristmasPickMessages.Messages;
using Newtonsoft.Json;
using Xunit;

namespace ChristmasPickMessages.uTests.Messages
{
    public class PickAvailableMessageTests
    {
        [Fact]
        public void GivenAMessageWhenSerializedThenItDeserilzes()
        {
            // Arrange
            var sut = new PickAvailableMessage
            {
                HtmlBody = "<html><body>Hello World!</body></html>",
                PlainTextBody = "HelloWorld",
                Name = "bill",
                Subject = "test email subject",
                ToAddress = "bill@wonderland.com",
                NotificationType = NotifyType.Email
            };
            var sutAsString = JsonConvert.SerializeObject(sut);
            // Act
            var actual = JsonConvert.DeserializeObject<PickAvailableMessage>(sutAsString);
            // Assert
            Assert.Equal<string>(sut.HtmlBody, actual.HtmlBody);
            Assert.Equal<string>(sut.PlainTextBody, actual.PlainTextBody);
            Assert.Equal<string>(sut.Name, actual.Name);
            Assert.Equal<string>(sut.Subject, actual.Subject);
            Assert.Equal<string>(sut.ToAddress, actual.ToAddress);
            Assert.Equal<string>(sut.NotificationType, actual.NotificationType);
        }
    }
}
