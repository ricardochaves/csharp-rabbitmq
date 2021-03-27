using System;
using System.Collections.Generic;
using System.Diagnostics;
using RabbitMQ.Client;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace CoreRabbitMQ
{
    public class RabbitMqPublisher
    {
        private readonly IModel _channel;

        private readonly ILogger<RabbitMqPublisher> _logger;

        public RabbitMqPublisher(ILogger<RabbitMqPublisher> logger)
        {
            _logger = logger;
            IConnection conn = new RabbitMqConnectionFactory(_logger).CreateConnection();
            _channel = new RabbitMqChannelFactory(_logger).CreateChannel(conn);
        }

        public void PublishMessage(RabbitMqMessage message)
        {
            var messageBodyBytes = ConverterObjectToFinalMessageType(message);
            var props = GetMessageProperties();

            _channel.BasicPublish(exchange: message.ExchangeName, routingKey: message.QueueName, basicProperties: props,
                body: messageBodyBytes);
            // _channel.WaitForConfirmsOrDie();
        }

        public void PublishBatchMessages(IEnumerable<RabbitMqMessage> messages)
        {
            var x = _channel.CreateBasicPublishBatch();
            foreach (var message in messages)
            {
                var messageBodyBytes = ConverterObjectToFinalMessageType(message);
                var props = GetMessageProperties();

                x.Add(message.ExchangeName, message.QueueName, false, props, messageBodyBytes);

            }
            try
            {
                _channel.TxSelect();
                x.Publish();
                _channel.TxCommit();
            }
            catch (Exception)
            {
                _channel.TxRollback();
                throw;
            }
        }

        private static string GetRequestId()
        {
            return Activity.Current.Id;
        }

        private ReadOnlyMemory<byte> ConverterObjectToFinalMessageType(RabbitMqMessage message)
        {
            var json = JsonConvert.SerializeObject(message.Message);
            _logger.LogDebug("Message to be send: {json}", json);
            return System.Text.Encoding.UTF8.GetBytes(json);
        }

        private IBasicProperties GetMessageProperties()
        {
            IBasicProperties props = _channel.CreateBasicProperties();
            props.ContentType = "application/json";
            props.Headers = new Dictionary<string, object> {{"request-id", GetRequestId()}};
            return props;
        }
    }
}
