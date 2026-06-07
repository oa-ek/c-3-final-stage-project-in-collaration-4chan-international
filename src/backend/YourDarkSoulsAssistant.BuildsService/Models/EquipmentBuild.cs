namespace YourDarkSoulsAssistant.BuildsService.Models;

public class EquipmentBuild
{
    public Guid EquipmentId { get; set; }
    public Guid SetId { get; set; }

    public Build? Build { get; set; }
}
