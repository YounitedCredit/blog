using NSubstitute;
using Xunit;

namespace DataGenerator.Tests
{
    public class PurchaseBuilderTests
    {
        [Theory]
        [InlineData(4)]
        [InlineData(9)]
        public void Should_Create_X_Purchases_Using_Faker(int expectedCount)
        {
            // Arrange.
            var purchaseFaker = Substitute.For<IPurchaseFaker>();

            var purchaseBuilder = new PurchaseBuilder(purchaseFaker);

            // Act.
            purchaseBuilder.Build(expectedCount);

            // Assert.
            purchaseFaker.Received(expectedCount).BuildFake();

        }
    }
}
