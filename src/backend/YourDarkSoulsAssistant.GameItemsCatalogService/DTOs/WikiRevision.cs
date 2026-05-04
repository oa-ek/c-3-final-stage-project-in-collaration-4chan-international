using System.Text.Json.Serialization;

namespace YourDarkSoulsAssistant.GameItemsCatalogService.DTOs;

public class WikiRevision
{
    // Використовуємо ключ "*", щоб дістати HTML
    [JsonPropertyName("*")]
    public string HtmlContent { get; set; }
}
