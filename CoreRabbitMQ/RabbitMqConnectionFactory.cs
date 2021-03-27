using System;
using System.Threading;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;

using Microsoft.Extensions.Logging;

namespace CoreRabbitMQ
{
    internal class RabbitMqConnectionFactory
    {
        private readonly ILogger _logger;
        private readonly IConfigurationRoot _configuration;

        public RabbitMqConnectionFactory(ILogger logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = (IConfigurationRoot)configuration;
        }
        private IConnection? _conn;

        private ConnectionFactory CreateConnectionFactory()
        {
            //https://www.rabbitmq.com/dotnet-api-guide.html#connection-recovery
            return new ConnectionFactory
            {
                AutomaticRecoveryEnabled = true,
                NetworkRecoveryInterval =
                    TimeSpan.FromSeconds(_configuration.GetValue<double>("CoreRabbitMq:NetworkRecoveryIntervalSeconds")),
                ClientProvidedName = _configuration.GetValue<string>("CoreRabbitMq:ClientProvidedNamePrefix")+Guid.NewGuid(),
                RequestedHeartbeat =
                    TimeSpan.FromSeconds(Convert.ToDouble(_configuration.GetValue<double>("CoreRabbitMq:RequestedHeartbeatSeconds")))
            };
        }

        public IConnection CreateConnection()
        {

            // https://www.rabbitmq.com/dotnet-api-guide.html#endpoints-list

            var endpoints = new System.Collections.Generic.List<AmqpTcpEndpoint>
            {
                new(_configuration.GetValue<string>("CoreRabbitMq:Hosts:0:Host"),
                    _configuration.GetValue<int>("CoreRabbitMq:Hosts:0:Port")),
                new(_configuration.GetValue<string>("CoreRabbitMq:Hosts:1:Host"),
                    _configuration.GetValue<int>("CoreRabbitMq:Hosts:1:Port"))
            };

            var factory = CreateConnectionFactory();

            factory.UserName = _configuration.GetValue<string>("CoreRabbitMq:UserName");
            factory.Password = _configuration.GetValue<string>("CoreRabbitMq:Password");

            while (true)
            {
                try
                {
                    var conn = factory.CreateConnection(endpoints);
                    _conn = conn;
                    return conn;
                }
                catch (RabbitMQ.Client.Exceptions.BrokerUnreachableException)
                {

                    _logger.LogWarning("BrokerUnreachableException occurred, retrying in 5 seconds");
                    Thread.Sleep(5000);
                }
            }
        }

        public void CloseAllConnections()
        {
            if (_conn == null) return;
            _conn.Close();
            _conn.Dispose();
        }
    }
}
