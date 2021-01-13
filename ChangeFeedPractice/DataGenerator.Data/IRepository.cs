using System.Threading.Tasks;
using DataGenerator.Data.Documents;
using DataGenerator.Documents;

namespace DataGenerator.Data
{
    public interface IRepository
    {
        Task CreatePurchaseDocumentAsync(Purchase purchase);
        Task CreateCustomerDocumentAsync(Customer customer);
        Task AddToCustomerRecentPurchasesAsync(Purchase purchase);
    }
}
