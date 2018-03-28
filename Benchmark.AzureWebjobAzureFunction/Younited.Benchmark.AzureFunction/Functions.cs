using Microsoft.Azure.WebJobs;
using Microsoft.ServiceBus.Messaging;

namespace Younited.Benchmark.AzureFunction
{
    public static class Functions
    {
        [FunctionName("ProcessQueueMessageFromAzureFunction")]
        public static void Run([ServiceBusTrigger("%ServiceBus_QueueName%")]BrokeredMessage brokeredMessage)
        {
        }
    }
}