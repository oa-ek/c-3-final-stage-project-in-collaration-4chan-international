using System.ComponentModel.DataAnnotations.Schema;
using DarkSoulsBuildsAssistant.Core.Entities.Base;

namespace DarkSoulsBuildsAssistant.Core.Entities.System;

public class RefreshToken : BaseEntity
{
    public string? Token { get; set; }

    public DateTime? Expires { get; set; }
    public bool IsRevoked { get; set; }
    public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;


    public int UserId { get; set; }
    
    [ForeignKey(nameof(UserId))]
    public User? User { get; set; }
}
