using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using YourDarkSoulsAssistant.ArticlesService.Models; 
using YourDarkSoulsAssistant.ArticlesService.DTOs;
using YourDarkSoulsAssistant.ArticlesService.Infrastructure.Context;
using YourDarkSoulsAssistant.ArticlesService.Interfaces.Services;
using YourDarkSoulsAssistant.ArticlesService.Services.External;

namespace YourDarkSoulsAssistant.ArticlesService.Controllers.Api;

[ApiController]
[Route("wiki")]
[Produces("application/json")]
public class WikiController : ControllerBase
{
    private readonly ArticleDBContext _context;
    private readonly IWikiEnrichmentService _enrichmentService;
    private readonly EldenRingApiClient _externalApiClient;

    public WikiController(
        ArticleDBContext context, 
        IWikiEnrichmentService enrichmentService,
        EldenRingApiClient externalApiClient)
    {
        _context = context;
        _enrichmentService = enrichmentService;
        _externalApiClient = externalApiClient;
    }
    
    [HttpGet]
    [ProducesResponseType(typeof(PagedResult<EnrichedWikiItemDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetLocalArticles(
        [FromQuery] string? query = "",
        [FromQuery] string category = "all",
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 12)
    {
        var dbQuery = _context.Articles
            .Include(a => a.Type)
            .AsQueryable();

        // Фільтр по категорії
        if (!string.IsNullOrWhiteSpace(category) && category.ToLower() != "all")
        {
            dbQuery = dbQuery.Where(a => a.Type.Name.ToLower() == category.ToLower());
        }

        // Фільтр по тексту
        if (!string.IsNullOrWhiteSpace(query))
        {
            var queryLower = query.ToLower();
            dbQuery = dbQuery.Where(a => a.Title.ToLower().Contains(queryLower) || 
                                         a.Description.ToLower().Contains(queryLower));
        }

        var totalItems = await dbQuery.CountAsync();

        // Отримуємо локальні сутності
        var articles = await dbQuery
            .OrderByDescending(a => a.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Include(a => a.Blocks) // Підтягуємо jsonb блоки
            .ToListAsync();

        // Оскільки це список, ми можемо або віддавати їх сирими, 
        // або прогнати через EnrichmentService (але це може бути повільно для 12 штук одночасно).
        // Для лаби краще віддати базову інфу + картинку:
        var enrichedItems = new List<EnrichedWikiItemDto>();
        foreach (var article in articles)
        {
            // Для списку можна викликати спрощений мапінг або повний (з кешем це буде швидко)
            var enriched = await _enrichmentService.EnrichArticleAsync(article);
            enrichedItems.Add(enriched);
        }

        return Ok(new PagedResult<EnrichedWikiItemDto>
        {
            Items = enrichedItems,
            TotalCount = totalItems,
            CurrentPage = page,
            PageSize = pageSize
        });
    }

    // --- 2. ЗОВНІШНІЙ ПОШУК (Для адмінки, щоб знайти що додати) ---
    [HttpGet("external-search")]
    public async Task<IActionResult> ExternalSearch([FromQuery] string category, [FromQuery] string query)
    {
        var result = await _externalApiClient.SearchItemsExternalAsync(category, query);
        if (result == null) return NotFound("Нічого не знайдено у зовнішньому API");
        
        return Ok(result);
    }

    // --- 3. ІНДЕКСАЦІЯ (Збереження в БД) ---
    [HttpPost("index")]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
    public async Task<IActionResult> IndexArticle([FromBody] CreateIndexedArticleDTO dto)
    {
        // 1. Шукаємо категорію в БД (щоб отримати TypeId)
        var type = await _context.ArticleTypes.FirstOrDefaultAsync(t => t.Name == dto.Category.ToLower());
        if (type == null) return BadRequest($"Категорію {dto.Category} не знайдено в БД.");

        // 2. Перевіряємо, чи вже немає такої статті
        if (await _context.Articles.AnyAsync(a => a.Title == dto.Title))
        {
            return Conflict("Ця стаття вже є в локальній базі.");
        }

        // 3. Створюємо статтю
        var article = new Article
        {
            Id = Guid.NewGuid(),
            Title = dto.Title,
            Subtitle = dto.Subtitle,
            Description = dto.Description,
            TypeId = type.Id,
            CreatorId = Guid.NewGuid(), // TODO: Брати з Claims (User.Identity)
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            IsChecked = true
        };

        // 4. Створюємо блок зі статами (jsonb)
        var statsBlockType = await _context.PageBlockTypes.FirstOrDefaultAsync(t => t.Name == "stats");
        if (statsBlockType != null && !string.IsNullOrWhiteSpace(dto.RawStatsJson))
        {
            var block = new ArticleBlock
            {
                Id = Guid.NewGuid(),
                EntryId = article.Id,
                OrdinalNumber = 1,
                TypeId = statsBlockType.Id,
                ContentData = JsonDocument.Parse(dto.RawStatsJson) // Парсимо рядок у jsonb
            };
            _context.ArticleBlocks.Add(block);
        }

        _context.Articles.Add(article);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetLocalArticles), new { id = article.Id }, article.Id);
    }
    
    [HttpGet("{category}/{id}")]
    [ProducesResponseType(typeof(EnrichedWikiItemDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetWikiItem(string category, Guid id)
    {
        var article = await _context.Articles
            .Include(a => a.Type)
            .Include(a => a.Blocks)
            .FirstOrDefaultAsync(a => a.Id == id && a.Type.Name == category.ToLower());

        if (article == null) return NotFound(new { Message = "Архівний запис не знайдено." });
        
        var result = await _enrichmentService.EnrichArticleAsync(article);

        return Ok(result);
    }
}
