using YourDarkSoulsAssistant.ContentDeliveryService.Interfaces;

namespace YourDarkSoulsAssistant.ContentDeliveryService.Services;

public class ContentService(IServiceProvider serviceProvider) : IContentService
{
    private readonly ILogger<ContentService> _logger = serviceProvider.GetRequiredService<ILogger<ContentService>>(); 
    
    public Task<Stream?> GetImageAsync(string privateRoute)
    {
        if (!File.Exists(privateRoute))
        {
            _logger.LogWarning("--> [ContentService]: Файл не знайдено на диску за шляхом {PrivateRoute}", privateRoute);
            return Task.FromResult<Stream?>(null);
        }

        try
        {
            Stream stream = new FileStream(privateRoute, FileMode.Open, FileAccess.Read, FileShare.Read);
            return Task.FromResult<Stream?>(stream);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "--> [ContentService]: Помилка при відкритті файлу на диску за шляхом {PrivateRoute}", privateRoute);
            return Task.FromResult<Stream?>(null);
        }
    }
    
    public async Task<bool> SaveImageAsync(IFormFile file, string privateRoute)
    {
        _logger.LogInformation("--> [ContentService]: Спроба збереження файлу на диск за шляхом {PrivateRoute}", privateRoute);
        
        try
        {
            var directory = Path.GetDirectoryName(privateRoute);
            
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                Directory.CreateDirectory(directory);
            
            await using var stream = file.OpenReadStream();
            await using var fileStream = new FileStream(privateRoute, FileMode.Create, FileAccess.Write);
            await stream.CopyToAsync(fileStream);
            
            _logger.LogInformation("--> [ContentService]: Файл успішно збережено на диск за шляхом {PrivateRoute}", privateRoute);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "--> [ContentService]: Помилка при збереженні файлу на диск за шляхом {PrivateRoute}", privateRoute);
            return false;
        }
    }

    public Task<bool> DeleteImageAsync(string privateRoute)
    {
        _logger.LogInformation("--> [ContentService]: Спроба видалення файлу з диску за шляхом {PrivateRoute}", privateRoute);
        try
        {
            if (File.Exists(privateRoute))
                File.Delete(privateRoute);
            
            _logger.LogInformation("--> [ContentService]: Файл успішно видалено з диску за шляхом {PrivateRoute}", privateRoute);
            return Task.FromResult(true);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "--> [ContentService]: Помилка при видаленні файлу з диску за шляхом {PrivateRoute}", privateRoute);
            return Task.FromResult(false);
        }
    }
}
