using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;

namespace YourDarkSoulsAssistant.ArticlesService.Models;

public class ArticleBlock
{
    [Key]
    public Guid Id { get; set; }
    
    public Guid EntryId { get; set; }
    [ForeignKey(nameof(EntryId))]
    public Article Article { get; set; } = null!;

    public int OrdinalNumber { get; set; }
    
    public int TypeId { get; set; }
    [ForeignKey(nameof(TypeId))]
    public PageBlockType BlockType { get; set; } = null!;
    
    [Column(TypeName = "jsonb")]
    public JsonDocument ContentData { get; set; } = null!;
}