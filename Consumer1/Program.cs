using System;
using System.Text;
using CoreRabbitMQ;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Consumer1
{
    internal static class Program
    {
        private static void PrintMessage(Payload payload)
        {
            var bytesAsString = Encoding.UTF8.GetString(payload.Body);

            Console.Write(bytesAsString);
            payload.Ack();
        }

        static void Main()
        {

            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);

            var serviceProvider = serviceCollection.BuildServiceProvider();
            var consumer = serviceProvider.GetService<RabbitMqConsumer>();

            var queueName = "Queue Test";

            consumer.Start(queueName, PrintMessage);

            Console.WriteLine("The consumer is ready...");
            Console.ReadLine();
        }

        private static void ConfigureServices(IServiceCollection serviceCollection)
        {
            serviceCollection.AddLogging(configure => configure.AddConsole())
                .AddTransient<RabbitMqConsumer>();
        }
    }
}
