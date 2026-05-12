using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;

namespace YourDarkSoulsAssistant.ArticlesService.Models;

public class Article
{
    [Key]
    public Guid Id { get; set; }
    
    [Required, MaxLength(255)]
    public string Title { get; set; } = string.Empty;
    
    [MaxLength(255)]
    public string Subtitle { get; set; } = string.Empty;
    
    public string Description { get; set; } = string.Empty;
    
    public float Rating { get; set; }
    
    public Guid CreatorId { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    
    public bool IsChecked { get; set; }
    
    public int TypeId { get; set; }
    [ForeignKey(nameof(TypeId))]
    public ArticleType Type { get; set; } = null!;
    
    public ICollection<ArticleBlock> Blocks { get; set; } = new List<ArticleBlock>();
}
