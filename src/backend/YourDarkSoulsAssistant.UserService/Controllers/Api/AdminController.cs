using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YourDarkSoulsAssistant.UserService.DTOs.Users;
using YourDarkSoulsAssistant.UserService.Interfaces.Identity;

namespace YourDarkSoulsAssistant.UserService.Controllers.Api;

[ApiController]
[Authorize(Roles = "Admin")]
[Route("[controller]")]
public class AdminController(IUserService userService) : ControllerBase
{
    [HttpGet("all-users")]
    public async Task<IActionResult> GetAllUsers()
    {
        var result = await userService.GetAllUsersAsync();
        
        if (result == null) return NotFound("No users found");
        
        return Ok(result);
    }
    
    [HttpGet("all-roles")]
    public async Task<IActionResult> GetAllRoles()
    {
        var result = await userService.GetAllRolesAsync();
        
        if (result == null) return NotFound("No roles found");
        
        return Ok(result);
    }
    
    [HttpPost("change-role")]
    public async Task<IActionResult> ChangeUserRole([FromBody] ChangeRoleDTO request)
    {
        var result = await userService.UpdateUserRoleAsync(request.Id, request.Roles);
    
        if (!result) return BadRequest("Failed to update user role");

        return Ok();
    }
}
