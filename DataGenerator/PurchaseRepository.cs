using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;

namespace DataGenerator
{
    public class PurchaseRepository
    {
        private Container _container;
        public const string ContainerName = "Purchases";

        public PurchaseRepository(Database database)
        {
            _container = database.GetContainer(ContainerName);
        }


        public async Task CreatePurchaseDocumentAsync(Purchase purchase)
        {
            await _container.CreateItemAsync(purchase, new PartitionKey(purchase.FictionalArticleNumber.ToString()));        }
    }
}
