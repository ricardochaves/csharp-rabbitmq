using System;
using System.Text;
using CoreRabbitMQ;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Consumer1
{
    internal static class Program
    {


        static void Main()
        {


        }

        private static void ConfigureServices(IServiceCollection serviceCollection)
        {
            serviceCollection.AddLogging(configure => configure.AddConsole())
                .AddTransient<RabbitMqConsumer>();
        }
    }
}
