using Microsoft.AspNetCore.Mvc;
using YourDarkSoulsAssistant.GameItemsCatalogService.Services;

namespace YourDarkSoulsAssistant.GameItemsCatalogService.Controllers.API;

[ApiController]
[Route("api/[controller]")]
public class WeaponController(OutsideGameItemsService itemsService) : ControllerBase
{
    [HttpGet("weapons")]
    public async Task<ActionResult<string>> GetWeaponByName(string weaponName)
    {
        var content = await itemsService.GetItemHtmlByNameAsync(weaponName);
        
        return Ok(content);
    }
}
