namespace YourDarkSoulsAssistant.UsersService.DTOs.Users;

public record UpdateUserDTO
{
    public Guid? Id { get; init; }
    
    public string? FirstName { get; init; }
    
    public string? LastName { get; init; }
    
    public string? UserName { get; init; }
    
    public string? Email { get; init; }
    
    public string? AvatarPath { get; init; }
    
    public string? Covenant { get; init; }
}
