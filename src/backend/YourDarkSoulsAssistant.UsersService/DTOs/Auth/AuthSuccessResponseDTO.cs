namespace YourDarkSoulsAssistant.UsersService.DTOs.Auth;

public record AuthSuccessResponseDTO
{
    public required string AccessToken { get; init; }
    
    public required string RefreshToken { get; init; }
    
    public required string UserName { get; init; }
    
    public required IEnumerable<string> Role { get; init; }
}
