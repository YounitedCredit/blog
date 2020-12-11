using System;
using System.Collections.Generic;
using DataGenerator.Data.Documents;
using Newtonsoft.Json;

namespace DataGenerator.Documents
{
    public class Customer
    {
        [JsonProperty("id")]
        public Guid Id;

        public IEnumerable<Purchase> LastPurchases;
    }
}
