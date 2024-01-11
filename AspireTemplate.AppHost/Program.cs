var builder = DistributedApplication.CreateBuilder(args);

var rabbit = builder.AddRabbitMQContainer("messaging", password: "aspire");

var apiservice = builder.AddProject<Projects.AspireTemplate_ApiService>("apiservice")
    .WithReference(rabbit);


builder.AddProject<Projects.AspireTemplate_Web>("webfrontend")
    .WithReference(apiservice);

builder.AddProject<Projects.RabbitMQConsumers>("consumers")
    .WithReference(rabbit);

builder.Build().Run();
