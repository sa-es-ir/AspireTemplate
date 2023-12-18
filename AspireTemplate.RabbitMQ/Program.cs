
using AspireTemplate.RabbitMQ;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateApplicationBuilder(args);

builder.AddServiceDefaults();

builder.AddRabbitMQ("messaging");
builder.Services.AddHostedService<OrderProcessingWorker>();

var host = builder.Build();
host.Run();