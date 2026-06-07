using YourDarkSoulsAssistant.CatalogService.DTOs.Equipment;
using YourDarkSoulsAssistant.CatalogService.DTOs.GameItems;

namespace YourDarkSoulsAssistant.CatalogService.Interfaces.GameItems;

public interface IEquipmentsService
{
    Task<IEnumerable<GameResponseDTO>> GetAllSupportedGamesAsync();
    
    Task<IEnumerable<EquipmentResponseDTO>> GetAllEquipmentAsync();
    Task<EquipmentResponseDTO?> GetEquipmentByIdAsync(Guid id);
}
