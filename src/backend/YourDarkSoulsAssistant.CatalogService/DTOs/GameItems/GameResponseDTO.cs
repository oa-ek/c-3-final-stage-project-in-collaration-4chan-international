namespace YourDarkSoulsAssistant.CatalogService.DTOs.GameItems;

public record GameResponseDTO
{
    public int Id { get; init; }
    
    public required string Name { get; init; }
    
    public required string Code { get; init; }
    
    public required string IconPath { get; init; }
}
