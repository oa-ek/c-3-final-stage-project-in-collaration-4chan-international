namespace DarkSoulsBuildsAssistant.Core.DTOs.Auth;

public record RegisterRequestDTO
{
    public string? Email { get; init; }
    
    public string? Password { get; init; }
    
    public string? ConfirmPassword { get; init; }
    
    public string? FirstName { get; init; }
    
    public string? LastName { get; init; }
    
    public string? UserName { get; init; }
}
