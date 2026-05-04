using System.Text.Json.Serialization;

namespace YourDarkSoulsAssistant.GameItemsCatalogService.DTOs;

public class WikiPage
{
    [JsonPropertyName("pageid")]
    public int PageId { get; set; }

    [JsonPropertyName("title")]
    public string Title { get; set; }

    // Список ревізій (версій сторінки)
    [JsonPropertyName("revisions")]
    public List<WikiRevision> Revisions { get; set; }
}
