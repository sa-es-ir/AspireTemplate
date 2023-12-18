var builder = DistributedApplication.CreateBuilder(args);

var apiservice = builder.AddProject<Projects.AspireTemplate_ApiService>("apiservice");

var messaging = builder.AddRabbitMQContainer("messaging", password: "saeed");

builder.AddProject<Projects.AspireTemplate_Web>("webfrontend")
    .WithReference(apiservice);

builder.AddProject<Projects.AspireTemplate_RabbitMQ>("consumers")
    .WithReference(messaging);

builder.Build().Run();
