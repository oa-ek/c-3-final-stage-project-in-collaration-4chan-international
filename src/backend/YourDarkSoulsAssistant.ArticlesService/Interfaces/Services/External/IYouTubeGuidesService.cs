using YourDarkSoulsAssistant.ArticlesService.DTOs;

namespace YourDarkSoulsAssistant.ArticlesService.Interfaces.Services.External;

public interface IYouTubeGuidesService
{
    Task<List<YouTubeGuideResponseDTO>> SearchVideosCachedAsync(string query);
    Task<List<YouTubeGuideResponseDTO>> GetVideosAsync(List<string> videoIds);
    Task<List<YouTubeGuideResponseDTO>> SearchVideosAsync(string query, int maxResults = 8);
}