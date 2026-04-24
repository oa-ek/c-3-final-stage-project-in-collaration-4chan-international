namespace YourDarkSoulsAssistant.UserService.DTOs.Roles;

public record UpdateRoleDTO
{
    public string? Id { get; init; }
    
    public string? Name { get; init; }
    
    public string? Description { get; init; }
}
