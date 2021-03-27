using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace CoreRabbitMQ
{
    public class Payload
    {
        public byte[] Body { get; }
        private readonly IModel _channel;
        private readonly BasicDeliverEventArgs _ea;
        public Payload(byte[] body, IModel channel, BasicDeliverEventArgs ea)
        {
            Body = body;
            _channel = channel;
            _ea =ea;
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
