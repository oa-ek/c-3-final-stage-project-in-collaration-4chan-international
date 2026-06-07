namespace YourDarkSoulsAssistant.BuildsService.Models;

public class AttributeType
{
    public int AttributeId { get; set; }
    public string Name { get; set; } = string.Empty;
    public int GameId { get; set; }

    public ICollection<CharacterAttribute> CharacterAttributes { get; set; } = new List<CharacterAttribute>();
}
