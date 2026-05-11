namespace YourDarkSoulsAssistant.UsersService.DTOs.Users;

public record ChangeUserRolesRequestDTO
{
    public required Guid Id { get; init; }
    
    public required IEnumerable<string> Roles { get; init; }
}
