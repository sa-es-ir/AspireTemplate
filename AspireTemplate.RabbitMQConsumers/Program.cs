
using AspireTemplate.RabbitMQConsumers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateApplicationBuilder(args);

builder.AddServiceDefaults();

builder.AddRabbitMQ("messaging");

builder.Services.AddHostedService<OrderProcessingJob>();

var host = builder.Build();
host.Run();