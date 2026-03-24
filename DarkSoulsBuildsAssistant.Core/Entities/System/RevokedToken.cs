using DarkSoulsBuildsAssistant.Core.Entities.Base;

namespace DarkSoulsBuildsAssistant.Core.Entities.System;

public class RevokedToken: BaseEntity
{
    public string? Token { get; set; }

    public DateTime? ExpirationDate { get; set; }
}