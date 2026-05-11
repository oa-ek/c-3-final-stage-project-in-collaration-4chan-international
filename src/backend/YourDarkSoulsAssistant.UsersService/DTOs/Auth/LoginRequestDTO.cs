namespace YourDarkSoulsAssistant.UsersService.DTOs.Auth;

public record LoginRequestDTO
{
    public required string Login { get; init; }
    
    public required string Password { get; init; }
    
    public bool RememberMe { get; init; }
    
    public bool IsEmail => Login.Contains('@');
}
