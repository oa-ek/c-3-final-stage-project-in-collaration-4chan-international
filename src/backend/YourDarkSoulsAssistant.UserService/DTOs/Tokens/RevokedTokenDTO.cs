namespace YourDarkSoulsAssistant.UserService.DTOs.Tokens;

public record RevokedTokenDTO
{
    public int? Id { get; init; }
    
    public string? Token { get; init; }
    
    public DateTime? ExpirationDate { get; init; }
}
