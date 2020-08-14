using System;
using System.IO;
using System.Security.Policy;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos.Fluent;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace DataGenerator
{
    public static class Program
    {
        private static async Task Main()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
                                                    .AddJsonFile("app-settings.json", optional: false, reloadOnChange: false);

            var configuration = builder.Build();

            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection, configuration);

            var serviceProvider = serviceCollection.BuildServiceProvider();

            await StartAsync(serviceProvider);
        }

        private static async Task StartAsync(ServiceProvider serviceProvider)
        {
            throw new NotImplementedException();
        }

        public static void ConfigureServices(ServiceCollection serviceCollection, IConfiguration configuration)
        {
            serviceCollection.TryAddSingleton(new CosmosClientBuilder(configuration.GetConnectionString("CosmosDb"))
                                              .WithConnectionModeDirect()
                                              .Build()
                                              .GetDatabase("Store"));

            serviceCollection.AddTransient<PurchaseBuilder>();
            serviceCollection.AddTransient<IPurchaseFaker, PurchaseFaker>();
        }
    }
}
