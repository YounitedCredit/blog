using System;
using System.Linq;
using System.Threading.Tasks;
using DataGenerator.Data;
using DataGenerator.Data.Documents;
using DataGenerator.Documents;
using Microsoft.Azure.Cosmos;
using NSubstitute;
using Xunit;

namespace DataGenerator.Tests
{
    public class RepositoryTests
    {
        [Fact]
        public async Task Should_Add_Purchase_To_Container_When_Called()
        {
            // Arrange.
            // NSubstitute works with virtual and abstract methods too. Not just interface.
            var databaseFake = Substitute.For<Database>();
            var containerMock = Substitute.For<Container>();
            databaseFake.GetContainer(Repository.PurchasesContainerName).Returns(containerMock);

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
            await new Repository(databaseFake).CreatePurchaseDocumentAsync(expectedPurchase);

            // Assert.
            await containerMock.Received(1).CreateItemAsync(Arg.Any<Purchase>(), Arg.Any<PartitionKey>());
            Assert.Equal(expectedPurchase, actualPurchase);
            Assert.StrictEqual(expectPartitionKey, actualPartitionKey);
        }

        [Fact]
        public async Task Should_Add_Customer_To_Container_When_Called()
        {
            // Arrange.
            // NSubstitute works with virtual and abstract methods too. Not just interface.
            var databaseFake = Substitute.For<Database>();
            var containerMock = Substitute.For<Container>();
            databaseFake.GetContainer(Repository.CustomersContainerName).Returns(containerMock);

            var expectedCustomer = new Customer
            {
                Id = new Guid(),
                someString = "that",
                LastPurchases = null
            };
            var expectPartitionKey = new PartitionKey(expectedCustomer.Id.ToString());

            // For test result readability I want my assertions on the parameters received by my mock.
            // Using NSubstitute may provided less clear error messages.
            Customer actualCustomer = null;
            PartitionKey actualPartitionKey = default;
            await containerMock.CreateItemAsync(Arg.Do<Customer>(_ => actualCustomer = _),
                Arg.Do<PartitionKey>(_ => actualPartitionKey = _));

            // Act.
            await new Repository(databaseFake).CreateCustomerDocumentAsync(expectedCustomer);

            // Assert.
            await containerMock.Received(1).CreateItemAsync(Arg.Any<Customer>(), Arg.Any<PartitionKey>());
            Assert.Equal(expectedCustomer, actualCustomer);
            Assert.StrictEqual(expectPartitionKey, actualPartitionKey);
        }
        [Fact]
        public async Task Should_Add_Recent_Purchases_To_Customer()
        {
            // Arrange.
            // NSubstitute works with virtual and abstract methods too. Not just interface.
            var databaseFake = Substitute.For<Database>();
            var containerMock = Substitute.For<Container>();
            databaseFake.GetContainer(Repository.CustomersContainerName).Returns(containerMock);

            var customer = new Customer
            {
                Id = new Guid(),
                someString = "that",
                LastPurchases = null
            };

            var newPurchase = new Purchase
            {
                FictionalArticleNumber = Guid.NewGuid(),
                ArticleName = "ThatItem",
                CustomerId = Guid.NewGuid(),
                TransactionDate = new DateTime(2020, 10, 01),
                PurchaseId = Guid.NewGuid()
            };

            await containerMock.ReadItemAsync<Customer>(Arg.Is<string>(_ => _ == newPurchase.CustomerId.ToString()),
                Arg.Is<PartitionKey>(_ => _.ToString() == newPurchase.CustomerId.ToString()));

            // Act.
            await new Repository(databaseFake).AddToCustomerRecentPurchasesAsync(newPurchase);

            // Assert.
            await containerMock.Received(1).ReplaceItemAsync(Arg.Is<Customer>(_ => _.LastPurchases.Last().PurchaseId == newPurchase.PurchaseId),
                Arg.Is<string>(_ => _ == customer.Id.ToString()),
                Arg.Is<PartitionKey>( _ => _.ToString() == new PartitionKey(customer.Id.ToString()).ToString()));
        }
    }
}
