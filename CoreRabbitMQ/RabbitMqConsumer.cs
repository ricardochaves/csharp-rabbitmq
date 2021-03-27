using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace CoreRabbitMQ
{
    public class RabbitMqConsumer: IDisposable
    {
        private readonly ILogger<RabbitMqConsumer> _logger;
        private readonly RabbitMqConnectionFactory _connectionFactory;
        private readonly RabbitMqChannelFactory _channelFactory;
        private string? _consumerTag;
        public RabbitMqConsumer(ILogger<RabbitMqConsumer> logger, IConfiguration configuration)
        {
            _logger = logger;
            _connectionFactory = new RabbitMqConnectionFactory(_logger, configuration);
            _channelFactory = new  RabbitMqChannelFactory(logger);
        }

        public void Start(string queueName, Action<Payload> callback)
        {

            var conn = _connectionFactory.CreateConnection();
            var channel = _channelFactory.CreateChannel(conn);

            // TODO: Whats the qos default? If it is 0 we need to change
            //channel.BasicQos(prefetchCount:(ushort)5);

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received +=  (ch, ea) =>
            {

                var consumerNumber = (((EventingBasicConsumer) ch)!).Model.ChannelNumber;
                _logger.LogDebug("EventingBasicConsumer consumer number {consumerNumber}",consumerNumber);

                var body = ea.Body.ToArray();

                var payload = new Payload(body, channel, ea);

                callback(payload);

            };

            // this consumer tag identifies the subscription
            // when it has to be cancelled
            _consumerTag = channel.BasicConsume(queueName, false, consumer);

            _logger.LogInformation("Consumer {consumerTag} ready...", _consumerTag);
        }

        public void Dispose()
        {
            _connectionFactory.CloseAllConnections();
            _channelFactory.CloseChannel();
        }
    }
}
