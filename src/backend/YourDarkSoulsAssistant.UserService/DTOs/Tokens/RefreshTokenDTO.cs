using YourDarkSoulsAssistant.UserService.DTOs.Users;

namespace YourDarkSoulsAssistant.UserService.DTOs.Tokens;

public record class RefreshTokenDTO
{
    public int? Id { get; init; }
    
    public string? Token { get; init; }
    
    public DateTime? Expires { get; init; }
    
    public bool IsRevoked { get; init; }
    
    public DateTime? CreatedAt { get; init; }
    
    public string? UserId { get; init; }
    
    public UserDTO? User { get; init; }
}
