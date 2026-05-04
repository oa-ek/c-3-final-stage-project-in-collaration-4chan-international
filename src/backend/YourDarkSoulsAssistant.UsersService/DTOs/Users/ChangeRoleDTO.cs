namespace YourDarkSoulsAssistant.UsersService.DTOs.Users;

public record ChangeRoleDTO(Guid? Id, IEnumerable<string>? Roles);