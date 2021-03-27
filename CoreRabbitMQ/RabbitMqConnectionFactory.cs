using System;
using System.Linq;
using System.Threading;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;

using Microsoft.Extensions.Logging;

namespace CoreRabbitMQ
{
    internal class RabbitMqConnectionFactory
    {
        private readonly ILogger _logger;
        private IConnection? _conn;
        private readonly CoreRabbitMqConfiguration _configuration;

        public RabbitMqConnectionFactory(ILogger logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration.GetSection("CoreRabbitMq").Get<CoreRabbitMqConfiguration>();
            if (_configuration == null)
                throw new Exception("The configuration is wrong. Please check the values");
        }

        private ConnectionFactory CreateConnectionFactory()
        {
            //https://www.rabbitmq.com/dotnet-api-guide.html#connection-recovery
            return new ConnectionFactory
            {
                AutomaticRecoveryEnabled = true,
                NetworkRecoveryInterval = TimeSpan.FromSeconds(_configuration.NetworkRecoveryIntervalSeconds),
                ClientProvidedName = _configuration.ClientProvidedNamePrefix+Guid.NewGuid(),
                RequestedHeartbeat = TimeSpan.FromSeconds(_configuration.RequestedHeartbeatSeconds)
            };
        }

        public IConnection CreateConnection()
        {

            // https://www.rabbitmq.com/dotnet-api-guide.html#endpoints-list
            var endpoints = _configuration.Hosts.Select(host => new AmqpTcpEndpoint(host.Host, host.Port)).ToList();

            var factory = CreateConnectionFactory();

            factory.UserName = _configuration.UserName;
            factory.Password = _configuration.Password;

            while (true)
            {
                try
                {
                    _conn = factory.CreateConnection(endpoints);
                    return _conn;
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
