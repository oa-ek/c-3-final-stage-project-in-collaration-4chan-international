using YourDarkSoulsAssistant.Core.DTOs;
using YourDarkSoulsAssistant.UsersService.DTOs.Auth;
using YourDarkSoulsAssistant.UsersService.Models;

namespace YourDarkSoulsAssistant.UsersService.Interfaces.Identity;

public interface IAuthService
{
    Task<HTTPResult<AuthSuccessResponseDTO>> LoginAsync(LoginRequestDTO model);
    Task<HTTPResult<AuthSuccessResponseDTO>> RefreshTokenAsync(RefreshTokenRequestDTO model);

    Task<HTTPResult<AuthSuccessResponseDTO>> IssueTokensAsync(User user, bool rememberMe);
}
