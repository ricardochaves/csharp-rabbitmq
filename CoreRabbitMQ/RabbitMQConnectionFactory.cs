using System;
using System.Threading;
using RabbitMQ.Client;

// using Microsoft.Extensions.Logging;

namespace CoreRabbitMQ
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
            return new ConnectionFactory
            {
                AutomaticRecoveryEnabled = true,
                NetworkRecoveryInterval = TimeSpan.FromSeconds(10),
                ClientProvidedName = "app: test"+Guid.NewGuid(),
                RequestedHeartbeat = TimeSpan.FromSeconds(5)
            };
        }
        
        public IConnection CreateConnection()
        {
            
            // https://www.rabbitmq.com/dotnet-api-guide.html#endpoints-list
            var endpoints = new System.Collections.Generic.List<AmqpTcpEndpoint> {
                new AmqpTcpEndpoint("localhost",5672),
                new AmqpTcpEndpoint("localhost",5673),
            };
            
            var factory = CreateConnectionFactory();
            
            factory.UserName = "guest";
            factory.Password = "guest";
            
            while (true)
            {
                try
                {
                    var conn = factory.CreateConnection(endpoints);
                    _conn = conn;
                    return conn;
                }
                catch (RabbitMQ.Client.Exceptions.BrokerUnreachableException e)
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