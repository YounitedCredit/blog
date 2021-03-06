using System.Collections.Generic;
using DataGenerator.Documents;

namespace DataGenerator.DataBuilders
{
    public class CustomerBuilder
    {
        public IReadOnlyCollection<Customer> Build()
        {
            var customers = new List<Customer>();

            customers.Add(new Customer()
            {
                Id = SampleData.MockCustomerId1,
                LastPurchases = null
            });
            customers.Add(new Customer()
            {
                Id = SampleData.MockCustomerId2,
                LastPurchases = null
            });
            customers.Add(new Customer()
            {
                Id = SampleData.MockCustomerId3,
                LastPurchases = null
            });
            return customers;
        }
    }
}
