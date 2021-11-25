using System;
using Xunit;
using Common.ChristmasPickList;

namespace ChristmasPickCommon.uTests
{
    public class XMasDayFixture
    {
        [Theory]
        [InlineData(new object[]{1970})]
        [InlineData(new object[]{1971})]
        [InlineData(new object[]{3001})]
        public void ShouldThrowWithInvalidYear(int invalidYear)
        {
            // Arrange
            // Act
            // Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => {
                new XMasDay(invalidYear);
            });
            
        }
        [Fact]
        public void ShouldHaveValidChristmasDay()
        {
            // Arrange
            var sut = new XMasDay(2021);
            // Act
            DateTime actual = sut;
            // Assert
            Assert.Equal(DateTimeKind.Utc, actual.Kind);
            Assert.Equal(2021, actual.Year);
            Assert.Equal(12, actual.Month);
            Assert.Equal(25, actual.Day);
            Assert.Equal(0, actual.Hour);
            Assert.Equal(0, actual.Minute);
            Assert.Equal(0, actual.Second);
        }
    }
}
