namespace YourDarkSoulsAssistant.UsersService.DTOs.Roles;

public record UpdateRoleRequestDTO
{
    public required Guid Id { get; init; }
    
    public string? Name { get; init; }
    
    public string? Description { get; init; }
}
