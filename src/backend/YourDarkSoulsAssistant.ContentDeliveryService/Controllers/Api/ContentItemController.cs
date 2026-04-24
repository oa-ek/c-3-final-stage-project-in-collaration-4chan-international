using Microsoft.AspNetCore.Mvc;
using YourDarkSoulsAssistant.ContentDeliveryService.DTOs.Routes;
using YourDarkSoulsAssistant.ContentDeliveryService.Interfaces;

namespace YourDarkSoulsAssistant.ContentDeliveryService.Controllers.Api;

[ApiController]
[Route("[controller]")]
public class ContentItemController(IRouteService routeService, IContentService contentService) : ControllerBase
{ 
    [HttpPost("upload")]
    public async Task<IActionResult> UploadImage(IFormFile file, [FromForm] InputContentItemDTO inputDto)
    {
        if (file == null || file.Length == 0) return BadRequest("Файл порожній.");

        var extension = Path.GetExtension(file.FileName);
        
        var existingItem = await routeService.GetByPublicRouteAsync(inputDto.Route);
        
        string? oldPrivateRoute = null;
        ContentItemDTO routeInfo;

        if (existingItem != null)
        {
            oldPrivateRoute = existingItem.PrivateRoute;
            
            routeInfo = await routeService.UpdateFileRouteAsync(existingItem.Id, extension);
            
            if (routeInfo == null) return StatusCode(500, "Помилка оновлення бази даних.");
        }
        else
        {
            routeInfo = await routeService.RegisterRouteAsync(inputDto, extension);
        }
        
        var isSaved = await contentService.SaveImageAsync(file, routeInfo.PrivateRoute);
        
        if (!isSaved)
        {
            if (existingItem == null) await routeService.DeleteRouteAsync(routeInfo.Id);
            
            return StatusCode(500, "Не вдалося зберегти файл на диск.");
        }
        
        if (oldPrivateRoute != null) await contentService.DeleteImageAsync(oldPrivateRoute);

        return Ok(routeInfo);
    }
    
    [HttpGet("{*publicRoute}")]
    public async Task<IActionResult> GetImage(string publicRoute)
    {
        var privateRoute = await routeService.GetPrivateRouteAsync(publicRoute);
        
        if (string.IsNullOrEmpty(privateRoute)) return NotFound("Контент не знайдено в базі.");
        
        var imageStream = await contentService.GetImageAsync(privateRoute);
        
        if (imageStream == null) return NotFound("Файл зареєстровано, але фізично він відсутній на диску.");
        
        var ext = Path.GetExtension(privateRoute).ToLower();
        var contentType = ext switch
        {
            ".png" => "image/png",
            ".jpg" or ".jpeg" => "image/jpeg",
            ".webp" => "image/webp",
            ".txt" => "text/plain",
            ".json" => "application/json",
            _ => "application/octet-stream"
        };
        
        return File(imageStream, contentType);
    }

    [HttpGet("download")]
    public async Task<IActionResult> GetImageByQuery([FromQuery] string publicRoute)
    {
        return await GetImage(publicRoute);
    }
}
