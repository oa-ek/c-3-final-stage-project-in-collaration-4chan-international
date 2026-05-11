namespace YourDarkSoulsAssistant.UsersService.DTOs.Auth;

public record RefreshTokenRequestDTO
{
    public required string RefreshToken { get; init; }
}
