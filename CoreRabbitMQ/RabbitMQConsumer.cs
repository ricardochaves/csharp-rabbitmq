using System;
using System.Threading.Tasks;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace CoreRabbitMQ
{
    public class RabbitMqConsumer: IDisposable
    {
        private static RabbitMqConnectionFactory _connectionFactory = new RabbitMqConnectionFactory();
        private static RabbitMqChannelFactory _channelFactory = new RabbitMqChannelFactory();
        public void Start(string queueName, Action<Payload> callback)
        {
            //Disposing channel and connection objects is not enough, they must be explicitly closed with the API methods from the example above.
            var conn = _connectionFactory.CreateConnection();
            var channel = _channelFactory.CreateChannel(conn);
            
            // TODO: Whats the qos default? If it is 0 we need to change
            //channel.BasicQos(prefetchCount:(ushort)5);
            
            var consumer = new EventingBasicConsumer(channel);
            consumer.Received +=  (ch, ea) =>
            {
                var body = ea.Body.ToArray();
                // // copy or deserialise the payload
                // // and process the message
                // // ...
                // channel.BasicAck(ea.DeliveryTag, false);
                
                var payload = new Payload(body, channel, ea);
                callback(payload);
                
            };
            
            // this consumer tag identifies the subscription
            // when it has to be cancelled
            var consumerTag = channel.BasicConsume(queueName, false, consumer);
            
        }
        
        public void Dispose()
        {
            _connectionFactory.CloseAllConnections();
            _channelFactory.CloseAllChannels();
        }
    }
}