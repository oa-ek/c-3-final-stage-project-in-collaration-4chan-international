using Microsoft.AspNetCore.Identity;

namespace YourDarkSoulsAssistant.UsersService.Models;

public class Role : IdentityRole<Guid>
{
    public string? Description { get; set; }
    
    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
