namespace DarkSoulsBuildsAssistant.Core.DTOs.Base;

public abstract record NamedDTO : BaseDTO
{
    public string? Name { get; init; }
}
