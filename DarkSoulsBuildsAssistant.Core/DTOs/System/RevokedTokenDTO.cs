using DarkSoulsBuildsAssistant.Core.DTOs.Base;

namespace DarkSoulsBuildsAssistant.Core.DTOs.System;

public record class RevokedTokenDTO : BaseDTO
{
    public string? Token { get; init; }
    public DateTime? ExpirationDate { get; init; }
}
