using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;
using Shared.DTO.Auth;

namespace Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(IAuthService authService, ITokenBlacklistService blacklistService)
    : ControllerBase
{
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto model)
    {
        var result = await authService.LoginAsync(model);
        if (!result.IsSuccess) return BadRequest(result.ErrorMessage);
        return Ok(result);
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequestDto model)
    {
        var result = await authService.RegisterAsync(model);
        if (!result.IsSuccess) return BadRequest(result.ErrorMessage);
        return Ok(result);
    }

    [HttpPost("logout")]
    [Authorize]
    public async Task<IActionResult> Logout()
    {
        var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

        // 1. Розбираємо токен, щоб дізнатися дату закінчення (exp)
        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(token);

        // 2. Додаємо в БД
        await blacklistService.BlacklistTokenAsync(token, jwtToken.ValidTo);

        return Ok(new { message = "Токен успішно анульовано (записано в БД)." });
    }
    
    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh([FromBody] RefreshTokenRequestDto model)
    {
        var result = await authService.RefreshTokenAsync(model);
        
        if (!result.IsSuccess) 
            return Unauthorized(result); // 401, щоб клієнт знав, що треба повний логаут
            
        return Ok(result);
    }
}