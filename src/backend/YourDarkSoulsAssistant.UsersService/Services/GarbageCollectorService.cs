using Microsoft.EntityFrameworkCore;
using YourDarkSoulsAssistant.UsersService.Infrastructure.Context;

namespace YourDarkSoulsAssistant.UsersService.Services;

public class GarbageCollectorService(IServiceScopeFactory scopeFactory, ILogger<GarbageCollectorService> logger)
    : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("--> [GarbageCollector]: Service запущено. Інтервал: 1 день.");
        
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await CleanupOrphanedRecordsAsync(stoppingToken);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "--> [GarbageCollector]: ❌ КРИТИЧНА ПОМИЛКА під час очищення сміття.");
            }
            
            await Task.Delay(TimeSpan.FromDays(1), stoppingToken);
        }
    }

    private async Task CleanupOrphanedRecordsAsync(CancellationToken stoppingToken)
    {
        using var scope = scopeFactory.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<UserDBContext>();
        
        var deletedCount = await dbContext.RevokedTokens
            .Where(t => t.ExpirationDate < DateTime.UtcNow)
            .ExecuteDeleteAsync(stoppingToken);
        
        if (deletedCount > 0)
        {
            logger.LogInformation("--> [GarbageCollector]: ✅ Очищення завершено. Знищено {Count} прострочених токенів.", deletedCount);
        }
    }
}
