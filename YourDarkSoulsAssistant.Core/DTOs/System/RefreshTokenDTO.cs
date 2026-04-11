using DarkSoulsBuildsAssistant.Core.DTOs.Base;

namespace DarkSoulsBuildsAssistant.Core.DTOs.System;

public record class RefreshTokenDTO : BaseDTO
{
    public string? Token { get; init; }
    public DateTime? Expires { get; init; }
    public bool IsRevoked { get; init; }
    public DateTime? CreatedAt { get; init; }
    public string? UserId { get; init; }
    public UserDTO? User { get; init; }
}
