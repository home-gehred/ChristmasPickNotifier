using System;
using ChristmasPickMessages.Messages;
using Newtonsoft.Json;
using Xunit;

namespace ChristmasPickMessages.uTests
{
    public class EnvelopeTests
    {
       [Fact]
        public void MessageEnvelopeSerializeDeserialize()
        {
            // Arrange
            var envelopeContent = new PickAvailableMessage();
            envelopeContent.NotificationType = NotifyType.Email;
            var sut = new Envelope(envelopeContent);

            // Act
            var serialized = JsonConvert.SerializeObject(sut);

            var envelope = JsonConvert.DeserializeObject<Envelope>(serialized);
            var actual = JsonConvert.DeserializeObject<PickAvailableMessage>(envelope.Payload);

            // Assert
            Assert.Equal<string>(envelopeContent.NotificationType, actual.NotificationType);
        }
    }
}
