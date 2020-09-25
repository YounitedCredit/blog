using System.Collections.Generic;
using System.Threading.Tasks;
using DataGenerator.Data;
using DataGenerator.Data.Documents;
using DataGenerator.Function;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace DataGenerator.Function
{
    public class ChangeFeedFunction
    {
        private readonly IRepository _fakeRepository;

        public ChangeFeedFunction(IRepository fakeRepository)
        {
            _fakeRepository = fakeRepository;
        }

        [FunctionName("PurchasesTrigger")]
        public async Task RunAsync([CosmosDBTrigger(
                                              databaseName: Repository.DatabaseName,
                                              collectionName: Repository.PurchasesContainerName,
                                              ConnectionStringSetting = "CosmosDBConnection",
                                              CreateLeaseCollectionIfNotExists = true)]
                                          IReadOnlyList<Document> input, ILogger log)
        {
            log.LogInformation("Documents modified " + input.Count);
            if (input != null && input.Count > 0)
            {
                foreach (var purchaseDocument in input)
                {
                    var purchase = JsonConvert.DeserializeObject<Purchase>(purchaseDocument.ToString());

                    _fakeRepository.AddToCustomerRecentPurchasesAsync(purchase);
                }
            }
        }
    }
}
