using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace csharp_rabbitmq
{
    public class Payload
    {
        private readonly byte[] _body;
        private readonly IModel _channel;
        private readonly BasicDeliverEventArgs _ea;
        public Payload(byte[] body, IModel channel, BasicDeliverEventArgs ea)
        {
            _body = body;
            _channel = channel;
            _ea =ea;
        }
        public byte[] GetBody()
        {
            return _body;
        }

        public void Ack()
        {
            _channel.BasicAck(_ea.DeliveryTag, false);
        }

        public void NackAndReQueue()
        {
            _channel.BasicNack(_ea.DeliveryTag,false,true);
        }
        
        public void NackAndDiscard()
        {
            _channel.BasicNack(_ea.DeliveryTag,false,false);
        }
    }
}