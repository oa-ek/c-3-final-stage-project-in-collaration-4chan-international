using System.Text.Json;

namespace YourDarkSoulsAssistant.ArticlesService.Services.External;

public class MediaWikiClient
{
    private readonly HttpClient _http;
    private readonly ILogger<MediaWikiClient> _logger;

    public MediaWikiClient(HttpClient http, ILogger<MediaWikiClient> logger)
    {
        _http = http;
        _logger = logger;
        _http.BaseAddress = new Uri("https://en.wikipedia.org/w/api.php");
        _http.DefaultRequestHeaders.Add("User-Agent", "TarnishedWikiApp/1.0");
    }

    public async Task<string?> GetLoreAsync(string title)
    {
        try
        {
            // Формуємо запит до Вікіпедії для отримання короткого тексту статті
            var query = $"?action=query&prop=extracts&exintro&explaintext&format=json&titles={Uri.EscapeDataString(title)}";
            var response = await _http.GetAsync(query);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(json);
            
            // Парсимо специфічний формат Вікіпедії
            var pages = doc.RootElement.GetProperty("query").GetProperty("pages");
            var firstPage = pages.EnumerateObject().FirstOrDefault().Value;
            
            if (firstPage.TryGetProperty("extract", out var extract))
            {
                return extract.GetString();
            }
            return null;
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "MediaWiki недоступна або не знайшла статтю для {Title}", title);
            return null; // Fallback: повертаємо null замість падіння
        }
    }
}
