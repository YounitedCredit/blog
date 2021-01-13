using System;
using System.Collections.Generic;
using System.Linq;
using DataGenerator.Data.Documents;

namespace DataGenerator.DataBuilders
{
    public class PurchaseBuilder
    {
        private readonly IFaker _faker;

        public PurchaseBuilder(IFaker faker)
        {
            _faker = faker;
        }
        public IReadOnlyCollection<Purchase> Build(in int expectedCount)
        {
            var purchases = new List<Purchase>();

            var random = new Random();
            for (var i = 0; i < expectedCount; i++)
            {
                var purchase = _faker.BuildFake<Purchase>();
                int index = random.Next(SampleData.MockCustomerIds.Count);
                purchase.CustomerId = SampleData.MockCustomerIds.ElementAt(index);

                purchases.Add(purchase);
            }

            return purchases;
        }
    }
}
