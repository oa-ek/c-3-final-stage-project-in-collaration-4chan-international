var builder = DistributedApplication.CreateBuilder(args);

var gatewaySecret = builder.AddParameter("GatewaySecret");

var userService = builder.AddProject<Projects.YourDarkSoulsAssistant_UserService>("userservice")
    .WithEnvironment("GatewaySecret", gatewaySecret);

var contentDeliveryService = builder.AddProject<Projects.YourDarkSoulsAssistant_ContentDeliveryService>("contentdeliveryservice")
    .WithEnvironment("GatewaySecret", gatewaySecret);

var gateway = builder.AddProject<Projects.YourDarkSoulsAssistant_ApiGateway>("gateway")
    .WithReference(contentDeliveryService)
    .WithReference(userService)
    .WithEnvironment("GatewaySecret", gatewaySecret);

builder.Build().Run();
