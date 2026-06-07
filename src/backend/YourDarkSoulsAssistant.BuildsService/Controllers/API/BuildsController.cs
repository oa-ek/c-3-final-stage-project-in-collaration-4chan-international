using Microsoft.AspNetCore.Mvc;
using YourDarkSoulsAssistant.BuildsService.DTOs.Builds;
using YourDarkSoulsAssistant.BuildsService.Interfaces;

namespace YourDarkSoulsAssistant.BuildsService.Controllers.API;

[ApiController]
[Route("[controller]")]
public class BuildsController(IBuildsService buildsService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetBuilds([FromQuery] string? userId = null)
    {
        var builds = await buildsService.GetAllBuildsAsync(userId);

        return Ok(builds);
    }

    [HttpPost]
    public async Task<IActionResult> CreateBuild([FromBody] CreateBuildRequestDto request)
    {
        var build = await buildsService.CreateBuildAsync(request);
        return Created($"/Builds/{build.Id}", build);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateBuild([FromRoute] Guid id, [FromBody] UpdateBuildRequestDto request)
    {
        var build = await buildsService.UpdateBuildAsync(id, request);
        if (build is null)
        {
            return NotFound();
        }

        return Ok(build);
    }
}
