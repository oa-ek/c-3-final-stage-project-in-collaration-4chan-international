namespace YourDarkSoulsAssistant.CatalogService.DTOs.GameItems;

public class Weapon
{
    public int Id { get; init; }
    
    public required string Name { get; init; }
    
    public required string Description { get; init; }
    
    public required string IconPath { get; init; }
    
    public required string GameCode { get; init; }
}