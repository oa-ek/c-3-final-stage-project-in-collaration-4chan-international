namespace YourDarkSoulsAssistant.ContentDeliveryService.DTOs.Routes;

public record ContentItemDTO
{
    public required Guid Id { get; init; }
    
    public required string Name { get; init; }
    
    public required string PublicRoute { get; init; }
    
    public required string PrivateRoute { get; init; }
    
    public required bool IsActive { get; init; }
}
