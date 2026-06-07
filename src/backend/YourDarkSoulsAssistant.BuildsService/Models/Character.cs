namespace YourDarkSoulsAssistant.BuildsService.Models;

public class Character
{
    public Guid CharacterId { get; set; }
    public string CharacterName { get; set; } = string.Empty;
    public int Level { get; set; }
    public string UserId { get; set; } = string.Empty;

    public ICollection<Build> Builds { get; set; } = new List<Build>();
    public ICollection<CharacterAttribute> CharacterAttributes { get; set; } = new List<CharacterAttribute>();
}
