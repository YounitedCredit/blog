using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataGenerator.Data.Documents;
using DataGenerator.Documents;
using Microsoft.Azure.Cosmos;

namespace DataGenerator.Data
{
    public class Repository : IRepository
    {
        private readonly Database _database;

        public const string DatabaseName = "Store";
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

        public async Task AddToCustomerRecentPurchasesAsync(Purchase purchase)
        {
            var container = _database.GetContainer(CustomersContainerName);

            var customer = (await container.ReadItemAsync<Customer>(purchase.CustomerId.ToString(),
                new PartitionKey(purchase.CustomerId.ToString()))).Resource;

            var customerLastPurchases = customer.LastPurchases == null ? new List<Purchase>() : customer.LastPurchases.ToList();

            customerLastPurchases.Add(purchase);
            customer.LastPurchases = customerLastPurchases;

            await container.ReplaceItemAsync(customer, customer.Id.ToString(), new PartitionKey(customer.Id.ToString()));
        }
    }
}
