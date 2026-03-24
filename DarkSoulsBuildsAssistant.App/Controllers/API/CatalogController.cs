using Microsoft.AspNetCore.Mvc;
using DarkSoulsBuildsAssistant.Core.Interfaces.Services.Business;
using DarkSoulsBuildsAssistant.Core.DTOs.Equipment;

namespace DarkSoulsBuildsAssistant.App.Controllers.Api;

[ApiController]
[Route("api/[controller]")]
public class CatalogController(ICatalogService catalogService) : ControllerBase
{
    // GET: api/catalogapi/weapons
    [HttpGet("weapons")]
    public async Task<ActionResult<IEnumerable<EquipmentDTO>>> GetWeapons()
    {
        var weapons = await catalogService.GetAllWeaponsAsync();
        return Ok(weapons);
    }

    // GET: api/catalogapi/armors
    [HttpGet("armors")]
    public async Task<ActionResult<IEnumerable<EquipmentDTO>>> GetArmors()
    {
        var armors = await catalogService.GetAllArmorAsync();
        return Ok(armors);
    }
}
