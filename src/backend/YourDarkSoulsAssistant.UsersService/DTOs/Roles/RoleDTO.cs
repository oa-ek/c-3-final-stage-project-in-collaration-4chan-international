namespace YourDarkSoulsAssistant.UsersService.DTOs.Roles;

public record RoleDTO
{
    public required Guid Id { get; init; }
    
    public required string Name { get; init; }
    
    public required string NormalizedName { get; init; }
    
    public required string Description { get; init; }
}
