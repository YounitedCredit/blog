using System.Threading.Tasks;
using DataGenerator.Data.Documents;
using DataGenerator.Documents;
using Microsoft.Azure.Cosmos;

namespace DataGenerator.Data
{
    public class Repository
    {
        private readonly Database _database;
        public const string PurchasesContainerName = "Purchases";
        public const string CustomersContainerName = "Customers";

        public Repository(Database database)
        {
            _database = database;
        }


        public async Task CreatePurchaseDocumentAsync(Purchase purchase)
        {
            await _database.GetContainer(PurchasesContainerName).CreateItemAsync(purchase, new PartitionKey(purchase.FictionalArticleNumber.ToString()));
        }

        public async Task CreateCustomerDocumentAsync(Customer customer)
        {
            await _database.GetContainer(CustomersContainerName).CreateItemAsync(customer, new PartitionKey(customer.Id.ToString()));
        }
    }
}
