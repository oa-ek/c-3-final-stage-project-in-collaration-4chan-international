using AutoMapper;
using Microsoft.EntityFrameworkCore;
using YourDarkSoulsAssistant.ContentDeliveryService.Infrastructure.Context;
using YourDarkSoulsAssistant.ContentDeliveryService.DTOs.Routes;
using YourDarkSoulsAssistant.ContentDeliveryService.Interfaces;
using YourDarkSoulsAssistant.ContentDeliveryService.Models;

namespace YourDarkSoulsAssistant.ContentDeliveryService.Services;

public class RouteService(ContentDeliveryDBContext dbContext, IMapper mapper, IServiceProvider serviceProvider) : IRouteService
{
    private readonly ILogger<RouteService> _logger = serviceProvider.GetRequiredService<ILogger<RouteService>>();
    
    public async Task<string?> GetPrivateRouteAsync(string publicRoute)
    {
        try
        {
            var record = await dbContext.ContentItems
                .AsNoTracking()
                .FirstOrDefaultAsync(r => r.PublicRoute == publicRoute && r.IsActive);

            return record?.PrivateRoute;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "--> [RouteService]: Помилка при пошуку приватного маршруту для публічного шляху {PublicRoute}", publicRoute);
            return null;
        }
    }
    
    public async Task<ContentItemDTO?> GetByPublicRouteAsync(string publicRoute)
    {
        var record = await dbContext.ContentItems
            .AsNoTracking()
            .FirstOrDefaultAsync(r => r.PublicRoute == publicRoute);

        return record == null ? null : mapper.Map<ContentItemDTO>(record);
    }

    
    public async Task<ContentItemDTO> RegisterRouteAsync(InputContentItemDTO inputDto, string fileExtension)
    {
        _logger.LogInformation("--> [RouteService]: Реєстрація нового маршруту для контенту з публічним шляхом {PublicRoute}", inputDto.Route);
        try
        {
            var uniqueId = Guid.NewGuid();
            var privateRoute = $"/app/data/content/{uniqueId}{fileExtension}";
            
            var record = mapper.Map<ContentItem>(inputDto);
            
            record.Id = uniqueId;
            record.PrivateRoute = privateRoute;
            record.IsActive = true;
    
            dbContext.ContentItems.Add(record);
            await dbContext.SaveChangesAsync();
            _logger.LogInformation("--> [RouteService]: Маршрут успішно зареєстровано з приватним шляхом {PrivateRoute}", privateRoute);
            return mapper.Map<ContentItemDTO>(record);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "--> [RouteService]: Помилка при реєстрації маршруту для контенту з публічним шляхом {PublicRoute}", inputDto.Route);
            throw;
        }
    }
    
    public async Task<ContentItemDTO> UpdateFileRouteAsync(Guid id, string newExtension)
    {
        _logger.LogInformation("--> [RouteService]: Спроба оновлення приватного маршруту для контенту з ID {ContentItemId}", id);
        try
        {
            var record = await dbContext.ContentItems.FindAsync(id);
            
            if (record == null) return null;
            
            record.PrivateRoute = $"/app/data/content/{Guid.NewGuid()}{newExtension}";
            
            await dbContext.SaveChangesAsync();
    
            return mapper.Map<ContentItemDTO>(record);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "--> [RouteService]: Помилка при оновленні приватного маршруту для запису з ID {ContentItemId}", id);
            return null;
        }
    }
    
    public async Task DeleteRouteAsync(Guid id)
    {
        _logger.LogInformation("--> [RouteService]: Спроба видалення контенту з ID {ContentItemId}", id);
        try
        {
            var record = await dbContext.ContentItems.FindAsync(id);
            
            if (record != null)
            {
                dbContext.ContentItems.Remove(record);
                await dbContext.SaveChangesAsync();
            }
            _logger.LogInformation("--> [RouteService]: Запис з ID {ContentItemId} успішно видалено", id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "--> [RouteService]: Помилка при видаленні запису з ID {ContentItemId}", id);
        }
    }
}
