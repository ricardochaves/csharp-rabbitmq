using System;
using System.Text;
using CoreRabbitMQ;

namespace Consumer
{
    public static class ConsumerFromQueue1
    {
        public static void PrintMessage(Payload payload)
        {
            var bytesAsString = Encoding.UTF8.GetString(payload.Body);

            Console.Write("Message from ConsumerFromQueue1: " + bytesAsString);
            payload.Ack();
        }
    }
}
