using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DataGenerator.Data;
using DataGenerator.DataBuilders;
using Microsoft.Azure.Cosmos.Fluent;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace DataGenerator
{
    public static class Program
    {
        private const int ExistCode_ErrorBadArguments = 0xA0;
        private const int ExitCode_Success = 0;

        /// <summary>
        /// Generate test data to play with Cosmos Db Change Feed
        /// </summary>
        /// <param name="command">send an 'init', 'feed', or 'reset' command</param>
        /// <returns></returns>
        private static async Task<int> Main(string command)
        {
            if (string.IsNullOrEmpty(command) || !new []{ "init", "feed", "reset"}.Contains(command))
            {
                Console.ForegroundColor = ConsoleColor.Red;

                Console.WriteLine("The --command value is missing. Please retry with the right argument.");
                Console.WriteLine("Expecting \"init\", \"feed\" or \"reset\" ");

                Console.ResetColor();
                return ExistCode_ErrorBadArguments;
            }

            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
                                                    .AddJsonFile("app-settings.json", optional: false, reloadOnChange: false);

            var configuration = builder.Build();


            if (command == "reset")
            {
                await ResetAsync(configuration);
                return ExitCode_Success;
            }

            var serviceCollection = new ServiceCollection();
            await ConfigureServices(serviceCollection, configuration);

            var serviceProvider = serviceCollection.BuildServiceProvider();

            if (command == "init")
            {
                await InitAsync(serviceProvider);
            }
            if(command == "feed")
            {
                await FeedAsync(serviceProvider);
            }
            return ExitCode_Success;
        }

        private static async Task FeedAsync(ServiceProvider serviceProvider)
        {
            var purchases = serviceProvider.GetService<PurchaseBuilder>().Build(10);

            var repository = serviceProvider.GetService<IRepository>();

            foreach (var purchase in purchases)
            {
                await repository.CreatePurchaseDocumentAsync(purchase);
            }

            Console.WriteLine("Done feeding Purchases to Store Database");
        }

        private static async Task InitAsync(ServiceProvider serviceProvider)
        {
            var customers = serviceProvider.GetService<CustomerBuilder>().Build();
            var purchases = serviceProvider.GetService<PurchaseBuilder>().Build(10);

            var repository = serviceProvider.GetService<IRepository>();

            foreach (var customer in customers)
            {
                await repository.CreateCustomerDocumentAsync(customer);
            }

            foreach (var purchase in purchases)
            {
                await repository.CreatePurchaseDocumentAsync(purchase);
            }

            Console.WriteLine("Done Initializing Store Database");
        }

        private static async Task ResetAsync(IConfiguration configuration)
        {
            var client = new CosmosClientBuilder(configuration.GetConnectionString("CosmosDb"))
                         .WithConnectionModeDirect()
                         .Build();

            await client.GetDatabase(Repository.DatabaseName).DeleteAsync();

            await client.CreateDatabaseIfNotExistsAsync(Repository.DatabaseName, 400);
            await client.GetDatabase(Repository.DatabaseName).CreateContainerIfNotExistsAsync(Repository.CustomersContainerName, "/id");
            await client.GetDatabase(Repository.DatabaseName).CreateContainerIfNotExistsAsync(Repository.PurchasesContainerName, "/FictionalArticleNumber");

            Console.WriteLine("Done Resetting Store Database");
        }

        private static async Task ConfigureServices(ServiceCollection serviceCollection, IConfiguration configuration)
        {
            var client = new CosmosClientBuilder(configuration.GetConnectionString("CosmosDBConnection"))
                       .WithConnectionModeDirect()
                       .Build();
            await client.CreateDatabaseIfNotExistsAsync(Repository.DatabaseName, 400);
            await client.GetDatabase(Repository.DatabaseName).CreateContainerIfNotExistsAsync(Repository.CustomersContainerName, "/id");
            await client.GetDatabase(Repository.DatabaseName).CreateContainerIfNotExistsAsync(Repository.PurchasesContainerName, "/FictionalArticleNumber");

            serviceCollection.TryAddSingleton(client
                                              .GetDatabase(Repository.DatabaseName));

            serviceCollection.AddTransient<IRepository, Repository>();
            serviceCollection.AddTransient<PurchaseBuilder>();
            serviceCollection.AddTransient<CustomerBuilder>();
            serviceCollection.AddTransient<IFaker, Faker>();
        }
    }
}
