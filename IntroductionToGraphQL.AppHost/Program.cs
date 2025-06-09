var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.IntroductionToGraphQL>("introductiontographql");

builder.Build().Run();
