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

var gameItemsCatalogService = builder
    .AddProject<Projects.YourDarkSoulsAssistant_GameItemsCatalogService>("GameItemsCatalogService")
    .WithReference(contentDeliveryService)
    .WithEnvironment("GatewaySecret", gatewaySecret)
    .WithHttpHealthCheck("/health");

var gateway = builder
    .AddProject<Projects.YourDarkSoulsAssistant_ApiGateway>("Gateway")
    .WithReference(contentDeliveryService)
    .WithReference(gameItemsCatalogService)
    .WithReference(userService)
    .WithEnvironment("GatewaySecret", gatewaySecret)
    .WithHttpHealthCheck("/health");

builder.Build().Run();
