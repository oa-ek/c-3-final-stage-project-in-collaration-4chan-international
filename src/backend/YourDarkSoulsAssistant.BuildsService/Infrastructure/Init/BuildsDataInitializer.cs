using Microsoft.EntityFrameworkCore;
using YourDarkSoulsAssistant.BuildsService.Infrastructure.Context;
using YourDarkSoulsAssistant.BuildsService.Models;
using YourDarkSoulsAssistant.Core.Infrastructure.Init;

namespace YourDarkSoulsAssistant.BuildsService.Infrastructure.Init;

public class BuildsDataInitializer(
    BuildsDbContext context,
    ILogger<BuildsDataInitializer> logger,
    IWebHostEnvironment environment)
    : BaseDataInitializer<BuildsDbContext>(context, logger, environment)
{
    protected override async Task SeedDataAsync()
    {
        if (await Context.Characters.AnyAsync())
        {
            Logger.LogInformation("--> [Builds Seeder]: Builds database already contains data. Skipping seed.");
            return;
        }

        var characterId = Guid.Parse("11111111-1111-1111-1111-111111111111");
        var setId = Guid.Parse("22222222-2222-2222-2222-222222222222");
        var weaponId = Guid.Parse("33333333-3333-3333-3333-333333333333");
        var shieldId = Guid.Parse("44444444-4444-4444-4444-444444444444");

        var character = new Character
        {
            CharacterId = characterId,
            CharacterName = "Chosen Undead",
            Level = 42,
            UserId = "seed-user"
        };

        var vitality = new AttributeType
        {
            AttributeId = 1,
            Name = "Vitality",
            GameId = 1
        };

        var endurance = new AttributeType
        {
            AttributeId = 2,
            Name = "Endurance",
            GameId = 1
        };

        var build = new Build
        {
            SetId = setId,
            CharacterId = characterId,
            Name = "Knight Starter"
        };

        Context.Characters.Add(character);
        Context.AttributeTypes.AddRange(vitality, endurance);
        Context.Builds.Add(build);
        Context.CharacterAttributes.AddRange(
            new CharacterAttribute { CharacterId = characterId, AttributeId = 1, Value = 20 },
            new CharacterAttribute { CharacterId = characterId, AttributeId = 2, Value = 18 });
        Context.EquipmentBuilds.AddRange(
            new EquipmentBuild { EquipmentId = weaponId, SetId = setId },
            new EquipmentBuild { EquipmentId = shieldId, SetId = setId });

        await Context.SaveChangesAsync();

        Logger.LogInformation("--> [Builds Seeder]: Seed completed.");
    }
}
