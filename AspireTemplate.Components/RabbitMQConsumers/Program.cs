using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RabbitMQConsumers;

var builder = Host.CreateApplicationBuilder(args);

builder.AddServiceDefaults();

builder.AddRabbitMQ("messaging");

builder.Services.AddHostedService<OrderProcessingJob>();

var host = builder.Build();
host.Run();