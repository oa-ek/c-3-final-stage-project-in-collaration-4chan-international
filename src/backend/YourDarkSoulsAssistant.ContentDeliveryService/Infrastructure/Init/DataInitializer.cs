using YourDarkSoulsAssistant.ContentDeliveryService.Infrastructure.Context;
using YourDarkSoulsAssistant.Core.Infrastructure.Init;

namespace YourDarkSoulsAssistant.ContentDeliveryService.Infrastructure.Init;

public class ContentDataInitializer(
    ContentDeliveryDBContext context, 
    ILogger<ContentDataInitializer> logger, 
    IWebHostEnvironment env) :
    BaseDataInitializer<ContentDeliveryDBContext>(context, logger, env)
{
    protected override async Task SeedDataAsync()
    {
        Logger.LogInformation("--> [Seeding]: Для сервісу контенту тестові дані не потрібні, пропускаємо...");
    }
}
