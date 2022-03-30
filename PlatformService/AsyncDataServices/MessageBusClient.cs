using PlatformService.Dtos;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace PlatformService.AsyncDataServices
{
    public class MessageBusClient : IMessageBusClient
    {
        private readonly IConfiguration _configuration;
        private readonly IConnection _connection;
        private readonly IModel _channel;

        public MessageBusClient(IConfiguration configuration)
        {
            _configuration = configuration;
            var factory = new ConnectionFactory()
            {
                HostName = _configuration["RabbitMQ:Host"],
                Port = int.Parse(_configuration["RabbitMQ:Port"]),
            };
            try
            {
                _connection = factory.CreateConnection();
                _channel = _connection.CreateModel();
                _channel.ExchangeDeclare(exchange: _configuration["RabbitMQ:Exchange"], type: ExchangeType.Fanout);
                _connection.ConnectionShutdown += RabbitMQ_ConnectionShutdown;
                Console.WriteLine("--> RabbitMQ connection established");
            }
            catch (Exception ex)
            {
                Console.WriteLine("--> Could not connect to Message Bus: " + ex.Message);
            }
        }

        private void RabbitMQ_ConnectionShutdown(object sender, ShutdownEventArgs e)
        {
            Console.WriteLine("--> RabbitMQ Connection Shutdown");
        }

        public void PublishNewPlatform(PlatformPublishDto platformPublishDto)
        {
            var message = JsonSerializer.Serialize(platformPublishDto);
            if (_connection.IsOpen)
            {
                Console.WriteLine("--> RabbitMQ Connection Open, sending message");
                SendMessage(message);
            }
            else
            {
                Console.WriteLine("--> RabbitMQ connection is not open");
            }
        }

        public void SendMessage(string message)
        {
            var body = Encoding.UTF8.GetBytes(message);
            _channel.BasicPublish(exchange: _configuration["RabbitMQ:Exchange"],
                                 routingKey: "",
                                 basicProperties: null,
                                 body: body);
            Console.WriteLine($"--> We have sent {message}");
        }

        public void Dispose()
        {
            Console.WriteLine("--> Message Bus Client Disposed");
            if (_connection != null)
            {
                _channel.Dispose();
                _connection.Dispose();
            }
        }
    }
}
