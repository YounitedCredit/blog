using System;
using System.Collections.Generic;

namespace DataGenerator
{
    public class PurchaseBuilder
    {
        private readonly IPurchaseFaker _faker;

        public PurchaseBuilder(IPurchaseFaker faker)
        {
            _faker = faker;
        }
        public List<Purchase> Build(in int expectedCount)
        {
            var purchases = new List<Purchase>();

            for (var i = 0; i < expectedCount; i++)
            {
                purchases.Add(_faker.BuildFake());
            }

            return purchases;
        }
    }
}
