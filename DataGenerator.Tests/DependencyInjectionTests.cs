using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace DataGenerator.Tests
{
    public class DependencyInjectionTests
    {
        private static readonly IDictionary<string, string> Configuration;

        static DependencyInjectionTests()
        {
            Configuration = new Dictionary<string, string>
                {
                    ["ConnectionStrings:CosmosDb"] = "AccountEndpoint=https://that.documents.azure.com:443/;AccountKey=YWJjZGVm;"
                };
        }

        [Theory]
        [MemberData(nameof(Types))]
        public void CheckType(ServiceProvider services, ServiceDescriptor registration)
        {
            services.GetRequiredService(registration.ServiceType);
        }

        public static  IEnumerable<object[]> Types()
        {
            var serviceCollection = new ServiceCollection();

            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(Configuration)
                .Build();

            Program.ConfigureServices(serviceCollection, configuration);

            var serviceProvider = serviceCollection.BuildServiceProvider();

            foreach (var registration in serviceCollection)
            {
                if (registration.ServiceType.Namespace.StartsWith("DataGenerator"))
                {
                    yield return new object[] { serviceProvider, registration };
                }
            }
        }
    }
}
