using System.ComponentModel.DataAnnotations;

namespace YourDarkSoulsAssistant.ArticlesService.DTOs;

public class CreateIndexedArticleDTO
{
    [Required]
    public string Title { get; set; } = string.Empty;
    public string Subtitle { get; set; } = string.Empty;
    [Required]
    public string Category { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    
    public string RawStatsJson { get; set; } = "{}"; 
}