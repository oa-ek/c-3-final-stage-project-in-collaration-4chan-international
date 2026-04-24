using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace YourDarkSoulsAssistant.UserService.Models;

public class User : IdentityUser<Guid>
{
    public required string FirstName { get; set; }
    
    public required string LastName { get; set; }
    
    public override required string UserName { get; set; }

    public override required string Email { get; set; }

    public string? AvatarPath { get; set; }
    
    public required DateTime JoinDate { get; set; }
    
    public bool IsAdmin { get; set; }
    
    public int Level { get; set; }
    
    public string? Covenant { get; set; }
    
    public virtual ICollection<Role> Roles { get; set; } = new List<Role>();
    
    public virtual ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
}
