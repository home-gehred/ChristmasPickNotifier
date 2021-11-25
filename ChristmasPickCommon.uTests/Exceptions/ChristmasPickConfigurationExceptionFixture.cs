using System;
using System.Threading;
using System.Threading.Tasks;
using ChristmasPickCommon.Exceptions;
using Xunit;

namespace ChristmasPickCommon.utest.Exceptions
{
    public class ChristmasPickConfigurationExceptionFixture
    {
        [Fact]
        public async Task ShouldThrowExceptionWithProperMessage()
        {
            var actual = await Assert.ThrowsAsync<ChristmasPickConfigurationException>(() => {
                    throw new ChristmasPickConfigurationException("test:keyname");
            });

            Assert.Equal("The configuration setting test:keyname was not found. Please check the configuration of application.",
                actual.Message);
        }

    }
}
