namespace YourDarkSoulsAssistant.UserService.DTOs.Users;

public record ChangeRoleDTO(Guid Id, IEnumerable<string> Roles);