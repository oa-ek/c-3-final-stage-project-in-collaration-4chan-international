namespace YourDarkSoulsAssistant.UsersService.DTOs.Users;

public record CreateUserDTO
{
    public string? FirstName { get; init; }
    
    public string? LastName { get; init; }
    
    public string? UserName { get; init; }
    
    public string? Email { get; init; }
    
    public string? Password { get; init; }
    
    public string? ConfirmPassword { get; init; }
}
