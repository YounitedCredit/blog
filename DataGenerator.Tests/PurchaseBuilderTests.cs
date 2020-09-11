using DataGenerator.Data.Documents;
using DataGenerator.DataBuilders;
using DataGenerator.Documents;
using NSubstitute;
using Xunit;

namespace DataGenerator.Tests
{
    public class PurchaseBuilderTests
    {
        [Theory]
        [InlineData(4)]
        [InlineData(9)]
        public void Should_Create_X_Purchases_Using_Faker_With_Sample_Customer_Ids(int expectedCount)
        {
            // Arrange.
            var purchaseFaker = Substitute.For<IFaker>();

            var purchaseBuilder = new PurchaseBuilder(purchaseFaker);
            purchaseFaker.BuildFake<Purchase>().Returns(new Purchase());

            // Act.
            var actualPurchases = purchaseBuilder.Build(expectedCount);

            // Assert.
            var expectedCustomerIds = SampleData.MockCustomerIds;
            purchaseFaker.Received(expectedCount).BuildFake<Purchase>();
            Assert.All(actualPurchases, _ => Assert.Contains(_.CustomerId, expectedCustomerIds));
        }
    }
}
