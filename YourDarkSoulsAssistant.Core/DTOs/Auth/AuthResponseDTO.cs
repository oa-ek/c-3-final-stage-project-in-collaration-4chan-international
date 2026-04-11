namespace DarkSoulsBuildsAssistant.Core.DTOs.Auth;

public record AuthResponseDTO
{
    public bool IsSuccess { get; init; }
    
    public string? AccessToken { get; init; }
    
    public string? RefreshToken { get; init; }
    
    public string? Token { get; init; }
    
    public string? ErrorMessage { get; init; }
    
    public string? UserName { get; init; }
    
    public string? Role { get; init; }
}

public record RefreshTokenRequestDTO(string AccessToken, string RefreshToken);
