var builder = DistributedApplication.CreateBuilder(args);

var gatewaySecret = builder.AddParameter("GatewaySecret");

var userService = builder.AddProject<Projects.YourDarkSoulsAssistant_UsersService>("UsersService")
    .WithEnvironment("GatewaySecret", gatewaySecret);

var contentDeliveryService = builder.AddProject<Projects.YourDarkSoulsAssistant_ContentDeliveryService>("ContentDeliveryService")
    .WithEnvironment("GatewaySecret", gatewaySecret);

var gameItemsCatalogService = builder.AddProject<Projects.YourDarkSoulsAssistant_GameItemsCatalogService>("GameItemsCatalogService")
    .WithEnvironment("GatewaySecret", gatewaySecret)
    .WithReference(contentDeliveryService);

var gateway = builder.AddProject<Projects.YourDarkSoulsAssistant_ApiGateway>("Gateway")
    .WithReference(contentDeliveryService)
    .WithReference(gameItemsCatalogService)
    .WithReference(userService)
    .WithEnvironment("GatewaySecret", gatewaySecret);

builder.Build().Run();
