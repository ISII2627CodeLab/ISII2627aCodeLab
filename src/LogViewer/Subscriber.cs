//crear la conexion tcp con RabbitMQ usando ConnectionFactory
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
namespace LogViewer;
public class Subscriber
{
        private readonly IConfiguration _configuration;
        private readonly string _exchangeName;
        private IConnection? _connection;
        private IChannel? _channel;

    public Subscriber(IConfiguration configuration)
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
    

        await _channel.ExchangeDeclareAsync(_exchangeName, ExchangeType.Fanout, durable: true, autoDelete: false);

        var tempQueue = await _channel.QueueDeclareAsync(queue: "", durable:false, exclusive: true, autoDelete: true);
        var queueName = tempQueue.QueueName; 

        await _channel.QueueBindAsync(queue: queueName, exchange: _exchangeName, routingKey: string.Empty);

        var consumer = new AsyncEventingBasicConsumer(_channel);
        consumer.ReceivedAsync += (model, ea) =>
        {
            var json = Encoding.UTF8.GetString(ea.Body.ToArray());
            var log = JsonSerializer.Deserialize<Log>(json);
            if (log != null) Print(log);
            return Task.CompletedTask;
        };

        await _channel.BasicConsumeAsync(queue: queueName, autoAck: true, consumer: consumer);
    }
    
    
    private static void Print(Log log)
    {
        Console.ForegroundColor = log.LogLevel switch
        {
            "Critical" or "Error" => ConsoleColor.Red,
            "Warning" => ConsoleColor.Yellow,
            "Information" => ConsoleColor.Green,
            _ => ConsoleColor.Gray
        };
        Console.WriteLine($"[{log.Timestamp:HH:mm:ss}] [{log.LogLevel}] {log.Category}: {log.Message}");
        if (!string.IsNullOrEmpty(log.Exception))
            Console.WriteLine(log.Exception);
        Console.ResetColor();
    }
    
}