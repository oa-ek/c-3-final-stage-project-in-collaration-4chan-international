using DarkSoulsBuildsAssistant.Core.Entities.Base;

namespace DarkSoulsBuildsAssistant.Core.Entities.System;

public class Role : NamedEntity
{
    public string? Description { get; set; }
    
    public virtual ICollection<User> Users { get; set; } = new List<User>();
}