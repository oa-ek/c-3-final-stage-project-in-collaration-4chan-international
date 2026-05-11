using YourDarkSoulsAssistant.ContentDeliveryService.Interfaces;

namespace YourDarkSoulsAssistant.ContentDeliveryService.Services;

public class ContentService(ILogger<ContentService> logger, IConfiguration config) : IContentService
{
    private string GetAbsoluteStorageRoot()
    {
        var rawStorageRoot = config["App:StoragePath"] ?? "Uploads";
        return Path.GetFullPath(rawStorageRoot);
    }
    
    public Task<Stream?> GetImageAsync(string privateRoute)
    {
        var absolutePath = Path.Combine(GetAbsoluteStorageRoot(), privateRoute);
        
        if (!File.Exists(absolutePath))
        {
            logger.LogWarning("--> [ContentService]: Файл не знайдено на диску за шляхом {PrivateRoute}", privateRoute);
            return Task.FromResult<Stream?>(null);
        }

        try
        {
            Stream stream = new FileStream(absolutePath, FileMode.Open, FileAccess.Read, FileShare.Read);
            return Task.FromResult<Stream?>(stream);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "--> [ContentService]: Помилка при відкритті файлу на диску за шляхом {PrivateRoute}", privateRoute);
            return Task.FromResult<Stream?>(null);
        }
    }
    
    public async Task<(bool IsSuccess, string PrivateRoute)> SaveImageAsync(IFormFile file, string category)
    {
        try
        {
            // 1. Генеруємо ВІДНОСНИЙ шлях (напр: "system/2026.05/uuid.jpg")
            var relativeRoute = GenerateRelativePath(category, file.FileName);
            
            // 2. Будуємо ПОВНИЙ шлях для збереження на диск
            var absolutePath = Path.Combine(GetAbsoluteStorageRoot(), relativeRoute);
            
            var directory = Path.GetDirectoryName(absolutePath);
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                Directory.CreateDirectory(directory);
            
            await using var stream = file.OpenReadStream();
            await using var fileStream = new FileStream(absolutePath, FileMode.Create, FileAccess.Write);
            await stream.CopyToAsync(fileStream);
            
            // 3. ПОВЕРТАЄМО ВІДНОСНИЙ ШЛЯХ (щоб RouteService зберіг його в БД)
            return (true, relativeRoute);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "--> [ContentService]: Помилка збереження.");
            return (false, string.Empty);
        }
    }

    public Task<bool> DeleteImageAsync(string privateRoute)
    {
        var absolutePath = Path.Combine(GetAbsoluteStorageRoot(), privateRoute);
        
        logger.LogInformation("--> [ContentService]: Спроба видалення файлу з диску за шляхом {PrivateRoute}", privateRoute);
        try
        {
            if (File.Exists(absolutePath)) File.Delete(absolutePath);
            
            logger.LogInformation("--> [ContentService]: Файл успішно видалено з диску за шляхом {PrivateRoute}", privateRoute);
            return Task.FromResult(true);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "--> [ContentService]: Помилка при видаленні файлу з диску за шляхом {PrivateRoute}", privateRoute);
            return Task.FromResult(false);
        }
    }
    
    private string GenerateRelativePath(string category, string originalFileName)
    {
        var safeCategory = Path.GetFileName(category); 
        if (string.IsNullOrWhiteSpace(safeCategory)) safeCategory = "uncategorized";

        var yearMonth = DateTime.UtcNow.ToString("yyyy.MM"); // До речі, крапка тут безпечніша для URL
        var extension = Path.GetExtension(originalFileName);
        var uniqueFileName = $"{Guid.NewGuid():N}{extension}";

        return Path.Combine(safeCategory, yearMonth, uniqueFileName).Replace("\\", "/"); 
    }
}
