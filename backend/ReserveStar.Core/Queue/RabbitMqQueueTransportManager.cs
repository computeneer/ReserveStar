using System.Text;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using ReserveStar.Helper;

namespace ReserveStar.Core.Queue;

public sealed class RabbitMqQueueTransportManager : IQueueTransportManager, IDisposable
{
   private readonly ILogger<RabbitMqQueueTransportManager> _logger;
   private readonly ConnectionFactory _connectionFactory;

   private IConnection? _connection;
   private IChannel? _channel;
   private bool _disposed;

   public RabbitMqQueueTransportManager(ILogger<RabbitMqQueueTransportManager> logger)
   {
      _logger = logger;
      _connectionFactory = new ConnectionFactory
      {
         HostName = EnvironmentVariables.RabbitMqHost,
         Port = int.Parse(EnvironmentVariables.RabbitMqPort),
         UserName = EnvironmentVariables.RabbitMqUser,
         Password = EnvironmentVariables.RabbitMqPassword,
         VirtualHost = EnvironmentVariables.RabbitMqVHost,
      };
   }

   public async Task PublishAsync(string queueName, string payload, CancellationToken cancellationToken = default)
   {
      await EnsureConnected();

      _connection ??= await _connectionFactory.CreateConnectionAsync();

      using var channel = await _connection.CreateChannelAsync();

      await channel.QueueDeclareAsync(
         queue: queueName,
         durable: true,
         exclusive: false,
         autoDelete: false,
         arguments: null);


      var body = Encoding.UTF8.GetBytes(payload);

      await channel.BasicPublishAsync(
         exchange: string.Empty,
         routingKey: queueName,
         body: body);

      _logger.LogInformation("RabbitMQ message published. Queue: {QueueName}", queueName);
   }

   private async Task EnsureConnected()
   {
      if (_connection is { IsOpen: true } && _channel is { IsOpen: true })
      {
         return;
      }
      _channel?.Dispose();
      _connection?.Dispose();

      _connection = await _connectionFactory.CreateConnectionAsync();
      _channel = await _connection.CreateChannelAsync();

   }

   public void Dispose()
   {
      if (_disposed)
      {
         return;
      }

      _channel?.Dispose();
      _connection?.Dispose();
      _disposed = true;
   }
}
