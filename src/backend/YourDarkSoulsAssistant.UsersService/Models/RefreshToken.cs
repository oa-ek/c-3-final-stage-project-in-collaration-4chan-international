using System.ComponentModel.DataAnnotations.Schema;

namespace YourDarkSoulsAssistant.UsersService.Models;

public class RefreshToken
{
    public int Id { get; init; }

    public required string Token { get; init; }
    
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;

    public DateTime ExpiresAt { get; init; }
    
    public bool IsRevoked { get; set; }
    
    public Guid UserId { get; set; }
    
    [ForeignKey(nameof(UserId))]
    public User User { get; set; }
}