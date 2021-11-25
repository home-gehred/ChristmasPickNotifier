using System;
using Xunit;

namespace Common.Test
{
    public class ExchangeCheckSumFixture
    {
        private void VerifyDiagnosticMessage(int expectedIn, int expectedOut, string actualMsg)
        {
            Assert.Equal(string.Format("buying {0} present(s) and is recieving {1} present(s)", expectedOut, expectedIn), actualMsg);
        }

        [Fact]
        public void ConstructedCheckSumIsNotValid()
        {
            ExchangeCheckSum subject = new ExchangeCheckSum();
            Assert.False(subject.isValid());
            Assert.Equal("not buying or recieving a gift", subject.DiagnosticMessage());
        }

        [Fact]
        public void OnePresentInAndOnePresentOutWillBeValid()
        {
            ExchangeCheckSum subject = new ExchangeCheckSum();
            subject.updatePresentsIn();
            subject.updatePresentsOut();
            Assert.True(subject.isValid());
            Assert.Equal("correct", subject.DiagnosticMessage());
        }

        [Fact]
        public void OnePresentInAndNoPresentOutWillBeInvalid()
        {
            ExchangeCheckSum subject = new ExchangeCheckSum();
            subject.updatePresentsIn();
            Assert.False(subject.isValid());
            VerifyDiagnosticMessage(1, 0, subject.DiagnosticMessage());
        }

        [Fact]
        public void NoPresentInAndOnePresentOutWillBeInvalid()
        {
            ExchangeCheckSum subject = new ExchangeCheckSum();
            subject.updatePresentsOut();
            Assert.False(subject.isValid());
            VerifyDiagnosticMessage(0, 1, subject.DiagnosticMessage());
        }

        [Fact]
        public void MultiplePresentInAndMultiplePresentOutWillBeInvalid()
        {
            ExchangeCheckSum subject = new ExchangeCheckSum();
            subject.updatePresentsIn();
            subject.updatePresentsIn();
            subject.updatePresentsIn();
            subject.updatePresentsOut();
            subject.updatePresentsOut();
            subject.updatePresentsOut();
            Assert.False(subject.isValid());
            VerifyDiagnosticMessage(3, 3, subject.DiagnosticMessage());
        }

    }
}
