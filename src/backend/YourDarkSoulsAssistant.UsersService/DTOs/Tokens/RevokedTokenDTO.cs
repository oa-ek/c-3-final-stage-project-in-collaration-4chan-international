namespace YourDarkSoulsAssistant.UsersService.DTOs.Tokens;

public record RevokedTokenDTO
{
    public int? Id { get; init; }
    
    public string? TokenHash { get; init; }
    
    public DateTime? ExpirationDate { get; init; }
}
