using DarkSoulsBuildsAssistant.Core.DTOs.User;

namespace DarkSoulsBuildsAssistant.Core.DTOs.System;

public record LogDTO
{
    public DateTime? CreatedAt { get; init; }
    
    public string? Message { get; init; }
    
    public UserDTO? User { get; init; }
    
    public LogLevelDTO? LogLevel { get; init; }
}
