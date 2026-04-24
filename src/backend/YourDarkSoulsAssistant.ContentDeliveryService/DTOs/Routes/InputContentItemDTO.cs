namespace YourDarkSoulsAssistant.ContentDeliveryService.DTOs.Routes;

public record InputContentItemDTO
{
    public required string Name { get; set; }
    
    public required string Route { get; set; }
}
