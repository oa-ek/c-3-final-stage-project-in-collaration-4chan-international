using DarkSoulsBuildsAssistant.Core.DTOs.Base;

namespace DarkSoulsBuildsAssistant.Core.DTOs.System;

public record class LogDTO : BaseDTO
{
    public DateTime? CreatedAt { get; init; }
    public string? Message { get; init; }
    public string? UserId { get; init; }
    public UserDTO? User { get; init; }
    public int? LogLevelId { get; init; }
    public LogLevelDTO? LogLevel { get; init; }
}
