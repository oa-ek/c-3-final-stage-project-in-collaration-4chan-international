using Microsoft.EntityFrameworkCore;
using YourDarkSoulsAssistant.ContentDeliveryService.Infrastructure.Context;

namespace YourDarkSoulsAssistant.ContentDeliveryService.Services;

public class GarbageCollectorService(
    IServiceScopeFactory scopeFactory, 
    IConfiguration config, 
    ILogger<GarbageCollectorService> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("--> [GC]: Garbage Collector Service запущено.");
        
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var scope = scopeFactory.CreateScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<ContentDeliveryDBContext>();
                
                await HealDatabaseAsync(dbContext, stoppingToken);
                
                await CleanPhysicalDiskAsync(dbContext, stoppingToken);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "--> [GC]: ❌ Помилка під час очищення сміття.");
            }
            
            await Task.Delay(TimeSpan.FromSeconds(15), stoppingToken);
        }
    }

 private async Task HealDatabaseAsync(ContentDeliveryDBContext dbContext, CancellationToken stoppingToken)
    {
        var activeItems = dbContext.ContentItems
            .Where(i => i.IsActive)
            .AsAsyncEnumerable();

        int orphanedCount = 0;
        
        // Додав нормалізацію кореневого шляху (як у методі CleanPhysicalDisk)
        var rawStoragePath = config["App:StoragePath"]!;
        var storageRoot = Path.GetFullPath(rawStoragePath); 
        
        await foreach (var item in activeItems.WithCancellation(stoppingToken))
        {
            // Path.GetFullPath нормалізує змішані слеші ("\" та "/") в єдиний стандарт поточної ОС
            var absolutePath = Path.GetFullPath(Path.Combine(storageRoot, item.PrivateRoute));
            
            if (!File.Exists(absolutePath))
            {
                item.IsActive = false;
                orphanedCount++;
                logger.LogWarning("--> [GC]: Знайдено запис-привид! Шлях: {Route}. Запис деактивовано.", item.PublicRoute);
            }
        }
        
        if (orphanedCount > 0)
        {
            await dbContext.SaveChangesAsync(stoppingToken);
            logger.LogInformation("--> [GC]: ✅ Зцілення БД завершено. Деактивовано {Count} записів.", orphanedCount);
        }
    }

    private async Task CleanPhysicalDiskAsync(ContentDeliveryDBContext dbContext, CancellationToken stoppingToken)
    {
        var rawStoragePath = config["App:StoragePath"]!;
        var storagePath = Path.GetFullPath(rawStoragePath);

        if (!Directory.Exists(storagePath))
        {
            logger.LogWarning("--> [GC]: Директорію сховища {Path} не знайдено, пропускаємо очищення диска.", storagePath);
            return;
        }

        logger.LogInformation("--> [GC]: Початок сканування фізичного диска у {Path}...", storagePath);
        
        // 1. Спочатку витягуємо ВІДНОСНІ шляхи з БД у пам'ять
        // (не можна використовувати Path.GetFullPath прямо в запиті LINQ, бо EF не зможе перекласти це в SQL)
        var rawActiveRoutes = await dbContext.ContentItems
            .Where(i => i.IsActive)
            .Select(i => i.PrivateRoute)
            .ToListAsync(stoppingToken);

        // 2. Тепер у пам'яті клеїмо їх і НОРМАЛІЗУЄМО слеші через Path.GetFullPath
        var activePaths = new HashSet<string>(
            rawActiveRoutes.Select(route => Path.GetFullPath(Path.Combine(storagePath, route))),
            StringComparer.OrdinalIgnoreCase 
        );
        
        var allFilesOnDisk = Directory.GetFiles(storagePath, "*.*", SearchOption.AllDirectories);
        int deletedFilesCount = 0;

        foreach (var filePath in allFilesOnDisk)
        {
            // GetFiles вже повертає абсолютні шляхи, але для надійності нормалізуємо і їх
            var fullPath = Path.GetFullPath(filePath);

            // Тепер порівняння спрацює на 100% точно!
            if (!activePaths.Contains(fullPath))
            {
                try
                {
                    File.Delete(fullPath); // Видаляємо нормалізований шлях
                    deletedFilesCount++;
                    logger.LogInformation("--> [GC]: 🗑️ Видалено файл-сироту: {Path}", fullPath);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "--> [GC]: ❌ Не вдалося видалити файл-сироту: {Path}", fullPath);
                }
            }
        }

        if (deletedFilesCount > 0)
        {
            logger.LogInformation("--> [GC]: ✅ Очищення диска завершено. Видалено {Count} файлів-сиріт.", deletedFilesCount);
        }
    }
}
