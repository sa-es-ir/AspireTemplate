using AspireTemplate.Common;
using RabbitMQ.Client;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// Add service defaults & Aspire components.
builder.AddServiceDefaults();

builder.AddRabbitMQ("messaging");


// Add services to the container.
builder.Services.AddProblemDetails();


var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseExceptionHandler();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();

    return forecast;
});


app.MapGet("/send-message", (IConnection messageConnection, IConfiguration configuration) =>
{
    const string configKeyName = "RabbitMQ:QueueName";
    string? queueName = configuration[configKeyName];

    using var channel = messageConnection.CreateModel();
    channel.QueueDeclare(queueName, exclusive: false);
    channel.BasicPublish(
        exchange: "",
        routingKey: queueName,
        basicProperties: null,
        body: JsonSerializer.SerializeToUtf8Bytes(new OrderModel
        {
            Name = $"Message from API: {Guid.NewGuid()}",
            Amount = 1000
        }));

    return Results.Ok("message sent");
});

app.MapDefaultEndpoints();

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}

