namespace YourDarkSoulsAssistant.ContentDeliveryService.Models;

public class ContentItem
{
    public Guid Id { get; set; }
    
    public required string Name { get; set; }
    
    public required string PublicRoute { get; set; }
    
    public required string PrivateRoute { get; set; }
    
    public bool IsActive { get; set; } = true;
}
