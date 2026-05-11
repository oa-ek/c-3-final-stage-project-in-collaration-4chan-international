using YourDarkSoulsAssistant.ContentDeliveryService.DTOs.Routes;
using YourDarkSoulsAssistant.ContentDeliveryService.Models;

namespace YourDarkSoulsAssistant.ContentDeliveryService.Interfaces;

public interface IRouteService
{
    Task<string?> GetPrivateRouteAsync(string publicRoute);
    Task<ContentItem?> GetByPublicRouteAsync(string publicRoute);
    Task<ContentItemDTO> RegisterRouteAsync(InputContentItemDTO inputDto, string privateRoute);
    Task<ContentItemDTO?> UpdateFileRouteAsync(Guid id, string newPrivateRoute);
    Task DeleteRouteAsync(Guid id);
}
