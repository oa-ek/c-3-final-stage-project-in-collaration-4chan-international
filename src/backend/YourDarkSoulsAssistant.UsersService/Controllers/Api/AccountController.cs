using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YourDarkSoulsAssistant.UsersService.DTOs.Users;
using YourDarkSoulsAssistant.UsersService.Interfaces.Identity;

namespace YourDarkSoulsAssistant.UsersService.Controllers.Api;

[ApiController]
[Authorize]
[Route("[controller]")]
public class AccountController(IUserService userService) : ControllerBase
{
    [HttpGet("profile")]
    public async Task<IActionResult> GetProfile()
    {
        var profile = await userService.GetUserProfileAsync(User);

        if (profile == null) return NotFound("User not found");

        return Ok(profile);
    }

    [HttpPut("profile")]
    public async Task<IActionResult> UpdateProfile([FromBody] UpdateUserDTO model)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var success = await userService.UpdateUserProfileAsync(User, model);

        if (!success) return BadRequest("Failed to update profile");

        return Ok(new { message = "Profile updated successfully" });
    }
}
