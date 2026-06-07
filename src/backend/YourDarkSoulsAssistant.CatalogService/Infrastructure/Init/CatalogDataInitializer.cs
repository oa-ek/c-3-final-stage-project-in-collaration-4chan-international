using Microsoft.EntityFrameworkCore;
using YourDarkSoulsAssistant.CatalogService.Infrastructure.Context;
using YourDarkSoulsAssistant.Core.Infrastructure.Init;
using YourDarkSoulsAssistant.CatalogService.Models.Equipments;
using YourDarkSoulsAssistant.CatalogService.Models;

namespace YourDarkSoulsAssistant.CatalogService.Infrastructure.Init;

public class CatalogDataInitializer(
    CatalogDBContext context,
    ILogger<CatalogDataInitializer> logger,
    IWebHostEnvironment environment)
    : BaseDataInitializer<CatalogDBContext>(context, logger, environment)
{
    private sealed record SeedGame(string Name, string Code, string Icon);

    private sealed record SeedEquipment(string Name, float Weight, int Durability, string IconPath, string SlotName, string TypeName);

    protected override async Task SeedDataAsync()
    {
        if (await Context.Equipments.AnyAsync())
        {
            Logger.LogInformation("--> [Catalog Seeder]: Catalog already contains data. Skipping seed.");
            return;
        }

        Logger.LogInformation("--> [Catalog Seeder]: Seeding games and related catalog data...");

        var games = new List<Game>
        {
            new() { Name = "Dark Souls", Code = "DS1", Icon = "assets/games/ds1.png", IsActive = true },
            new() { Name = "Dark Souls II", Code = "DS2", Icon = "assets/games/ds2.png", IsActive = true },
            new() { Name = "Dark Souls III", Code = "DS3", Icon = "assets/games/ds3.png", IsActive = true },
            new() { Name = "Elden Ring", Code = "ER", Icon = "assets/games/er.png", IsActive = true }
        };

        Context.Games.AddRange(games);
        await Context.SaveChangesAsync();

        var gameByCode = games.ToDictionary(g => g.Code);

        var slotNames = new[] { "Right Hand 1", "Left Hand 1" };
        var slots = new List<Slot>();

        foreach (var game in games)
        {
            foreach (var slotName in slotNames)
            {
                slots.Add(new Slot
                {
                    Name = slotName,
                    GameId = game.GameId
                });
            }
        }

        Context.Slots.AddRange(slots);
        await Context.SaveChangesAsync();

        var equipmentTypeNames = new[] { "Straight Sword", "Greatsword", "Shield", "Staff", "Sacred Seal" };
        var equipmentTypes = new List<EquipmentType>();

        foreach (var game in games)
        {
            foreach (var typeName in equipmentTypeNames)
            {
                equipmentTypes.Add(new EquipmentType
                {
                    Id = Guid.NewGuid(),
                    Name = typeName,
                    GameId = game.GameId
                });
            }
        }

        Context.EquipmentTypes.AddRange(equipmentTypes);
        await Context.SaveChangesAsync();

        var slotByGameAndName = slots.ToDictionary(s => (s.GameId, s.Name));
        var typeByGameAndName = equipmentTypes.ToDictionary(t => (t.GameId, t.Name));

        var equipmentSeedByCode = new Dictionary<string, List<SeedEquipment>>
        {
            ["DS1"] =
            [
                new SeedEquipment("Longsword", 3.0f, 200, "assets/weapons/ds1-longsword.png", "Right Hand 1", "Straight Sword"),
                new SeedEquipment("Claymore", 6.0f, 200, "assets/weapons/ds1-claymore.png", "Right Hand 1", "Greatsword"),
                new SeedEquipment("Heater Shield", 2.0f, 100, "assets/shields/ds1-heater-shield.png", "Left Hand 1", "Shield")
            ],
            ["DS2"] =
            [
                new SeedEquipment("Broadsword", 3.0f, 70, "assets/weapons/ds2-broadsword.png", "Right Hand 1", "Straight Sword"),
                new SeedEquipment("Drangleic Sword", 6.0f, 70, "assets/weapons/ds2-drangleic-sword.png", "Right Hand 1", "Greatsword"),
                new SeedEquipment("Drangleic Shield", 4.5f, 70, "assets/shields/ds2-drangleic-shield.png", "Left Hand 1", "Shield")
            ],
            ["DS3"] =
            [
                new SeedEquipment("Lothric Knight Sword", 4.0f, 100, "assets/weapons/ds3-lothric-knight-sword.png", "Right Hand 1", "Straight Sword"),
                new SeedEquipment("Claymore", 9.0f, 100, "assets/weapons/ds3-claymore.png", "Right Hand 1", "Greatsword"),
                new SeedEquipment("Silver Eagle Kite Shield", 5.5f, 100, "assets/shields/ds3-silver-eagle-kite-shield.png", "Left Hand 1", "Shield")
            ],
            ["ER"] =
            [
                new SeedEquipment("Lordsworn's Straight Sword", 3.5f, 100, "assets/weapons/er-lordsworn-straight-sword.png", "Right Hand 1", "Straight Sword"),
                new SeedEquipment("Lordsworn's Greatsword", 9.0f, 100, "assets/weapons/er-lordsworn-greatsword.png", "Right Hand 1", "Greatsword"),
                new SeedEquipment("Brass Shield", 7.0f, 100, "assets/shields/er-brass-shield.png", "Left Hand 1", "Shield")
            ]
        };

        var equipments = new List<Equipment>();

        foreach (var (gameCode, equipmentItems) in equipmentSeedByCode)
        {
            var game = gameByCode[gameCode];

            foreach (var item in equipmentItems)
            {
                var slot = slotByGameAndName[(game.GameId, item.SlotName)];
                var type = typeByGameAndName[(game.GameId, item.TypeName)];

                equipments.Add(new Equipment
                {
                    Id = Guid.NewGuid(),
                    Name = item.Name,
                    Weight = item.Weight,
                    Durability = item.Durability,
                    IconPath = item.IconPath,
                    EquipmentTypeId = type.Id,
                    SlotId = slot.SlotId,
                    GameId = game.GameId,
                    RequiredAttributes = new List<ReqAttribute>(),
                    Scalings = new List<EquipmentScalingMap>(),
                    Influences = new List<EquipmentInfluenceMap>()
                });
            }
        }

        Context.Equipments.AddRange(equipments);
        await Context.SaveChangesAsync();

        Logger.LogInformation("--> [Catalog Seeder]: Seed completed for DS1, DS2, DS3 and ER.");
    }
}