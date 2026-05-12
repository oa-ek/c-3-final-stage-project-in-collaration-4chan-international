using System.Text.Json;

namespace YourDarkSoulsAssistant.ArticlesService.DTOs;

public record EnrichedWikiItemDto
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    
    // Збагачені дані із зовнішніх API
    public string? Lore { get; set; }          // Від MediaWiki
    public string? Image { get; set; }         // Від Google Images
    public List<YouTubeGuideResponseDTO> Videos { get; set; } = new(); // Від YouTube
    
    // Динамічні стати з нашого jsonb (HP, Defense, Attack Power тощо)
    public JsonDocument? Stats { get; set; } 
}