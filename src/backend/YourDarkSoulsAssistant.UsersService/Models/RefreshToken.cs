namespace YourDarkSoulsAssistant.UsersService.Models;

public class RefreshToken
{
    public int Id { get; init; }

    public required string Token { get; init; }
    
    public DateTime CreatedAt { get; init; }

    public DateTime ExpiresAt { get; init; }
    
    public bool IsRevoked { get; set; }
    
    public Guid UserId { get; init; }

    public User User { get; set; } = null!;
}
