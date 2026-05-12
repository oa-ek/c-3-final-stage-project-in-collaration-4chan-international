using System.Text.Json;
using System.Xml;
using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using Microsoft.Extensions.Caching.Distributed;
using YourDarkSoulsAssistant.ArticlesService.DTOs;
using YourDarkSoulsAssistant.ArticlesService.Interfaces.Services.External;

namespace YourDarkSoulsAssistant.ArticlesService.Services.External;

public class YouTubeGuidesService: IYouTubeGuidesService
{
    private readonly YouTubeService _youtubeService;
    private readonly ILogger<YouTubeGuidesService> _logger;
    private readonly IDistributedCache _cache;
    
    public YouTubeGuidesService(
        IConfiguration config, 
        ILogger<YouTubeGuidesService> logger,
        IDistributedCache cache)
    {
        _logger = logger;
        _cache = cache;
        
        var apiKey = config["YouTubeApiKey"]; 
        _youtubeService = new YouTubeService(new BaseClientService.Initializer() { ApiKey = apiKey });
    }

    public async Task<List<YouTubeGuideResponseDTO>> SearchVideosCachedAsync(string query)
    {
        var cacheKey = $"youtube_search_{query.ToLower().Replace(" ", "_")}";
        
        var cachedData = await _cache.GetStringAsync(cacheKey);
        if (!string.IsNullOrEmpty(cachedData))
        {
            _logger.LogInformation("Віддаємо дані з Redis кешу для запиту: {Query}", query);
            return JsonSerializer.Deserialize<List<YouTubeGuideResponseDTO>>(cachedData)!;
        }
        
        var videos = await SearchVideosAsync(query);

        if (videos.Any())
        {
            var cacheOptions = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30)
            };
            
            await _cache.SetStringAsync(
                cacheKey, 
                JsonSerializer.Serialize(videos), 
                cacheOptions);
                
            _logger.LogInformation("Дані збережено в Redis кеш.");
        }

        return videos;
    }
    
    public async Task<List<YouTubeGuideResponseDTO>> GetVideosAsync(List<string> videoIds)
    {
        try
        {
            if (videoIds == null || !videoIds.Any())
            {
                _logger.LogWarning("Метод GetVideosAsync викликано з порожнім списком videoIds.");
                return new List<YouTubeGuideResponseDTO>();
            }

            var joinedIds = string.Join(",", videoIds);
            _logger.LogInformation("Відправляємо запит до YouTube API для ID відео: {VideoIds}", joinedIds);

            var request = _youtubeService.Videos.List("snippet,statistics,contentDetails");
            request.Id = joinedIds;

            var response = await request.ExecuteAsync();
            var result = new List<YouTubeGuideResponseDTO>();

            // 2. Перевіряємо, чи YouTube взагалі щось знайшов
            if (response.Items == null || !response.Items.Any())
            {
                _logger.LogWarning("YouTube API повернув порожню відповідь для ID: {VideoIds}. Перевір, чи ці відео існують і чи вони публічні.", joinedIds);
                return result; 
            }

            _logger.LogInformation("Отримано {Count} відео з YouTube. Починаємо конвертацію в DTO.", response.Items.Count);

            // 3. Конвертуємо дані
            foreach (var item in response.Items)
            {
                result.Add(new YouTubeGuideResponseDTO
                {
                    Id = item.Id,
                    Title = item.Snippet.Title,
                    Description = item.Snippet.Description,
                    ThumbnailUrl = item.Snippet.Thumbnails?.High?.Url ?? item.Snippet.Thumbnails?.Default__?.Url ?? "",
                    VideoUrl = $"https://youtube.com/watch?v={item.Id}",
                    Duration = FormatDuration(item.ContentDetails.Duration),
                    Category = DetermineCategory(item.Snippet.Tags), 
                    Game = "elden-ring",
                    Author = item.Snippet.ChannelTitle,
                    Views = item.Statistics.ViewCount ?? 0,
                    Likes = item.Statistics.LikeCount ?? 0,
                    PublishedAt = item.Snippet.PublishedAtDateTimeOffset?.DateTime ?? DateTime.UtcNow,
                    Tags = item.Snippet.Tags?.ToList() ?? new List<string>()
                });
            }

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Помилка при отриманні деталей відео з YouTube.");
            return new List<YouTubeGuideResponseDTO>();
        }
    }
    
    public async Task<List<YouTubeGuideResponseDTO>> SearchVideosAsync(string query, int maxResults = 8)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                _logger.LogInformation("Порожній запит на пошук. Повертаємо порожній список.");
                return new List<YouTubeGuideResponseDTO>();
            }

            _logger.LogInformation("Шукаємо відео на YouTube за запитом: {Query}", query);

            var safeQuery = $"{query} (Elden Ring OR Dark Souls OR Bloodborne OR Sekiro)";

            var searchRequest = _youtubeService.Search.List("snippet");
            searchRequest.Q = safeQuery;
            searchRequest.Type = "video";
            searchRequest.VideoCategoryId = "20";
            searchRequest.SafeSearch = SearchResource.ListRequest.SafeSearchEnum.Strict;
            searchRequest.MaxResults = maxResults;

            var searchResponse = await searchRequest.ExecuteAsync();

            var videoIds = searchResponse.Items.Select(item => item.Id.VideoId).ToList();

            if (!videoIds.Any())
            {
                _logger.LogWarning("Нічого не знайдено за запитом: {Query}", query);
                return new List<YouTubeGuideResponseDTO>();
            }

            _logger.LogInformation("Знайдено {Count} відео. Отримуємо деталі...", videoIds.Count);
            
            return await GetVideosAsync(videoIds);
        }
        catch (Google.GoogleApiException ex)
        {
            _logger.LogWarning(ex, "YouTube API відхилив запит (можливо ліміт або ключ). Віддаємо порожній список.");
            return new List<YouTubeGuideResponseDTO>(); 
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Невідома помилка при роботі з YouTube API.");
            return new List<YouTubeGuideResponseDTO>();
        }
    }

    private string FormatDuration(string isoDuration)
    {
        try
        {
            var timeSpan = XmlConvert.ToTimeSpan(isoDuration);
            return timeSpan.ToString(timeSpan.Hours > 0 ? @"h\:mm\:ss" : @"m\:ss");
        }
        catch
        {
            return "0:00";
        }
    }

    private string DetermineCategory(IList<string>? tags)
    {
        if (tags == null) return "walkthrough";
        var lowerTags = tags.Select(t => t.ToLower()).ToList();
        
        if (lowerTags.Contains("boss") || lowerTags.Contains("boss guide")) return "boss";
        if (lowerTags.Contains("build") || lowerTags.Contains("strength")) return "build";
        if (lowerTags.Contains("lore")) return "lore";
        if (lowerTags.Contains("pvp")) return "pvp";
        
        return "walkthrough";
    }
}
