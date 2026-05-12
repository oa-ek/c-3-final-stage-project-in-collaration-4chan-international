using System.Text.Json;

namespace YourDarkSoulsAssistant.ArticlesService.Services.External;

public class EldenRingApiClient
{
    private readonly HttpClient _http;
    private readonly ILogger<EldenRingApiClient> _logger;

    public EldenRingApiClient(HttpClient http, ILogger<EldenRingApiClient> logger)
    {
        _http = http;
        _logger = logger;
        _http.BaseAddress = new Uri("https://eldenring.fanapis.com/api/");
    }
    
    public async Task<string?> GetImageUrlAsync(string bossName)
    {
        try
        {
            // Наприклад: https://eldenring.fanapis.com/api/bosses?name=Malenia
            var url = $"bosses?name={Uri.EscapeDataString(bossName)}";
            var response = await _http.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(json);
            
            var data = doc.RootElement.GetProperty("data");
            if (data.GetArrayLength() > 0)
            {
                return data[0].GetProperty("image").GetString();
            }
            return null;
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Elden Ring API не знайшло картинку для {Name}", bossName);
            return null;
        }
    }
    public async Task<JsonDocument?> SearchItemsExternalAsync(string category, string query)
    {
        try
        {
            var url = $"{category.ToLower()}?name={Uri.EscapeDataString(query)}";
            var response = await _http.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            return JsonDocument.Parse(json);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Помилка зовнішнього пошуку в Elden Ring API для {Category}: {Query}", category, query);
            return null;
        }
    }
}