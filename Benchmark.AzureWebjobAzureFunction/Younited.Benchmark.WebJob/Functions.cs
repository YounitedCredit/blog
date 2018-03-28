using Microsoft.Azure.WebJobs;
using Microsoft.ServiceBus.Messaging;

namespace Younited.Benchmark.WebJob
{
    public static class Functions
    {
        public static void ProcessQueueMessageFromAzureWebjob([ServiceBusTrigger("%ServiceBus_QueueName%")] BrokeredMessage brokeredMessage)
        {
        }
    }
}
