
namespace CoreRabbitMQ
{
    public class RabbitMqMessage
    {
        public string QueueName { get; }
        private readonly string? _exchangeName;


        public object Message { get; }

        public string ExchangeName => _exchangeName ?? "";

        public RabbitMqMessage(object message, string queueName, string? exchangeName)
        {
            QueueName = queueName;
            _exchangeName = exchangeName;
            Message = message;
        }
        public RabbitMqMessage(object message, string queueName)
        {
            QueueName = queueName;
            Message = message;
        }
    }
}
