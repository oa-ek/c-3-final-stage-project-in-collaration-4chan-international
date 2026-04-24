using YourDarkSoulsAssistant.ContentDeliveryService.DTOs.Routes;

namespace YourDarkSoulsAssistant.ContentDeliveryService.Interfaces;

public interface IRouteService
{
    Task<string?> GetPrivateRouteAsync(string publicRoute);
    
    Task<ContentItemDTO?> GetByPublicRouteAsync(string publicRoute);
    
    Task<ContentItemDTO> RegisterRouteAsync(InputContentItemDTO inputDto, string fileExtension);
    
    Task<ContentItemDTO> UpdateFileRouteAsync(Guid id, string newExtension);
    
    Task DeleteRouteAsync(Guid id);
}
