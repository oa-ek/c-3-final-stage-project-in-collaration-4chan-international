using Microsoft.AspNetCore.Identity;

namespace YourDarkSoulsAssistant.UsersService.Models;

public class User : IdentityUser<Guid>
{
    public required string FirstName { get; set; }
    
    public required string LastName { get; set; }

    public string AvatarPath { get; set; } = string.Empty;
    
    public DateTime JoinDate { get; set; }
    
    public int Level { get; set; }
    
    public string Covenant { get; set; } = string.Empty;
    
    public bool IsBanned { get; set; }
    
    public bool IsActive { get; set; }
    
    public virtual ICollection<Role> Roles { get; set; } = new List<Role>();
    
    public virtual ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
}
