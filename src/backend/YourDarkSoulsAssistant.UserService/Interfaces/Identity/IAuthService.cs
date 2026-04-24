using YourDarkSoulsAssistant.UserService.DTOs.Auth;

namespace YourDarkSoulsAssistant.UserService.Interfaces.Identity;

public interface IAuthService
{
    Task<AuthResponseDTO> LoginAsync(LoginRequestDTO model);
    Task<AuthResponseDTO> RegisterAsync(RegisterRequestDTO model);
    Task<AuthResponseDTO> RefreshTokenAsync(RefreshTokenRequestDTO model);
}
