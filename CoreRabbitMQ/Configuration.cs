// ReSharper disable All
namespace CoreRabbitMQ
{
    internal class Hosts
    {
        public string Host{ get; set; } = null!;
        public int Port{ get; set; }
    }

    internal class CoreRabbitMqConfiguration
    {
        public double NetworkRecoveryIntervalSeconds { get; set; }
        public string ClientProvidedNamePrefix { get; set; } = null!;
        public double RequestedHeartbeatSeconds { get; set; }
        public Hosts[] Hosts { get; set; } = null!;
        public string UserName { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}
