using AutoMapper;
using Microsoft.EntityFrameworkCore;
using YourDarkSoulsAssistant.ContentDeliveryService.Infrastructure.Context;
using YourDarkSoulsAssistant.ContentDeliveryService.DTOs.Routes;
using YourDarkSoulsAssistant.ContentDeliveryService.Interfaces;
using YourDarkSoulsAssistant.ContentDeliveryService.Models;

namespace YourDarkSoulsAssistant.ContentDeliveryService.Services;

public class RouteService(ContentDeliveryDBContext dbContext, IMapper mapper, ILogger<RouteService> logger) : IRouteService
{
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
            logger.LogError(ex, "--> [RouteService]: Помилка при пошуку приватного маршруту для публічного шляху {PublicRoute}", publicRoute);
            return null;
        }
    }
    
    public async Task<ContentItem?> GetByPublicRouteAsync(string publicRoute)
    {
        var record = await dbContext.ContentItems
            .AsNoTracking()
            .FirstOrDefaultAsync(r => r.PublicRoute == publicRoute);

        return record;
    }

    
    public async Task<ContentItemDTO> RegisterRouteAsync(InputContentItemDTO inputDto, string privateRoute)
    {
        logger.LogInformation("--> [RouteService]: Реєстрація нового маршруту {PublicRoute}", inputDto.Route);
        try
        {
            var record = mapper.Map<ContentItem>(inputDto);
            
            record.Id = Guid.NewGuid();
            record.PrivateRoute = privateRoute;
            record.IsActive = true;
    
            dbContext.ContentItems.Add(record);
            await dbContext.SaveChangesAsync();
            
            logger.LogInformation("--> [RouteService]: Маршрут успішно зареєстровано");
            return mapper.Map<ContentItemDTO>(record);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "--> [RouteService]: Помилка при реєстрації маршруту {PublicRoute}", inputDto.Route);
            throw;
        }
    }
    
    public async Task<ContentItemDTO?> UpdateFileRouteAsync(Guid id, string newPrivateRoute)
    {
        logger.LogInformation("--> [RouteService]: Спроба оновлення приватного маршруту для ID {Id}", id);
        try
        {
            var record = await dbContext.ContentItems.FindAsync(id);
            if (record == null) return null;
            
            record.PrivateRoute = newPrivateRoute;
            record.IsActive = true;
            await dbContext.SaveChangesAsync();
    
            return mapper.Map<ContentItemDTO>(record);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "--> [RouteService]: Помилка при оновленні маршруту ID {Id}", id);
            return null;
        }
    }
    
    public async Task DeleteRouteAsync(Guid id)
    {
        logger.LogInformation("--> [RouteService]: Спроба видалення контенту з ID {ContentItemId}", id);
        try
        {
            var record = await dbContext.ContentItems.FindAsync(id);
            
            if (record != null)
            {
                dbContext.ContentItems.Remove(record);
                await dbContext.SaveChangesAsync();
            }
            logger.LogInformation("--> [RouteService]: Запис з ID {ContentItemId} успішно видалено", id);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "--> [RouteService]: Помилка при видаленні запису з ID {ContentItemId}", id);
        }
    }
}
