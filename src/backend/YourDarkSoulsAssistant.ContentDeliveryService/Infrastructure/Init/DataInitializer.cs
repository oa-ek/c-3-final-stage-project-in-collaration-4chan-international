using Microsoft.EntityFrameworkCore;
using YourDarkSoulsAssistant.ContentDeliveryService.Infrastructure.Context;

public static class DataInitializer
{
    public static async Task InitializeAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var services = scope.ServiceProvider;

        var logger = services.GetRequiredService<ILogger<ContentDeliveryDBContext>>();
        var context = services.GetRequiredService<ContentDeliveryDBContext>();

        try
        {
            logger.LogInformation("--> [Seeding]: Початок ініціалізації бази даних...");

            logger.LogInformation("--> [Seeding]: Перевірка та застосування міграцій...");
            await context.Database.MigrateAsync();
            logger.LogInformation("--> [Seeding]: ✅ Міграції успішно застосовані.");
        }
        catch (Exception ex)
        {
            logger.LogCritical(ex, "--> [Seeding]: ❌ КРИТИЧНА ПОМИЛКА під час ініціалізації!");
            throw;
        }
    }
}