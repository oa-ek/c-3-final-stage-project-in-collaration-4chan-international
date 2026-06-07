namespace YourDarkSoulsAssistant.BuildsService.Models;

public class CharacterAttribute
{
    public Guid CharacterId { get; set; }
    public Character? Character { get; set; }

    public int AttributeId { get; set; }
    public AttributeType? AttributeType { get; set; }

    public int Value { get; set; }
}
