namespace YourDarkSoulsAssistant.UsersService.DTOs.Roles;

public record CreateRoleDTO
{
    public string? Name { get; init; }
    
    public string? Description { get; init; }
}
