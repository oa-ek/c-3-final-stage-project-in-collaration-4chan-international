using DarkSoulsBuildsAssistant.Core.Entities.Base;
using Microsoft.AspNetCore.Identity;

namespace DarkSoulsBuildsAssistant.Core.Entities.System;

public class Role : IdentityRole<int>
{
    public string? Description { get; set; }
    
    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
