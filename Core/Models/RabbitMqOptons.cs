namespace Core.Models
{
    public class RabbitMqOptons
    {
        public string Host { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
        public string QueueName { get; set; }
        public string ExchangeName { get; set; }
        public string RoutingKey { get; set; }
    }
}
