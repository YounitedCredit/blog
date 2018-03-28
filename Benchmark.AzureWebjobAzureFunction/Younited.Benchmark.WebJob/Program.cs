using System;
using System.Configuration;
using System.Diagnostics;
using System.Globalization;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.ServiceBus;
using Microsoft.Extensions.Logging;
using Microsoft.ServiceBus.Messaging;

namespace Younited.Benchmark.WebJob
{
    public static class Program
    {
        public static void Main()
        {
            try
            {
                var config = new JobHostConfiguration();

                if (config.IsDevelopment)
                {
                    config.UseDevelopmentSettings();
                }

                string instrumentationKey = ConfigurationManager.AppSettings["InstrumentationKey"];
                if (!string.IsNullOrEmpty(instrumentationKey))
                {
                    config.LoggerFactory = new LoggerFactory().AddApplicationInsights(instrumentationKey, null);
                }

                config.Tracing.ConsoleLevel = TraceLevel.Warning;

                var serviceBusConfig = new ServiceBusConfiguration();

                string maxConcurrentCallsSetting = ConfigurationManager.AppSettings["MaxConcurrentCalls"];
                if (int.TryParse(maxConcurrentCallsSetting, NumberStyles.Integer, CultureInfo.InvariantCulture, out int maxConcurrentCalls))
                {
                    serviceBusConfig.MessageOptions = new OnMessageOptions
                    {
                        MaxConcurrentCalls = maxConcurrentCalls
                    };
                }

                config.UseServiceBus(serviceBusConfig);

                var host = new JobHost(config);
                host.RunAndBlock();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}
