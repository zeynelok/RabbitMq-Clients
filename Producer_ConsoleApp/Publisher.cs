using Core.Models;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using System.Text;

namespace Producer_ConsoleApp
{
    public class Publisher
    {
        private readonly RabbitMqOptions _rabbitOptons;
        IConnection _connection;
        IModel _channel;
        public Publisher(IOptions<RabbitMqOptions> options)
        {
            _rabbitOptons = options.Value;
        }
        public void PublishToRabbit()
        {
            try
            {
                var connectionFactory = new ConnectionFactory()
                {
                    HostName = _rabbitOptons.Host,
                    VirtualHost = _rabbitOptons.User,
                    UserName = _rabbitOptons.User,
                    Password = _rabbitOptons.Password
                };

                _connection = connectionFactory.CreateConnection();
                _channel = _connection.CreateModel();
                _channel.QueueDeclare(_rabbitOptons.QueueName, true, false, false);
                _channel.QueueBind(_rabbitOptons.QueueName, _rabbitOptons.ExchangeName, _rabbitOptons.RoutingKey);
                Publish();

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Hata Oluştu. Mesaj : {ex.Message}");

            }

        }

        void Publish()
        {
            var count = 0;
            while (true)
            {
                Thread.Sleep(TimeSpan.FromSeconds(5));

                var message = $"count: {count} - date: {DateTime.Now}";
                var body = Encoding.UTF8.GetBytes(message);
                _channel.BasicPublish(_rabbitOptons.ExchangeName, _rabbitOptons.RoutingKey, null, body);
                Console.WriteLine($"Mesaj Rabbit'e gönderildi. Mesaj : {message}");
                count++;
            }
        }
    }
}
