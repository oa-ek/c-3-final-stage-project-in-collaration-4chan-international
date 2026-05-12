using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;
using YourDarkSoulsAssistant.ArticlesService.DTOs;
using YourDarkSoulsAssistant.ArticlesService.Interfaces.Services;
using YourDarkSoulsAssistant.ArticlesService.Interfaces.Services.External;
using YourDarkSoulsAssistant.ArticlesService.Models;
using YourDarkSoulsAssistant.ArticlesService.Services.External;


namespace YourDarkSoulsAssistant.ArticlesService.Services;

public class WikiEnrichmentService: IWikiEnrichmentService
{
    private readonly MediaWikiClient _wikiClient;
    private readonly EldenRingApiClient _imagesClient;
    private readonly IYouTubeGuidesService _youtubeService; // Твій старий сервіс
    private readonly IDistributedCache _cache;
    private readonly ILogger<WikiEnrichmentService> _logger;

    public WikiEnrichmentService(
        MediaWikiClient wikiClient,
        EldenRingApiClient imagesClient,
        IYouTubeGuidesService youtubeService,
        IDistributedCache cache,
        ILogger<WikiEnrichmentService> logger)
    {
        _wikiClient = wikiClient;
        _imagesClient = imagesClient;
        _youtubeService = youtubeService;
        _cache = cache;
        _logger = logger;
    }

    public async Task<EnrichedWikiItemDto> EnrichArticleAsync(Article article)
    {
        var cacheKey = $"wiki_article_{article.Id}";
        
        // 1. Перевіряємо Redis
        var cachedData = await _cache.GetStringAsync(cacheKey);
        if (!string.IsNullOrEmpty(cachedData))
        {
            _logger.LogInformation("Віддаємо збагачену статтю {Title} з Redis", article.Title);
            return JsonSerializer.Deserialize<EnrichedWikiItemDto>(cachedData)!;
        }

        _logger.LogInformation("Збираємо дані для {Title} із зовнішніх API...", article.Title);

        // 2. ЗАПУСКАЄМО ЗАПИТИ ПАРАЛЕЛЬНО (Це прискорює відповідь у 3 рази)
        var loreTask = _wikiClient.GetLoreAsync(article.Title);
        var imageTask = _imagesClient.GetImageUrlAsync(article.Title);
        var videosTask = _youtubeService.SearchVideosAsync(article.Title + " guide", 3);

        await Task.WhenAll(loreTask, imageTask, videosTask);

        // 3. Формуємо результат
        var statsBlock = article.Blocks.FirstOrDefault(b => b.TypeId == 1); // 1 = stats

        var enrichedDto = new EnrichedWikiItemDto
        {
            Id = article.Id.ToString(),
            Name = article.Title,
            Category = article.Type.Name,
            Description = article.Description,
            Stats = statsBlock?.ContentData, // Передаємо наш jsonb як є
            Lore = await loreTask,
            Image = await imageTask,
            Videos = await videosTask
        };

        // 4. Кешуємо у Redis на 24 години (Зовнішні дані змінюються рідко)
        var cacheOptions = new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(24) };
        await _cache.SetStringAsync(cacheKey, JsonSerializer.Serialize(enrichedDto), cacheOptions);

        return enrichedDto;
    }
}