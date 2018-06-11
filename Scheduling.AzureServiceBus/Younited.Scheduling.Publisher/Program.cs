using System;
using System.Text;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Configuration;

namespace Younited.Scheduling.Publisher
{
    /// <summary>
    /// The purpose of this program is to send scheduled messages to an Azure service bus queue as configured in file 'appsettings.json'.
    /// </summary>
    public static class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                var configuration = new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json")
                    .Build();

                var serviceBusConfiguration = configuration.GetSection("ServiceBus");
                var connectionString = serviceBusConfiguration["ConnectionString"];
                var queueName = serviceBusConfiguration["QueueName"];

                var queueClient = new QueueClient(connectionString, queueName);

                int messageNumber = 1;

                while (true)
                {
                    Console.WriteLine("Press return to schedule a message to become active in 1 minute.");
                    Console.ReadLine();

                    var body = Encoding.UTF8.GetBytes($"Message #{messageNumber}");
                    var message = new Message(body);
                    var scheduleEnqueueTimeUtc = DateTimeOffset.Now.AddMinutes(1);

                    queueClient.ScheduleMessageAsync(message, scheduleEnqueueTimeUtc);

                    messageNumber++;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Environment.Exit(-1);
            }
        }
    }
}
