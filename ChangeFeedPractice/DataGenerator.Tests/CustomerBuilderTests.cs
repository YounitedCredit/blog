using DataGenerator.DataBuilders;
using Xunit;

namespace DataGenerator.Tests
{
    public class CustomerBuilderTests
    {
        [Fact]
        public void Should_Generate_Prepared_Fake_Clients()
        {
            // Act.
            var actualCustomers = new CustomerBuilder().Build();

            // Assert.
            Assert.Equal(actualCustomers.Count, SampleData.MockCustomerIds.Count);
            Assert.Contains(actualCustomers, _ => _.Id == SampleData.MockCustomerId1);
            Assert.Contains(actualCustomers, _ => _.Id == SampleData.MockCustomerId2);
            Assert.Contains(actualCustomers, _ => _.Id == SampleData.MockCustomerId3);
        }
    }
}
