﻿using PlatformService.Dtos;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace PlatformService.AsyncDataServices
{
    public class MessageBusClient : IMessageBusClient
    {
        private IConfiguration _configuration;
        private IConnection _connection;
        private IModel _channel;

        public MessageBusClient(IConfiguration configuration)
        {
            _configuration = configuration;
            var factory = new ConnectionFactory()
            {
                HostName = _configuration["RabbitMQHost"],
                Port = int.Parse(_configuration["RabbitMQPort"])
            };
            try
            {
                _connection = factory.CreateConnection();
                _channel = _connection.CreateModel();
                _channel.ExchangeDeclare(exchange: "trigger", type: ExchangeType.Fanout);
                _connection.ConnectionShutdown += RabbitMQ_ConnectionShutdown;
                Console.WriteLine("--> connected to MessageBus");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"--> Could not connect to the message Bus: {ex.Message}");
            }
        }

        private void RabbitMQ_ConnectionShutdown(object? sender, ShutdownEventArgs e)
        {
            Console.WriteLine("--> RabbitMQ shutdown");
        }

        public void PublishNewPlatform(PlatformPublish platformPublishDto)
        {
           var message = JsonSerializer.Serialize(platformPublishDto);
            if (_connection.IsOpen)
            {
                Console.WriteLine("---> RabbitMQ connection Open sending message");
                SendMessage(message);
            }
            else
            {
                Console.WriteLine("---> RabbitMQ connection Colsed sending message");
            }
        }

        private void SendMessage(string message)
        {
            var body = Encoding.UTF8.GetBytes(message);
            _channel.BasicPublish(exchange: "trigger", routingKey: "", body: body);
            Console.WriteLine($"---> We have sent {message}");

        }

        public void Dispose() {
            Console.WriteLine("--> MessageBus disposed");
                if (_channel.IsOpen) {
                _channel.Close();
                _connection.Close();
            }
        }

    }
}
