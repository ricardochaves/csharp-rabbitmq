using System;
using System.Collections.Generic;
using System.Threading;
using RabbitMQ.Client;
// using Microsoft.Extensions.Logging;

namespace csharp_rabbitmq
{
    public class RabbitMqConnectionFactory
    {
        // private readonly ILogger _logger;
        // TODO: Inject log
        // public RabbitMqConnectionFactory(ILogger logger)
        // {
        //     _logger = logger;
        // }
        private IConnection _conn;
        
        private ConnectionFactory CreateConnectionFactory()
        {
            //https://www.rabbitmq.com/dotnet-api-guide.html#connection-recovery
            // TODO: Use a list: https://www.rabbitmq.com/dotnet-api-guide.html#endpoints-list
            return new ConnectionFactory
            {
                AutomaticRecoveryEnabled = true,
                NetworkRecoveryInterval = TimeSpan.FromSeconds(10),
                ClientProvidedName = "app: test"+Guid.NewGuid(),
                Uri = new Uri("amqp://guest:guest@localhost:5672"),
                RequestedHeartbeat = TimeSpan.FromSeconds(5)
            };
        }
        
        public IConnection CreateConnection()
        {
            var factory = this.CreateConnectionFactory();
            
            while (true)
            {
                try
                {
                    var conn = factory.CreateConnection();
                    _conn = conn;
                    return conn;
                }
                catch (RabbitMQ.Client.Exceptions.BrokerUnreachableException)
                {
                    
                    //_logger.LogWarning("BrokerUnreachableException occurred, retrying in 5 seconds");
                    Thread.Sleep(5000);
                }
            }
        }

        public void CloseAllConnections()
        {
            _conn.Close();
            _conn.Dispose();
        }
    }
}