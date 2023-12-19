var builder = DistributedApplication.CreateBuilder(args);

var apiservice = builder.AddProject<Projects.AspireTemplate_ApiService>("apiservice");

builder.AddProject<Projects.AspireTemplate_Web>("webfrontend")
    .WithReference(apiservice);

builder.Build().Run();
