namespace YourDarkSoulsAssistant.UserService.Models;

public class RevokedToken
{
    public int Id { get; set; }
    
    public string Token { get; set; }
    
    public DateTime ExpirationDate { get; set; }
    
}