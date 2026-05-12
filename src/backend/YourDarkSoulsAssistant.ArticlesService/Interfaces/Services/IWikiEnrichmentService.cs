using YourDarkSoulsAssistant.ArticlesService.DTOs;
using YourDarkSoulsAssistant.ArticlesService.Models;

namespace YourDarkSoulsAssistant.ArticlesService.Interfaces.Services;

public interface IWikiEnrichmentService
{
    Task<EnrichedWikiItemDto> EnrichArticleAsync(Article article);
}