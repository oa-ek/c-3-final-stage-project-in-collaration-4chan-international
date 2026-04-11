using DarkSoulsBuildsAssistant.Core.DTOs.Base;
using DarkSoulsBuildsAssistant.Core.DTOs.Equipment;

namespace DarkSoulsBuildsAssistant.Core.DTOs.Character;

public record SetDTO : NamedDTO
{
    public CharacterBuildDTO? CharacterBuild { get; init; }

    public List<SlotViewDTO> EquippedSlots { get; set; } = new();
}
