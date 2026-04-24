using YourDarkSoulsAssistant.UserService.DTOs.Roles;

namespace YourDarkSoulsAssistant.UserService.DTOs.Users;

public record UserDTO
{
    public required string Id { get; init; }
    
    public required string FirstName { get; init; }
    
    public required string LastName { get; init; }
    
    public required string UserName { get; init; }
    
    public required string Email { get; init; }
    
    public required int BuildCounts { get; init; }
    
    public required string AvatarPath { get; init; }
    
    public required DateTime JoinDate { get; init; }
    
    public required List<RoleDTO> Roles { get; init; }
    
    public required bool IsAdmin { get; init; }
    
    public required int Level { get; init; }
    
    public required string Covenant { get; init; }
}
