using DarkSoulsBuildsAssistant.Core.DTOs.Base;

namespace DarkSoulsBuildsAssistant.Core.DTOs.System;

public record class RoleDTO : NamedDTO
{
    public string? Description { get; init; }
}
