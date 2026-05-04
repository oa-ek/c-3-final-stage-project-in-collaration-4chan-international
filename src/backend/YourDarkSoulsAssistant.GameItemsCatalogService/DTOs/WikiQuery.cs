using System.Text.Json.Serialization;

namespace YourDarkSoulsAssistant.GameItemsCatalogService.DTOs;

public class WikiQuery
{
    // ОСЬ СЕКРЕТ! 
    // Ми кажемо: "Очікуй об'єкт 'pages', де ключі - це будь-які рядки (ID), 
    // а значення - це об'єкти типу WikiPage".
    [JsonPropertyName("pages")]
    public Dictionary<string, WikiPage> Pages { get; set; }
}