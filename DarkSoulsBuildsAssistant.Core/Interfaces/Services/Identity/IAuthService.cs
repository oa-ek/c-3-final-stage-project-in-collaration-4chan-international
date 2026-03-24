using Shared.DTO.Auth;

// Твої DTO

namespace Services.Interfaces;

public interface IAuthService
{
    Task<AuthResponseDto> LoginAsync(LoginRequestDto model);
    Task<AuthResponseDto> RegisterAsync(RegisterRequestDto model);
    Task<AuthResponseDto> RefreshTokenAsync(RefreshTokenRequestDto model);
}