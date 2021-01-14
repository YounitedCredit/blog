using System;
using System.Collections.Generic;
using DataGenerator.Data;
using DataGenerator.Data.Documents;
using DataGenerator.Function;
using Microsoft.Azure.Documents;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NSubstitute;
using Xunit;

namespace DataGenerator.Tests.Function
{
    public class ChangeFeedFunctionTests
    {
        [Fact]
        public void Should_Add_Purchase_To_Customer_Document()
        {
            //Arrange.
            var testInputPurchase = new Purchase
            {
                FictionalArticleNumber = Guid.NewGuid(),
                ArticleName = "ThatItem",
                CustomerId = Guid.NewGuid(),
                TransactionDate = new DateTime(2020, 10, 01),
                PurchaseId = Guid.NewGuid()
            };
            var dynamicDoc = JsonConvert.DeserializeObject<dynamic>(JsonConvert.SerializeObject(testInputPurchase));

            using JsonReader reader = new JTokenReader(dynamicDoc);
            var document = new Document();
            document.LoadFrom(reader);

            var fakeRepository = Substitute.For<IRepository>();

            // Act.
            new ChangeFeedFunction(fakeRepository).RunAsync(new List<Document>{ document }, Substitute.For<ILogger>());

            // Assert.
            fakeRepository.Received(1).AddToCustomerRecentPurchasesAsync(Arg.Is<Purchase>(_ => _.PurchaseId == testInputPurchase.PurchaseId));
        }
    }
}
