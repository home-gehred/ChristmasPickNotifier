using System;
using Xunit;

namespace ChristmasPickCommon.uTests
{
    public class XMasPickListTypeFixture
    {
        [Theory]
        [InlineData(new object[]{null, false})]
        [InlineData(new object[]{"", false})]
        [InlineData(new object[]{"AdUlT", true})]
        [InlineData(new object[]{"KiD", true})]
        public void TryParseShouldHandleAllStrings(string testValue, bool expectedResult)
        {
            // Arrange
            // Act
            var actual = XMasPickListType.TryParse(testValue, out XMasPickListType listType);
            Assert.Equal(expectedResult, actual);
            if (actual)
            {
                if (testValue.ToLower() == "kid")
                {
                    Assert.Equal(XMasPickListType.Kid, listType);
                }
                if (testValue.ToLower() == "adult")
                {
                    Assert.Equal(XMasPickListType.Adult, listType);
                }
            }
        }
    }
}
