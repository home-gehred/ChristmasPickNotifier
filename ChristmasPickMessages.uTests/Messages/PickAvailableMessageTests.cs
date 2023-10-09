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
            Assert.Equal(sut.HtmlBody, actual.HtmlBody);
            Assert.Equal(sut.PlainTextBody, actual.PlainTextBody);
            Assert.Equal(sut.Name, actual.Name);
            Assert.Equal(sut.Subject, actual.Subject);
            Assert.Equal(sut.ToAddress, actual.ToAddress);
            Assert.Equal((string)sut.NotificationType, (string)actual.NotificationType);
        }
    }
}
