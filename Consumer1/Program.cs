using System;
using System.ComponentModel.Design;
using System.Text;
using CoreRabbitMQ;

namespace Consumer1
{
    class Program
    {
        
        private static void PrintMessage(Payload payload)
        {
            var bytesAsString = Encoding.UTF8.GetString(payload.GetBody());
            
            Console.Write(bytesAsString);
            payload.Ack();
        }
        
        static void Main(string[] args)
        {
            
            var queueName = "Test"; 
                 
            using var consumer = new RabbitMqConsumer();
            consumer.Start(queueName, PrintMessage);
     
            
            Console.WriteLine("Start consumer...");
            Console.ReadLine();
        }
    }
}