using AutoMapper;
using Microsoft.EntityFrameworkCore;
using YourDarkSoulsAssistant.BuildsService.DTOs.Builds;
using YourDarkSoulsAssistant.BuildsService.Infrastructure.Context;
using YourDarkSoulsAssistant.BuildsService.Interfaces;
using YourDarkSoulsAssistant.BuildsService.Models;

namespace YourDarkSoulsAssistant.BuildsService.Services;

public class BuildsService(BuildsDbContext context, IMapper mapper) : IBuildsService
{
    private static readonly string[] AttributeKeys =
        ["vigor", "mind", "endurance", "strength", "dexterity", "intelligence", "faith", "arcane"];

    public async Task<IEnumerable<BuildResponseDto>> GetAllBuildsAsync(string? userId = null)
    {
        var query = context.Builds
            .Include(b => b.Character)
                .ThenInclude(c => c!.CharacterAttributes)
                    .ThenInclude(ca => ca.AttributeType)
            .Include(b => b.EquipmentBuilds)
            .AsNoTracking()
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(userId))
        {
            query = query.Where(x => x.Character != null && x.Character.UserId == userId);
        }

        var builds = await query
            .OrderBy(x => x.Name)
            .ToListAsync();

        return mapper.Map<List<BuildResponseDto>>(builds);
    }

    public async Task<BuildResponseDto> CreateBuildAsync(CreateBuildRequestDto request)
    {
        var characterId = Guid.NewGuid();
        var buildId = Guid.NewGuid();

        var character = new Character
        {
            CharacterId = characterId,
            CharacterName = string.IsNullOrWhiteSpace(request.CharacterName) ? request.Name : request.CharacterName.Trim(),
            Level = request.Stats.Level,
            UserId = request.UserId.Trim()
        };

        var build = new Build
        {
            SetId = buildId,
            Name = request.Name.Trim(),
            CharacterId = characterId
        };

        var attributeTypes = await GetAttributeTypesByKeyAsync();
        var attributes = CreateCharacterAttributes(characterId, request.Stats, attributeTypes);
        var equipmentBuilds = CreateEquipmentBuilds(buildId, request.Equipment);

        await context.Characters.AddAsync(character);
        await context.Builds.AddAsync(build);
        await context.CharacterAttributes.AddRangeAsync(attributes);
        await context.EquipmentBuilds.AddRangeAsync(equipmentBuilds);

        await context.SaveChangesAsync();

        build.Character = character;
        build.Character.CharacterAttributes = attributes;
        build.EquipmentBuilds = equipmentBuilds;

        return mapper.Map<BuildResponseDto>(build);
    }

    public async Task<BuildResponseDto?> UpdateBuildAsync(Guid buildId, UpdateBuildRequestDto request)
    {
        var build = await context.Builds
            .Include(x => x.Character)
                .ThenInclude(x => x!.CharacterAttributes)
                    .ThenInclude(x => x.AttributeType)
            .Include(x => x.EquipmentBuilds)
            .FirstOrDefaultAsync(x => x.SetId == buildId);

        if (build is null || build.Character is null)
        {
            return null;
        }

        if (!string.IsNullOrWhiteSpace(request.UserId) &&
            !string.Equals(build.Character.UserId, request.UserId, StringComparison.Ordinal))
        {
            return null;
        }

        build.Name = request.Name.Trim();
        build.Character.Level = request.Stats.Level;

        var attributeTypes = await GetAttributeTypesByKeyAsync();
        UpsertCharacterAttributes(build.Character, request.Stats, attributeTypes);

        context.EquipmentBuilds.RemoveRange(build.EquipmentBuilds);
        var equipmentBuilds = CreateEquipmentBuilds(build.SetId, request.Equipment);
        await context.EquipmentBuilds.AddRangeAsync(equipmentBuilds);

        await context.SaveChangesAsync();

        build.EquipmentBuilds = equipmentBuilds;
        return mapper.Map<BuildResponseDto>(build);
    }

    private async Task<Dictionary<string, AttributeType>> GetAttributeTypesByKeyAsync()
    {
        return await context.AttributeTypes
            .Where(x => AttributeKeys.Contains(x.Name.ToLower()))
            .ToDictionaryAsync(x => NormalizeKey(x.Name)!, x => x, StringComparer.OrdinalIgnoreCase);
    }

    private static List<CharacterAttribute> CreateCharacterAttributes(
        Guid characterId,
        CharacterStatsDto stats,
        IReadOnlyDictionary<string, AttributeType> attributeTypes)
    {
        var attributes = new List<CharacterAttribute>();

        AddAttribute(attributes, characterId, "vigor", stats.Vigor, attributeTypes);
        AddAttribute(attributes, characterId, "mind", stats.Mind, attributeTypes);
        AddAttribute(attributes, characterId, "endurance", stats.Endurance, attributeTypes);
        AddAttribute(attributes, characterId, "strength", stats.Strength, attributeTypes);
        AddAttribute(attributes, characterId, "dexterity", stats.Dexterity, attributeTypes);
        AddAttribute(attributes, characterId, "intelligence", stats.Intelligence, attributeTypes);
        AddAttribute(attributes, characterId, "faith", stats.Faith, attributeTypes);
        AddAttribute(attributes, characterId, "arcane", stats.Arcane, attributeTypes);

        return attributes;
    }

    private static void UpsertCharacterAttributes(
        Character character,
        CharacterStatsDto stats,
        IReadOnlyDictionary<string, AttributeType> attributeTypes)
    {
        var existing = character.CharacterAttributes
            .Where(x => x.AttributeType is not null)
            .ToDictionary(x => NormalizeKey(x.AttributeType!.Name)!, x => x, StringComparer.OrdinalIgnoreCase);

        UpsertAttribute(character, existing, "vigor", stats.Vigor, attributeTypes);
        UpsertAttribute(character, existing, "mind", stats.Mind, attributeTypes);
        UpsertAttribute(character, existing, "endurance", stats.Endurance, attributeTypes);
        UpsertAttribute(character, existing, "strength", stats.Strength, attributeTypes);
        UpsertAttribute(character, existing, "dexterity", stats.Dexterity, attributeTypes);
        UpsertAttribute(character, existing, "intelligence", stats.Intelligence, attributeTypes);
        UpsertAttribute(character, existing, "faith", stats.Faith, attributeTypes);
        UpsertAttribute(character, existing, "arcane", stats.Arcane, attributeTypes);
    }

    private static void UpsertAttribute(
        Character character,
        IDictionary<string, CharacterAttribute> existing,
        string key,
        int value,
        IReadOnlyDictionary<string, AttributeType> attributeTypes)
    {
        if (!attributeTypes.TryGetValue(key, out var attributeType))
        {
            return;
        }

        if (existing.TryGetValue(key, out var attribute))
        {
            attribute.Value = value;
            return;
        }

        character.CharacterAttributes.Add(new CharacterAttribute
        {
            CharacterId = character.CharacterId,
            AttributeId = attributeType.AttributeId,
            Value = value
        });
    }

    private static void AddAttribute(
        ICollection<CharacterAttribute> target,
        Guid characterId,
        string key,
        int value,
        IReadOnlyDictionary<string, AttributeType> attributeTypes)
    {
        if (!attributeTypes.TryGetValue(key, out var attributeType))
        {
            return;
        }

        target.Add(new CharacterAttribute
        {
            CharacterId = characterId,
            AttributeId = attributeType.AttributeId,
            Value = value
        });
    }

    private static List<EquipmentBuild> CreateEquipmentBuilds(Guid buildId, EquipmentSlotsDto equipment)
    {
        var ids = new List<Guid?>();
        ids.AddRange(equipment.Weapons);
        ids.AddRange(equipment.Shields);
        ids.AddRange(equipment.Arrows);
        ids.Add(equipment.Armor.Head);
        ids.Add(equipment.Armor.Chest);
        ids.Add(equipment.Armor.Hands);
        ids.Add(equipment.Armor.Legs);
        ids.AddRange(equipment.Talismans);
        ids.AddRange(equipment.Consumables);

        return ids
            .Where(x => x.HasValue)
            .Select(x => new EquipmentBuild
            {
                SetId = buildId,
                EquipmentId = x!.Value
            })
            .ToList();
    }

    private static string? NormalizeKey(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return null;
        }

        return value.Trim().ToLowerInvariant();
    }
}
