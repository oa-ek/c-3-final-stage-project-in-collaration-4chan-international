using System.Text.Json.Serialization;

namespace YourDarkSoulsAssistant.GameItemsCatalogService.DTOs;

public class WikiRootResponse
{
    [JsonPropertyName("query")]
    public WikiQuery Query { get; set; }
}