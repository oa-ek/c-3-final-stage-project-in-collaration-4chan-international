using System.ComponentModel.DataAnnotations;

namespace YourDarkSoulsAssistant.ArticlesService.Models;

public class PageBlockType
{
    [Key]
    public int Id { get; set; }
    
    [Required, MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    public ICollection<ArticleBlock> ArticleBlocks { get; set; } = new List<ArticleBlock>();
}
