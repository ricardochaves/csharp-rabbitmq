using RabbitMQ.Client;

namespace CoreRabbitMQ
{
    public class RabbitMqChannelFactory
    {
        private IModel _channel;
        
        public IModel CreateChannel(IConnection conn)
        {
            _channel = conn.CreateModel();
            return _channel;
        }

        public void CloseAllChannels()
        {
            _channel.Close();
            _channel.Dispose();
        }
    }
}