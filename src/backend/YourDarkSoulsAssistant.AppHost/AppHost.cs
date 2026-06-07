var builder = DistributedApplication.CreateBuilder(args);

var gatewaySecret = builder.AddParameter("GatewaySecret");

var redis = builder.AddRedis("redis");

var userService = builder
    .AddProject<Projects.YourDarkSoulsAssistant_UsersService>("UsersService")
    .WithReference(redis)
    .WithEnvironment("GatewaySecret", gatewaySecret)
    .WithHttpHealthCheck("/health");

var contentDeliveryService = builder
    .AddProject<Projects.YourDarkSoulsAssistant_ContentDeliveryService>("ContentDeliveryService")
    .WithEnvironment("GatewaySecret", gatewaySecret)
    .WithHttpHealthCheck("/health");

var articlesService = builder
    .AddProject<Projects.YourDarkSoulsAssistant_ArticlesService>("ArticlesService")
    .WithReference(contentDeliveryService)
    .WithReference(redis)
    .WithEnvironment("GatewaySecret", gatewaySecret)
    .WithHttpHealthCheck("/health");

var catalogService = builder
    .AddProject<Projects.YourDarkSoulsAssistant_CatalogService>("CatalogService")
    .WithReference(contentDeliveryService)
    .WithEnvironment("GatewaySecret", gatewaySecret)
    .WithHttpHealthCheck("/health");

var gateway = builder
    .AddProject<Projects.YourDarkSoulsAssistant_ApiGateway>("Gateway")
    .WithReference(contentDeliveryService)
    .WithReference(catalogService)
    .WithReference(articlesService)
    .WithReference(userService)
    .WithEnvironment("GatewaySecret", gatewaySecret)
    .WithHttpHealthCheck("/health");

builder.Build().Run();
