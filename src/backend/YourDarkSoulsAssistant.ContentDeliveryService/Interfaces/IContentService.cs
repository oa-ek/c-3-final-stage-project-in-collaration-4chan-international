namespace YourDarkSoulsAssistant.ContentDeliveryService.Interfaces;

public interface IContentService
{
    Task<Stream?> GetImageAsync(string privateRoute);
    
    Task<bool> SaveImageAsync(IFormFile file, string privateRoute);
    
    Task<bool> DeleteImageAsync(string privateRoute);
}
