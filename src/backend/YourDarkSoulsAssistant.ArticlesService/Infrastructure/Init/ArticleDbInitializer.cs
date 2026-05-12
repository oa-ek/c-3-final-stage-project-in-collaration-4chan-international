using YourDarkSoulsAssistant.ArticlesService.Infrastructure.Context;
using YourDarkSoulsAssistant.Core.Infrastructure.Init;

namespace YourDarkSoulsAssistant.ArticlesService.Infrastructure.Init;

public class ArticleDbInitializer(
    ArticleDBContext context,
    ILogger<ArticleDbInitializer> logger,
    IWebHostEnvironment env)
    : BaseDataInitializer<ArticleDBContext>(context, logger, env)
{
    protected override Task SeedDataAsync()
    {
        Logger.LogInformation("--> [ArticleInit]: Додатковий сідінг не потрібен (використовується HasData).");
        return Task.CompletedTask;
    }
}
