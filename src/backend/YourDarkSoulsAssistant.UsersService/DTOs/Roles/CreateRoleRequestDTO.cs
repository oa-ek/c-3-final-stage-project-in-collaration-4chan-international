namespace YourDarkSoulsAssistant.UsersService.DTOs.Roles;

public record CreateRoleRequestDTO
{
    public required string Name { get; init; }
    
    public required string Description { get; init; }
}
