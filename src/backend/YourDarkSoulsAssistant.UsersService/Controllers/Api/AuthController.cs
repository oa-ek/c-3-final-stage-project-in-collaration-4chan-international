using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YourDarkSoulsAssistant.UsersService.DTOs.Auth;
using YourDarkSoulsAssistant.UsersService.DTOs.Users;
using YourDarkSoulsAssistant.UsersService.Interfaces.Identity;

namespace YourDarkSoulsAssistant.UsersService.Controllers.Api;

[ApiController]
[Route("[controller]")]
public class AuthController(
    IAuthService authService,
    IUserService userService,
    ITokenBlacklistService blacklistService)
    : ControllerBase
{
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDTO model)
    {
        var result = await authService.LoginAsync(model);
        
        if (!result.IsSuccess) return BadRequest(result);
        
        return Ok(result);
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] CreateUserRequestDTO model)
    {
        var userResult = await userService.CreateUserAsync(model);
        
        if (!userResult.IsSuccess) return BadRequest(userResult);
        
        var tokenResult = await authService.IssueTokensAsync(userResult.Data!, rememberMe: false);
    
        if (!tokenResult.IsSuccess) return StatusCode(500, new { message = tokenResult.ErrorMessage });
        
        return Ok(tokenResult.Data);
    }

    [HttpPost("logout")]
    [Authorize]
    public async Task<IActionResult> Logout()
    {
        var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
        
        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(token);
        
        await blacklistService.BlacklistTokenAsync(token, jwtToken.ValidTo);

        return Ok(new { message = "Токен успішно анульовано (записано в БД)." });
    }
    
    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh([FromBody] RefreshTokenRequestDTO model)
    {
        var result = await authService.RefreshTokenAsync(model);
        
        if (!result.IsSuccess) return Unauthorized(result);
            
        return Ok(result);
    }
}
