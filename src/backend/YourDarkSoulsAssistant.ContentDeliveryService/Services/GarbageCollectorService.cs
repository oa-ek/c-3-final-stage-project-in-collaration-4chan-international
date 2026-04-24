using Microsoft.EntityFrameworkCore;
using YourDarkSoulsAssistant.ContentDeliveryService.Infrastructure.Context;

namespace YourDarkSoulsAssistant.ContentDeliveryService.Services;

public class GarbageCollectorService(IServiceScopeFactory scopeFactory, ILogger<GarbageCollectorService> logger)
    : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("Garbage Collector Service запущено.");
        
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await CleanupOrphanedRecordsAsync(stoppingToken);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Помилка під час очищення сміття.");
            }
            
            await Task.Delay(TimeSpan.FromHours(1), stoppingToken);
        }
    }

    private async Task CleanupOrphanedRecordsAsync(CancellationToken stoppingToken)
    {
        using var scope = scopeFactory.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ContentDeliveryDBContext>();
        
        var activeItems = await dbContext.ContentItems
            .Where(i => i.IsActive)
            .ToListAsync(stoppingToken);

        int orphanedCount = 0;
        
        foreach (var item in activeItems)
        {
            if (!File.Exists(item.PrivateRoute))
            {
                item.IsActive = false; 
                orphanedCount++;
                
                logger.LogWarning("Знайдено запис-привид! Шлях: {Route}. Запис деактивовано.", item.PublicRoute);
            }
        }
        
        if (orphanedCount > 0)
        {
            await dbContext.SaveChangesAsync(stoppingToken);
            logger.LogInformation("Очищення завершено. Деактивовано {Count} записів.", orphanedCount);
        }
    }
}
