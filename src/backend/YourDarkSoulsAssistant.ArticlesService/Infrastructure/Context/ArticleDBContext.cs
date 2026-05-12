using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using YourDarkSoulsAssistant.ArticlesService.Models;


namespace YourDarkSoulsAssistant.ArticlesService.Infrastructure.Context;

public class ArticleDBContext: DbContext
{
    public ArticleDBContext() { }
    
    public ArticleDBContext(DbContextOptions<ArticleDBContext> options) : base(options) { }

    public DbSet<ArticleType> ArticleTypes { get; set; }
    public DbSet<Article> Articles { get; set; }
    public DbSet<PageBlockType> PageBlockTypes { get; set; }
    public DbSet<ArticleBlock> ArticleBlocks { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // 1. Базові типи
        modelBuilder.Entity<ArticleType>().HasData(
            new ArticleType { Id = 1, Name = "bosses", Description = "Game Bosses" },
            new ArticleType { Id = 2, Name = "weapons", Description = "Game Weapons" }
        );

        modelBuilder.Entity<PageBlockType>().HasData(
            new PageBlockType { Id = 1, Name = "stats" }, 
            new PageBlockType { Id = 2, Name = "lore" }   
        );

        // 2. Статичні GUID та Дати для уникнення PendingModelChangesWarning
        var maleniaId = Guid.Parse("550e8400-e29b-41d4-a716-446655440000");
        var adminId = Guid.Parse("11111111-1111-1111-1111-111111111111"); // Захардкоджений GUID
        var staticDate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc); // Захардкоджена дата
        
        modelBuilder.Entity<Article>().HasData(
            new Article
            {
                Id = maleniaId,
                Title = "Malenia, Blade of Miquella",
                Subtitle = "Demigod Boss",
                Description = "The hardest boss in the game. Malenia has never known defeat.",
                Rating = 5.0f,
                CreatorId = adminId, // Використовуємо статичний
                CreatedAt = staticDate, // Використовуємо статичну дату
                UpdatedAt = staticDate, // Використовуємо статичну дату
                IsChecked = true,
                TypeId = 1
            }
        );

        var statsJson = JsonSerializer.SerializeToDocument(new { HP = 33251, Defense = 110, Stance = 80 });
        var blockId = Guid.Parse("22222222-2222-2222-2222-222222222222");
        
        modelBuilder.Entity<ArticleBlock>().HasData(
            new ArticleBlock
            {
                Id = blockId, // Використовуємо статичний
                EntryId = maleniaId,
                OrdinalNumber = 1,
                TypeId = 1,
                ContentData = statsJson
            }
        );
    }
}
