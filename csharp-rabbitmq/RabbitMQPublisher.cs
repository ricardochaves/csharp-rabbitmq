using System.Collections.Generic;
using RabbitMQ.Client;

namespace csharp_rabbitmq
{
    public class RabbitMQPublisher
    {
        private IConnection _conn;
        private IModel _channel;
        
        public RabbitMQPublisher()
        {
            _conn = new RabbitMqConnectionFactory().CreateConnection();
            _channel = new RabbitMqChannelFactory().CreateChannel(_conn);
        }
        
        public void publishStringMessageToQueue(string message, string queueName)
        {
            byte[] messageBodyBytes = System.Text.Encoding.UTF8.GetBytes(message);
            IBasicProperties props = _channel.CreateBasicProperties();
            props.ContentType = "text/plain";
            props.Headers = new Dictionary<string, object> {{"request-id", this.GetRequestId()}};
            _channel.BasicPublish(exchange:"", routingKey:queueName, body:messageBodyBytes);
            // _channel.WaitForConfirmsOrDie();
        }

        private string GetRequestId()
        {
            return "x";
        }
    }
}