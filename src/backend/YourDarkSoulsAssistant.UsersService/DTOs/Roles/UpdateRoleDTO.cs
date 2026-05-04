namespace YourDarkSoulsAssistant.UsersService.DTOs.Roles;

public record UpdateRoleDTO
{
    public Guid? Id { get; init; }
    
    public string? Name { get; init; }
    
    public string? Description { get; init; }
}
