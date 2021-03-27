using RabbitMQ.Client;
using Microsoft.Extensions.Logging;

namespace CoreRabbitMQ
{
    public class RabbitMqChannelFactory
    {
        private IModel? _channel;
        private readonly ILogger _logger;

        public RabbitMqChannelFactory(ILogger logger)
        {
            _logger = logger;
        }

        public IModel CreateChannel(IConnection conn)
        {
            if (_channel != null)
            {
                if (_channel.IsOpen)
                {
                    return _channel;
                }

                var channelCloseReason = _channel.CloseReason.ReplyText;

                _logger.LogWarning("The channel was closed because: {channelCloseReason}. Creating another...",
                    channelCloseReason);

            }

            _channel = conn.CreateModel();
            return _channel;
        }

        public void CloseChannel()
        {
            if (_channel == null) return;
            _channel.Close();
            _channel.Dispose();
        }
    }
}
