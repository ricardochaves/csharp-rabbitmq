#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CoreRabbitMQ;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Consumer
{
    public class Worker1 : BackgroundService
    {
        private readonly ILogger<Worker1> _logger;
        private readonly RabbitMqConsumer _consumer;
        private readonly string _queueName;
        private readonly string _methodName;

        public Worker1(ILogger<Worker1> logger, RabbitMqConsumer consumer)
        {
            _logger = logger;
            _consumer = consumer;
            _queueName = Environment.GetCommandLineArgs()[1];
            _methodName = Environment.GetCommandLineArgs()[2];
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {

            _consumer.Start(_queueName, WorkerCallBack);

            _logger.LogInformation ("The consumer is ready...");
            Console.ReadLine();

        }

        private void WorkerCallBack(Payload payload)
        {
            ExecuteCallMethod(payload);
        }

        private void ExecuteCallMethod(Payload payload)
        {

            object?[] parameters = {payload};
            Type type = typeof(Worker1);
            var methodInfo = type.GetMethod(_methodName);

            if (methodInfo == null) throw new Exception("Method name invalid");

            methodInfo.Invoke(this, parameters);

        }

        public void ExecuteQueue1(Payload payload)
        {
            ConsumerFromQueue1.PrintMessage(payload);

        }
        public void ExecuteQueue2(Payload payload)
        {
            ConsumerFromQueue2.PrintMessage(payload);

        }

    }
}
