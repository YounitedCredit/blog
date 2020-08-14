using System.Linq;
using Xunit;

namespace DataGenerator.Tests
{
    public class PurchaseBuilderTests
    {
        [Theory]
        [InlineData(4)]
        [InlineData(9)]
        public void Should_Create_X_Purchases_When_Called(int expectedCount)
        {
            // Arrange.
            var purchaseBuilder = new PurchaseBuilder();

            // Act.
            var actual = purchaseBuilder.Build(expectedCount);

            // Assert.
            Assert.Equal(expectedCount, actual.Count);
        }
    }
}
