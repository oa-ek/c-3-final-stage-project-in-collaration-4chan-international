using DarkSoulsBuildsAssistant.Core.DTOs.Auth;

namespace DarkSoulsBuildsAssistant.Core.Interfaces.Services.Identity;

public interface IAuthService
{
    Task<AuthResponseDTO> LoginAsync(LoginRequestDTO model);
    Task<AuthResponseDTO> RegisterAsync(RegisterRequestDTO model);
    Task<AuthResponseDTO> RefreshTokenAsync(RefreshTokenRequestDTO model);
}
