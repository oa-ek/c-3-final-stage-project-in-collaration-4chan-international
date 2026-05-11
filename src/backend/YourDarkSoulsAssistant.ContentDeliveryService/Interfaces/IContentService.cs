namespace YourDarkSoulsAssistant.ContentDeliveryService.Interfaces;

public interface IContentService
{
    Task<Stream?> GetImageAsync(string privateRoute);

    Task<(bool IsSuccess, string PrivateRoute)> SaveImageAsync(IFormFile file, string category);
    
    Task<bool> DeleteImageAsync(string privateRoute);
}
