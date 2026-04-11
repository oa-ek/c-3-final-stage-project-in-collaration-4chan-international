using DarkSoulsBuildsAssistant.Core.DTOs.Base;
using DarkSoulsBuildsAssistant.Core.DTOs.System;

namespace DarkSoulsBuildsAssistant.Core.DTOs.System;

public record class UserDTO
{
    public string? Id { get; init; }
    public string? UserName { get; init; }
    public string? Email { get; init; }
    
    public string? FirstName { get; init; }
    
    public string? LastName { get; init; }
    
    public bool IsAdmin { get; init; }
    
    public virtual ICollection<string> Roles { get; init; } = new List<string>();
}
