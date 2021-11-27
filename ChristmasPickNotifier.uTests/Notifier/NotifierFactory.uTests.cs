using ChristmasPickMessages;
using ChristmasPickMessages.Messages;
using ChristmasPickNotifier.Notifier;
using ChristmasPickNotifier.Notifier.Email;
using ChristmasPickCommon.Configuration;
using Moq;
using Xunit;

namespace ChristmasPickNotifier.uTest.Notifier
{
    public class NotifierFactoryTests
    {
        public INotifierFactory CreateTestSubject(IProvideConfiguration cfg)
        {
            return new NotifierFactory(cfg);
        }

        [Fact]
        public void GivenNotifierFactoryWhenEnvelopeIsUnrecognizedThenReturnNoOpNotifier()
        {
            // Arrange
            var testCfg = new Mock<IProvideConfiguration>();
            var sut = CreateTestSubject(testCfg.Object);
            var testEnvelope = new Mock<IEnvelope>();
            testEnvelope.Setup(env => env.PayloadType).Returns("Not A real Type");
            // Act
            var actual = sut.Create(testEnvelope.Object);
            // Assert
            Assert.IsType<NoOpNotifier>(actual);
        }

        [Fact]
        public void GivenNotifierFactoryWhenEnvelopeIsNotifierMessageThenReturnNotifierMessageFactory()
        {
            // Arrange
            var testCfg = new Mock<IProvideConfiguration>();
            testCfg.Setup(c => c.GetConfiguration("sendgrid-api-key")).Returns("thekey");
            var sut = CreateTestSubject(testCfg.Object);
            var testEnvelope = new Mock<IEnvelope>();
            testEnvelope.Setup(env => env.PayloadType).Returns(typeof(PickAvailableMessage).FullName);
            // Act
            var actual = sut.Create(testEnvelope.Object);
            // Assert
            Assert.IsType<SendGridNotifyPickIsAvalable>(actual);
        }

    }
}
