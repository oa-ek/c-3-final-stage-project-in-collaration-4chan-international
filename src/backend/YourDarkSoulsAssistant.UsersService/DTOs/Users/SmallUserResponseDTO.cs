namespace YourDarkSoulsAssistant.UsersService.DTOs.Users;

public record SmallUserResponseDTO
{
    public required Guid Id { get; init; }
    
    public required string FirstName { get; init; }
    
    public required string LastName { get; init; }
    
    public required string UserName { get; init; }
    
    public required string Email { get; init; }
    
    public required List<string> Roles { get; init; }
}
