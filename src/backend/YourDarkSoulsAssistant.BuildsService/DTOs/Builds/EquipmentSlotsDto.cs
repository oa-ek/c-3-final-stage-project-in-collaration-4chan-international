namespace YourDarkSoulsAssistant.BuildsService.DTOs.Builds;

public class EquipmentSlotsDto
{
    public Guid?[] Weapons { get; set; } = [null, null, null];
    public Guid?[] Shields { get; set; } = [null, null, null];
    public Guid?[] Arrows { get; set; } = [null, null, null, null];
    public ArmorSlotsDto Armor { get; set; } = new();
    public Guid?[] Talismans { get; set; } = [null, null, null, null];
    public Guid?[] Consumables { get; set; } = [null, null, null, null, null, null, null, null, null, null];
}

public class ArmorSlotsDto
{
    public Guid? Head { get; set; }
    public Guid? Chest { get; set; }
    public Guid? Hands { get; set; }
    public Guid? Legs { get; set; }
}