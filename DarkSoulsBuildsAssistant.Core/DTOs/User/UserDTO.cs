namespace DarkSoulsBuildsAssistant.Core.DTOs.User;

public class UserDTO
{
    public string UserName { get; set; }
    
    public string Email { get; set; }
    
    public RoleDTO Role { get; set; }
    
    public string AvatarUrl { get; set; }
}
