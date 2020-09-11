using System;
using DataGenerator.Data.Documents;
using Newtonsoft.Json;

namespace DataGenerator.Documents
{
    public class Customer
    {
        [JsonProperty("id")]
        public Guid Id;

        public string someString;

        public Purchase[] LastPurchases;
    }
}
