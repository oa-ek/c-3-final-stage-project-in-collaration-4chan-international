using Microsoft.AspNetCore.Mvc;
using YourDarkSoulsAssistant.CatalogService.DTOs.GameItems;
using YourDarkSoulsAssistant.CatalogService.Interfaces.GameItems;

namespace YourDarkSoulsAssistant.CatalogService.Controllers.API;

[ApiController]
[Route("[controller]")]
public class GameItemsController(IEquipmentsService equipmentsService): ControllerBase
{
    [HttpGet("supported-games")]
    public async Task<IActionResult> GetSupportedGames()
    {
        var games = await equipmentsService.GetAllSupportedGamesAsync();

        return Ok(games);
    }
    
    [HttpGet("equipments")]
    public async Task<IActionResult> GetAvailableEquipment()
    {
        var equipments = await equipmentsService.GetAllEquipmentAsync();
        
        return Ok(equipments);
    }
}
