using Core.Models;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Consumer_ConsoleApp
{
    internal class Subscriber
    {
        private readonly RabbitMqOptions _rabbitMqOptons;
        private readonly IDatabase _redisDb;

        IConnection _connection;
        IModel _channel;

        public Subscriber(IOptions<RabbitMqOptions> options, IConnectionMultiplexer redis)
        {
            _rabbitMqOptons = options.Value;
            _redisDb = redis.GetDatabase();
        }

        public void Subscribe()
        {
            var connectionFactory = new ConnectionFactory()
            {
                HostName = _rabbitMqOptons.Host,
                VirtualHost = _rabbitMqOptons.User,
                UserName = _rabbitMqOptons.User,
                Password = _rabbitMqOptons.Password
            };

            _connection = connectionFactory.CreateConnection();
            Console.WriteLine("Connection Oluşturuldu");

            _channel = _connection.CreateModel();
            _channel.QueueDeclare(_rabbitMqOptons.QueueName, true, false, false);
            _channel.QueueBind(_rabbitMqOptons.QueueName, _rabbitMqOptons.ExchangeName, _rabbitMqOptons.RoutingKey);

            var consumer = new EventingBasicConsumer(_channel);

            consumer.Received += MessageReceived;

            _channel.BasicConsume(_rabbitMqOptons.QueueName, true, consumer);
            Console.WriteLine("Mesaj Bekleniyor");
        }

        void MessageReceived(object? sender, BasicDeliverEventArgs ea)
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            Console.WriteLine($"Kuyruktan bir mesaj alındı. Mesaj : {message}");

            var result = _redisDb.ListLeftPush("message", message);
            Console.WriteLine($"Mesaj Redis'e kayıt edildi. Mesaj sayısı: {result}");
        }
    }
}
