using System;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;
using NSubstitute;
using Xunit;

namespace DataGenerator.Tests
{
    public class PurchaseRepositoryTests
    {
        [Fact]
        public async Task Should_Add_Purchase_To_Container_When_Called()
        {
            // Arrange.
            // NSubstitute works with virtual and abstract methods too. Not just interface.
            var databaseFake = Substitute.For<Database>();
            var containerMock = Substitute.For<Container>();
            databaseFake.GetContainer(PurchaseRepository.ContainerName).Returns(containerMock);

            var expectedPurchase = new Purchase
            {
                FictionalArticleNumber = Guid.NewGuid(),
                ArticleName = "Jafar's lamp",
                CustomerId = Guid.NewGuid(),
                TransactionDate = new DateTimeOffset(2020, 04, 23, 11, 12, 00,TimeSpan.FromHours(2)),
                PurchaseId = Guid.NewGuid()
            };
            var expectPartitionKey = new PartitionKey(expectedPurchase.FictionalArticleNumber.ToString());

            // For test result readability I want my assertions on the parameters received by my mock.
            // Using NSubstitute may provided less clear error messages.
            Purchase actualPurchase = null;
            PartitionKey actualPartitionKey = default;
            await containerMock.CreateItemAsync(Arg.Do<Purchase>(_ => actualPurchase = _),
                Arg.Do<PartitionKey>(_ => actualPartitionKey = _));

            // Act.
            await new PurchaseRepository(databaseFake).CreatePurchaseDocumentAsync(expectedPurchase);

            // Assert.
            await containerMock.Received(1).CreateItemAsync(Arg.Any<Purchase>(), Arg.Any<PartitionKey>());
            Assert.Equal(expectedPurchase, actualPurchase);
            Assert.StrictEqual(expectPartitionKey, actualPartitionKey);
        }
    }
}
