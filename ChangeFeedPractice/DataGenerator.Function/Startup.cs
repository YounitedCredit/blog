using DataGenerator.Data;
using DataGenerator.Function;
using Microsoft.Azure.Cosmos.Fluent;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Azure.WebJobs.Host.Bindings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

[assembly: FunctionsStartup(typeof(Startup))]
namespace DataGenerator.Function
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            var executionContextOptions = builder.Services.BuildServiceProvider().GetService<IOptions<ExecutionContextOptions>>().Value;

            var configuration = new ConfigurationBuilder()
                                .AddEnvironmentVariables()
                                .Build();

            var client = new CosmosClientBuilder(configuration["CosmosDBConnection"])
                         .WithConnectionModeDirect()
                         .Build();

            builder.Services.TryAddSingleton(client
                .GetDatabase(Repository.DatabaseName));

            builder.Services.AddTransient<IRepository, Repository>();        }
    }
}
