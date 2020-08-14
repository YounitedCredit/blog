using System.Collections.Generic;

namespace DataGenerator
{
    public class PurchaseBuilder
    {
        public List<Purchase> Build(in int expectedCount)
        {
            var purchases = new List<Purchase>();

            for (var i = 0; i < expectedCount; i++)
            {
                purchases.Add(new Purchase());
            }

            return purchases;
        }
    }
}
