using DarkSoulsBuildsAssistant.Core.DTOs.System;
using DarkSoulsBuildsAssistant.Core.Interfaces.Services.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DarkSoulsBuildsAssistant.App.Controllers.API;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class AccountController(IUserService userService) : ControllerBase
{
    [HttpGet("profile")]
    public async Task<IActionResult> GetProfile()
    {
        // User (ClaimsPrincipal) доступний у контролері автоматично завдяки Authorize + JWT
        var profile = await userService.GetUserProfileAsync(User);

        if (profile == null) return NotFound("User not found");

        return Ok(profile);
    }

    [HttpPut("profile")]
    public async Task<IActionResult> UpdateProfile([FromBody] UserDTO model)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var success = await userService.UpdateUserProfileAsync(User, model);

        if (!success) return BadRequest("Failed to update profile");

        return Ok(new { message = "Profile updated successfully" });
    }
}
