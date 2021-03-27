using System;
using System.Text;
using System.Threading;
using CoreRabbitMQ;

namespace csharp_rabbitmq
{
    internal static class Program
    {
        private static void PrintMessage(Payload payload)
        {
            var bytesAsString = Encoding.UTF8.GetString(payload.Body);
            Console.Write(bytesAsString);
            payload.Ack();

        }
        private static void Main(string[] args)
        {
            // const string queueName = "Queue Test";
            //
            // var publisher = new RabbitMQPublisher();
            // publisher.publishStringMessageToQueue("Ricardo Test",queueName);
            //
            // Thread.Sleep(TimeSpan.FromSeconds(1));
            //
            // using var consumer = new RabbitMqConsumer();
            // consumer.Start(queueName,PrintMessage);

            Console.WriteLine(" [*] Waiting for messages.");
            Console.ReadLine();


        }
    }
}
