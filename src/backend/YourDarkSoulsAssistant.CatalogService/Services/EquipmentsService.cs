using AutoMapper;
using Microsoft.EntityFrameworkCore;
using YourDarkSoulsAssistant.CatalogService.DTOs.Equipment;
using YourDarkSoulsAssistant.CatalogService.DTOs.GameItems;
using YourDarkSoulsAssistant.CatalogService.Infrastructure.Context;
using YourDarkSoulsAssistant.CatalogService.Interfaces.GameItems;


namespace YourDarkSoulsAssistant.CatalogService.Services;

public class EquipmentsService(CatalogDBContext context, IMapper mapper) : IEquipmentsService
{
    public async Task<IEnumerable<GameResponseDTO>> GetAllSupportedGamesAsync()
    {
        return mapper.Map<IEnumerable<GameResponseDTO>>(await context.Games.OrderBy(g => g.GameId).ToListAsync());
    }
    
    
    public async Task<IEnumerable<EquipmentResponseDTO>> GetAllEquipmentAsync()
    {
        var equipments = await context.Equipments
            .Include(e => e.EquipmentType)
            .Include(e => e.Slot)
            .Include(e => e.RequiredAttributes)
            .Include(e => e.Scalings)
            .Include(e => e.Influences)
            .AsNoTracking()
            .ToListAsync();

        return mapper.Map<IEnumerable<EquipmentResponseDTO>>(equipments);
    }

    public async Task<EquipmentResponseDTO?> GetEquipmentByIdAsync(Guid id)
    {
        var equipment = await context.Equipments
            .Include(e => e.EquipmentType)
            .Include(e => e.Slot)
            .Include(e => e.RequiredAttributes)
            .Include(e => e.Scalings)
            .Include(e => e.Influences)
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.Id == id);

        if (equipment == null) return null;

        return mapper.Map<EquipmentResponseDTO>(equipment);
    }
}