using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;
using Shared.Dtos;

namespace Server.Controllers;

[ApiController]
[Authorize] // 🔥 Тільки для залогінених!
[Route("api/[controller]")]
public class AccountController : ControllerBase
{
    private readonly IUserService _userService;

    public AccountController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet("profile")]
    public async Task<IActionResult> GetProfile()
    {
        // User (ClaimsPrincipal) доступний у контролері автоматично завдяки Authorize + JWT
        var profile = await _userService.GetUserProfileAsync(User);

        if (profile == null) return NotFound("User not found");

        return Ok(profile);
    }

    [HttpPut("profile")]
    public async Task<IActionResult> UpdateProfile([FromBody] UserDto model)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var success = await _userService.UpdateUserProfileAsync(User, model);

        if (!success) return BadRequest("Failed to update profile");

        return Ok(new { message = "Profile updated successfully" });
    }
}