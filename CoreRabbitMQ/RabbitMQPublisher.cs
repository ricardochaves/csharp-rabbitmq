using System;
using System.Collections.Generic;
using System.Diagnostics;
using RabbitMQ.Client;
using Microsoft.Extensions.Logging;

namespace CoreRabbitMQ
{
    public class RabbitMQPublisher
    {
        private IConnection _conn;
        private IModel _channel;
        
        private readonly ILogger _logger;
        
        public RabbitMQPublisher(ILogger logger)
        {
            _logger = logger;
            _conn = new RabbitMqConnectionFactory().CreateConnection();
            _channel = new RabbitMqChannelFactory().CreateChannel(_conn);
        }
        
        public void PublishStringMessageToQueue(string message, string queueName)
        {
            
            _logger.LogInformation("Test do log");
            
            byte[] messageBodyBytes = System.Text.Encoding.UTF8.GetBytes(message);
            IBasicProperties props = _channel.CreateBasicProperties();
            props.ContentType = "text/plain";
            props.Headers = new Dictionary<string, object> {{"request-id", GetRequestId()}};
            
            _channel.BasicPublish(exchange:"", routingKey:queueName,basicProperties:props, body:messageBodyBytes);
            // _channel.WaitForConfirmsOrDie();
        }

        public void PublishBatchMessages(IEnumerable<Messages.StringMessage> messages)
        {
            var x = _channel.CreateBasicPublishBatch();
            foreach (var message in messages)
            {
                
                ReadOnlyMemory<byte> messageBodyBytes = System.Text.Encoding.UTF8.GetBytes(message.message);
                IBasicProperties props = _channel.CreateBasicProperties();
                props.ContentType = "text/plain";
                props.Headers = new Dictionary<string, object> {{"request-id", GetRequestId()}};

                x.Add(message.exchange, message.queueName, false, props, messageBodyBytes);

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
    }


}