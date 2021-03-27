using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreRabbitMQ;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Consumer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseDefaultServiceProvider(options => {})
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddHostedService<Worker1>();
                    services.AddScoped<RabbitMqPublisher>();
                    services.AddScoped<RabbitMqConsumer>();
                });
    }
}
