using System.Collections.Generic;
using System.Threading.Tasks;
using DataGenerator.Data;
using Microsoft.Azure.Documents;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace DataGenerator.Function
{
    public static class ChangeFeedFunction
    {
        [FunctionName("PurchasesTrigger")]
        public static async Task RunAsync([CosmosDBTrigger(
                                              databaseName: Repository.DatabaseName,
                                              collectionName: Repository.PurchasesContainerName,
                                              ConnectionStringSetting = "CosmosDBConnection",
                                              CreateLeaseCollectionIfNotExists = true)]
                                          IReadOnlyList<Document> input, ILogger log)
        {
            if (input != null && input.Count > 0)
            {
                log.LogInformation("Documents modified " + input.Count);
                log.LogInformation("First document Id " + input[0].Id);
            }
        }
    }
}
