using YourDarkSoulsAssistant.UsersService.DTOs.Auth;

namespace YourDarkSoulsAssistant.UsersService.Interfaces.Identity;

public interface IAuthService
{
    Task<AuthResponseDTO> LoginAsync(LoginRequestDTO model);
    Task<AuthResponseDTO> RegisterAsync(RegisterRequestDTO model);
    Task<AuthResponseDTO> RefreshTokenAsync(RefreshTokenRequestDTO model);
}
