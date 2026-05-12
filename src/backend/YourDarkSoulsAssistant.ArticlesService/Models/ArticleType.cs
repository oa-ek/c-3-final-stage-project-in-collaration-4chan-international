using System.ComponentModel.DataAnnotations;

namespace YourDarkSoulsAssistant.ArticlesService.Models;

public class ArticleType
{
    [Key]
    public int Id { get; set; }
    
    [Required, MaxLength(100)]
    public string Name { get; set; } = string.Empty;
    
    [MaxLength(500)]
    public string Description { get; set; } = string.Empty;
    
    public ICollection<Article> Articles { get; set; } = new List<Article>();
}
