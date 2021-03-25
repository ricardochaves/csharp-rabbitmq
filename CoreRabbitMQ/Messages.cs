namespace CoreRabbitMQ
{
    public class Messages
    {
        public class BasicMessage
        {
            public string queueName;
            public string exchange = "";
        }

        public class StringMessage : BasicMessage
        {
            public string message;
        }
    }
}