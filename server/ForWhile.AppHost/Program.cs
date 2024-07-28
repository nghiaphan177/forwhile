var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.ForWhile>("forwhile");

builder.Build().Run();
