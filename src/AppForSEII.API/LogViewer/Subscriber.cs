//crear la conexion tcp con RabbitMQ usando ConnectionFactory
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;

public class Subscriber
{
        private readonly IConfiguration _configuration;
        private readonly string _exchangeName;
        private IConnection? _connection;
        private IChannel? _channel;

    public Subscriber(Iconfiguration configuration)
    {
        _configuration = configuration;
        _exchangeName = _configuration["RabbitMQ:Exchange"] ?? "logs";
    }

    public async Task StartAsync(){
        int port = int.TryParse(_configuration["RabbitMQ:Port"], out int parsedPort) ? parsedPort : 5672;
        var factory = new ConnectionFactory{
            HostName =_configuration["RabbitMQ:HostName"] ?? "localhost",
            Port = port,
            UserName = _configuration["RabbitMQ:UserName"] ?? "guest",
            Password = _configuration["RabbitMQ:Password"] ?? "guest"
        };

        _connection = await factory.CreateConnectionAsync();
        _channel = await _connection.CreateChannelAsync();
    }

    await _channel.ExchangeDeclareAsync(_exchangeName, ExchangeType.Fanout, durable: true, autodelete: false);

    var tempQueue = await _channel.QueueDeclareAsync(queue: "", durable:false, exclusive: true, autoDelete: true);
    var queueName = tempQueue.QueueName; 
}