using YourDarkSoulsAssistant.UsersService.DTOs.Users;

namespace YourDarkSoulsAssistant.UsersService.DTOs.Tokens;

public record RefreshTokenDTO
{
    public int? Id { get; init; }
    
    public DateTime? CreatedAt { get; init; }
    
    public DateTime? ExpiresAt { get; init; }
    
    public bool IsRevoked { get; init; }
    
    public string? UserId { get; init; }
    
    public UserResponseDTO? User { get; init; }
}
