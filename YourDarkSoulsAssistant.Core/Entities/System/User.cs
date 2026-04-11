using DarkSoulsBuildsAssistant.Core.Entities.Character;
using Microsoft.AspNetCore.Identity;

namespace DarkSoulsBuildsAssistant.Core.Entities.System;

public class User : IdentityUser<int>
{
    public string? FirstName { get; set; }
    
    public string? LastName { get; set; }
    
    public bool IsAdmin { get; set; }
    
    public virtual ICollection<CharacterBuild> CharacterBuilds { get; set; } = new List<CharacterBuild>();
    
    public virtual ICollection<Role> Roles { get; set; } = new List<Role>();
    
    public virtual ICollection<Log> Logs { get; set; } = new List<Log>();
    
    public virtual ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
}
