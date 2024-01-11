using System.Text.Json;
using AspireTemplate.Common;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace RabbitMQConsumers;

public class OrderProcessingJob : BackgroundService
{
    private readonly ILogger<OrderProcessingJob> _logger;
    private readonly IConfiguration _config;
    private readonly IServiceProvider _serviceProvider;
    private IConnection? _messageConnection;
    private IModel? _messageChannel;

    public OrderProcessingJob(ILogger<OrderProcessingJob> logger, IConfiguration config, IServiceProvider serviceProvider)
    {
        _logger = logger;
        _config = config;
        _serviceProvider = serviceProvider;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        const string configKeyName = "RabbitMQ:QueueName";
        string queueName = _config[configKeyName] ?? "orders";

        _messageConnection = _serviceProvider.GetService<IConnection>();

        _messageChannel = _messageConnection!.CreateModel();
        _messageChannel.QueueDeclare(queueName, exclusive: false);

        var consumer = new EventingBasicConsumer(_messageChannel);
        consumer.Received += ProcessMessageAsync;

        _messageChannel.BasicConsume(queue: queueName,
                                     autoAck: true,
                                     consumer: consumer);

        return Task.CompletedTask;
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        await base.StopAsync(cancellationToken);

        _messageChannel?.Dispose();
    }

    private void ProcessMessageAsync(object? sender, BasicDeliverEventArgs args)
    {
        _logger.LogInformation($"Processing Order at: {DateTime.UtcNow} with messageId: {args.BasicProperties.MessageId}");

        var message = args.Body;

        var model = JsonSerializer.Deserialize<OrderModel>(message.Span);

        _logger.LogInformation($"Message Name: {model!.Name} and Amount: {model!.Amount}");

    }
}