using System.ComponentModel.DataAnnotations.Schema;
using DarkSoulsBuildsAssistant.Core.Entities.Base;

namespace DarkSoulsBuildsAssistant.Core.Entities.System;

public class Log: BaseEntity
{
    public DateTime? CreatedAt { get; set; }
    
    public string? Message { get; set; }
    
    public int? UserId { get; set; }
    
    public int? LogLevelId { get; set; }
    
    [ForeignKey(nameof(UserId))]
    public User? User { get; set; }
    
    [ForeignKey(nameof(LogLevelId))]
    public LogLevel? LogLevel { get; set; }
}
