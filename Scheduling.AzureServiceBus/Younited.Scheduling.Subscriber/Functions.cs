using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.ServiceBus.Messaging;

namespace Younited.Scheduling.Subscriber
{
    public static class Functions
    {
        [FunctionName("ConsumeMessage")]
        public static async Task ConsumeMessage([ServiceBusTrigger("%QueueName%")]BrokeredMessage brokeredMessage, TraceWriter traceWriter)
        {
            string content;
            var body = brokeredMessage.GetBody<Stream>();
            using (var streamReader = new StreamReader(body, Encoding.UTF8))
            {
                content = await streamReader.ReadToEndAsync().ConfigureAwait(false);
            }

            traceWriter.Info($"Received message: '{content}'.");
        }
    }
}