using System;
using Newtonsoft.Json;

namespace DataGenerator.Data.Documents
{
    public class Purchase
    {
        public Guid FictionalArticleNumber { get; set; }
        public string ArticleName { get; set; }
        public Guid CustomerId { get; set; }
        public DateTimeOffset TransactionDate { get; set; }

        [JsonProperty("id")]
        public Guid PurchaseId { get; set; }
    }
}
