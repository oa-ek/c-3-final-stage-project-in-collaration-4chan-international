using DarkSoulsBuildsAssistant.Core.Entities.Equipment;
using DarkSoulsBuildsAssistant.Core.Entities.Equipment.Armor;
using DarkSoulsBuildsAssistant.Core.Entities.Equipment.Weapon;
using DarkSoulsBuildsAssistant.Core.Entities.System;
using DarkSoulsBuildsAssistant.Infrastructure.Context;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DarkSoulsBuildsAssistant.Infrastructure.Init;

public static class DataInitializer
{
    public static async Task InitializeAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<BuildsAssistantDbContext>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<Role>>();

        await context.Database.MigrateAsync();

        if (!await context.Games.AnyAsync())
        {
            context.Games.AddRange(
                new Game { Name = "Dark Souls" },
                new Game { Name = "Dark Souls II" },
                new Game { Name = "Dark Souls III" },
                new Game { Name = "Elden Ring" }
            );
            await context.SaveChangesAsync();
        }

        if (!await context.LogLevels.AnyAsync())
        {
            context.LogLevels.AddRange(
                new LogLevel { Name = "Info", Description = "Informational message" },
                new LogLevel { Name = "Warning", Description = "Warning message" },
                new LogLevel { Name = "Error", Description = "Error message" },
                new LogLevel { Name = "Critical", Description = "Critical message" }
            );
            await context.SaveChangesAsync();
        }

        if (!await roleManager.RoleExistsAsync("Admin"))
        {
            await roleManager.CreateAsync(new Role { Name = "Admin", Description = "Administrator role" });
        }
        if (!await roleManager.RoleExistsAsync("User"))
        {
            await roleManager.CreateAsync(new Role { Name = "User", Description = "Regular user role" });
        }

        if (await userManager.FindByNameAsync("admin") == null)
        {
            var adminUser = new User 
            { 
                UserName = "admin", 
                Email = "admin@example.com", 
                FirstName = "Admin", 
                LastName = "User", 
                IsAdmin = true 
            };
            var result = await userManager.CreateAsync(adminUser, "Admin123!");
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(adminUser, "Admin");
            }
        }

        if (await userManager.FindByNameAsync("user") == null)
        {
            var regularUser = new User 
            { 
                UserName = "user", 
                Email = "user@example.com", 
                FirstName = "Regular", 
                LastName = "User", 
                IsAdmin = false 
            };
            var result = await userManager.CreateAsync(regularUser, "User123!");
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(regularUser, "User");
            }
        }
        
        if (!await context.Slots.AnyAsync())
        {
            var slotHead = new Slot { Name = "Head" };
            var slotChest = new Slot { Name = "Chest" };
            var slotHands = new Slot { Name = "Hands" };
            var slotLegs = new Slot { Name = "Legs" };
            var slotRHand = new Slot { Name = "RightHand" };
            var slotLHand = new Slot { Name = "LeftHand" };

            context.Slots.AddRange(slotHead, slotChest, slotHands, slotLegs, slotRHand, slotLHand);
            await context.SaveChangesAsync();
        }
        
        if (!await context.InfluenceTypes.AnyAsync())
        {
            var infPhys = new InfluenceType { Name = "Physical" };
            var infMagic = new InfluenceType { Name = "Magic" };
            var infFire = new InfluenceType { Name = "Fire" };
            var infLight = new InfluenceType { Name = "Lightning" };
            var infCritical = new InfluenceType { Name = "Critical" };

            context.InfluenceTypes.AddRange(infPhys, infMagic, infFire, infLight, infCritical);
            await context.SaveChangesAsync();
        }
        
        if (!await context.EquipmentTypes.AnyAsync())
        {
            var slotRHandId = (await context.Slots.FirstAsync(s => s.Name == "RightHand")).Id;
            var slotLHandId = (await context.Slots.FirstAsync(s => s.Name == "LeftHand")).Id;

            var typeStraightSword = new WeaponType { Name = "Straight sword", SlotId = slotRHandId };
            var typeHalberd = new WeaponType { Name = "Halberd", SlotId = slotRHandId };
            var typeSpear = new WeaponType { Name = "Spear", SlotId = slotRHandId };
            var typeGreatAxe = new WeaponType { Name = "Great Axe", SlotId = slotRHandId };
            var typeGreatSword = new WeaponType { Name = "Greatsword", SlotId = slotRHandId };
            var typeHuge = new WeaponType { Name = "Huge sword", SlotId = slotRHandId };
            var typeKatana = new WeaponType { Name = "Katana", SlotId = slotRHandId };
            var typeShield = new WeaponType { Name = "Shield", SlotId = slotLHandId };
            var typeDagger = new WeaponType { Name = "Dagger", SlotId = slotRHandId };
            var typeCurvedSword = new WeaponType { Name = "Curved Sword", SlotId = slotRHandId };
            var typeThrusting = new WeaponType { Name = "Thrusting Sword", SlotId = slotRHandId };
            var typeAxe = new WeaponType { Name = "Axe", SlotId = slotRHandId };
            var typeHammer = new WeaponType { Name = "Hammer", SlotId = slotRHandId };
            var typeGreatHammer = new WeaponType { Name = "Great Hammer", SlotId = slotRHandId };
            var typeCurvedGreat = new WeaponType { Name = "Curved Greatsword", SlotId = slotRHandId };

            context.EquipmentTypes.AddRange(
                typeStraightSword, typeHalberd, typeSpear, typeGreatAxe, typeGreatSword, typeHuge, typeKatana, typeShield,
                typeDagger, typeCurvedSword, typeThrusting, typeAxe, typeHammer, typeGreatHammer, typeCurvedGreat
            );
            await context.SaveChangesAsync();

            var slotHeadId = (await context.Slots.FirstAsync(s => s.Name == "Head")).Id;
            var slotChestId = (await context.Slots.FirstAsync(s => s.Name == "Chest")).Id;
            var slotHandsId = (await context.Slots.FirstAsync(s => s.Name == "Hands")).Id;
            var slotLegsId = (await context.Slots.FirstAsync(s => s.Name == "Legs")).Id;

            var typeHelm = new ArmorType { Name = "Helm", SlotId = slotHeadId };
            var typeChestArmor = new ArmorType { Name = "Chest ArmorEquipment", SlotId = slotChestId };
            var typeGauntlets = new ArmorType { Name = "Gauntlets", SlotId = slotHandsId };
            var typeLeggings = new ArmorType { Name = "Leggings", SlotId = slotLegsId };

            context.EquipmentTypes.AddRange(typeHelm, typeChestArmor, typeGauntlets, typeLeggings);
            await context.SaveChangesAsync();
        }
        
        if (!await context.Equipments.AnyAsync())
        {
            var typeHugeId = (await context.EquipmentTypes.FirstAsync(wt => wt.Name == "Huge sword")).Id;
            var typeGreatSwordId = (await context.EquipmentTypes.FirstAsync(wt => wt.Name == "Greatsword")).Id;
            var typeStraightSwordId = (await context.EquipmentTypes.FirstAsync(wt => wt.Name == "Straight sword")).Id;
            var typeHalberdId = (await context.EquipmentTypes.FirstAsync(wt => wt.Name == "Halberd")).Id;
            var typeSpearId = (await context.EquipmentTypes.FirstAsync(wt => wt.Name == "Spear")).Id;
            var typeKatanaId = (await context.EquipmentTypes.FirstAsync(wt => wt.Name == "Katana")).Id;
            var typeShieldId = (await context.EquipmentTypes.FirstAsync(wt => wt.Name == "Shield")).Id;
            var typeGreatAxeId = (await context.EquipmentTypes.FirstAsync(wt => wt.Name == "Great Axe")).Id;
            var typeDaggerId = (await context.EquipmentTypes.FirstAsync(wt => wt.Name == "Dagger")).Id;
            var typeCurvedSwordId = (await context.EquipmentTypes.FirstAsync(wt => wt.Name == "Curved Sword")).Id;
            var typeThrustingId = (await context.EquipmentTypes.FirstAsync(wt => wt.Name == "Thrusting Sword")).Id;
            var typeAxeId = (await context.EquipmentTypes.FirstAsync(wt => wt.Name == "Axe")).Id;
            var typeHammerId = (await context.EquipmentTypes.FirstAsync(wt => wt.Name == "Hammer")).Id;
            var typeGreatHammerId = (await context.EquipmentTypes.FirstAsync(wt => wt.Name == "Great Hammer")).Id;
            var typeCurvedGreatId = (await context.EquipmentTypes.FirstAsync(wt => wt.Name == "Curved Greatsword")).Id;

            var infPhysId = (await context.InfluenceTypes.FirstAsync(it => it.Name == "Physical")).Id;
            var infMagicId = (await context.InfluenceTypes.FirstAsync(it => it.Name == "Magic")).Id;
            var infFireId = (await context.InfluenceTypes.FirstAsync(it => it.Name == "Fire")).Id;
            var infLightId = (await context.InfluenceTypes.FirstAsync(it => it.Name == "Lightning")).Id;
            var infCriticalId = (await context.InfluenceTypes.FirstAsync(it => it.Name == "Critical")).Id;

            var weapons = new List<WeaponEquipment>
            {
                new()
                {
                    Name = "Black Knight's Greatsword",
                    Weight = 14.0m,
                    EquipmentTypeId = typeHugeId,
                    ReqStrength = 32,
                    ReqDexterity = 18,
                    IconPath = "/images/Weapons/Black Knight's Greatsword.jpg",
                    WeaponInfluences = new List<WeaponInfluence> 
                    { 
                        new() { InfluenceTypeId = infPhysId, Value = 220 }, 
                        new() { InfluenceTypeId = infCriticalId, Value = 100 }
                    }
                },
                new()
                {
                    Name = "Greatsword of Artorias",
                    Weight = 10.0m,
                    EquipmentTypeId = typeGreatSwordId,
                    ReqStrength = 24,
                    ReqDexterity = 18,
                    ReqIntelligence = 18,
                    ReqFaith = 18,
                    IconPath = "/images/Weapons/Greatsword of Artorias.jpg", 
                    WeaponInfluences = new List<WeaponInfluence> 
                    {
                        new() { InfluenceTypeId = infPhysId, Value = 178 }, 
                        new() { InfluenceTypeId = infCriticalId, Value = 100 }
                    }
                },
                new()
                {
                    Name="Dark Sword",
                    Weight=6.0m,
                    EquipmentTypeId = typeStraightSwordId,
                    ReqStrength=16,
                    ReqDexterity=16,
                    IconPath="/images/Weapons/Dark Sword.png",
                    WeaponInfluences= new List<WeaponInfluence>
                    {
                        new() { InfluenceTypeId=infPhysId, Value=82 },
                        new() { InfluenceTypeId = infCriticalId, Value = 100 }
                    }
                },
                new()
                {
                    Name="Astor Straight Sword",
                    Weight=3.0m,
                    EquipmentTypeId = typeStraightSwordId,
                    ReqStrength=10,
                    ReqDexterity=10,
                    ReqFaith=14,
                    IconPath="/images/Weapons/Astor Straight Sword.png",
                    WeaponInfluences= new List<WeaponInfluence>
                    {
                        new() { InfluenceTypeId=infPhysId, Value=80 }, 
                        new() { InfluenceTypeId = infMagicId, Value = 80 }, 
                        new() { InfluenceTypeId = infCriticalId, Value = 100 }
                    }
                },
                new()
                {
                    Name="Black Knight's Halberd",
                    Weight=14.0m,
                    EquipmentTypeId = typeHalberdId,
                    ReqStrength=32,
                    ReqDexterity=18,
                    IconPath="/images/Weapons/Black Knight's Halberd.jpg",
                    WeaponInfluences= new List<WeaponInfluence>
                    {
                        new() { InfluenceTypeId=infPhysId, Value=245 }, 
                        new() { InfluenceTypeId = infMagicId, Value = 80 }, 
                        new() { InfluenceTypeId = infCriticalId, Value = 100 }
                    }
                },
                new()
                {
                    Name = "Winged Spear",
                    Weight = 4.5m,
                    EquipmentTypeId = typeSpearId,
                    ReqStrength = 13,
                    ReqDexterity = 15,
                    IconPath = "/images/Weapons/Winged Spear.png",
                    WeaponInfluences = new List<WeaponInfluence>
                    {
                        new() { InfluenceTypeId = infPhysId, Value = 86 },
                        new() { InfluenceTypeId = infMagicId, Value = 80 }, 
                        new() { InfluenceTypeId = infCriticalId, Value = 100 }
                    }
                },
                new()
                {
                    Name = "Claymor",
                    Weight = 9.0m,
                    EquipmentTypeId = typeGreatSwordId,
                    ReqStrength = 16,
                    ReqDexterity = 13,
                    IconPath = "/images/Weapons/claymor.png",
                    WeaponInfluences = new List<WeaponInfluence>
                    {
                        new() { InfluenceTypeId = infPhysId, Value = 138 }, 
                        new() { InfluenceTypeId = infCriticalId, Value = 100 }
                    }
                },
                new()
                {
                    Name = "Zweihander",
                    Weight = 10.0m,
                    EquipmentTypeId = typeHugeId,
                    ReqStrength = 24,
                    ReqDexterity = 10,
                    IconPath = "/images/Weapons/Zweihander.png",
                    WeaponInfluences = new List<WeaponInfluence>
                    {
                        new() { InfluenceTypeId = infPhysId, Value = 130 }, 
                        new() { InfluenceTypeId = infCriticalId, Value = 100 }
                    }
                },
                new()
                {
                    Name = "Uchigatana",
                    Weight = 5.0m,
                    EquipmentTypeId = typeKatanaId,
                    ReqStrength = 14,
                    ReqDexterity = 14,
                    IconPath = "/images/Weapons/Uchigatana.png",
                    WeaponInfluences = new List<WeaponInfluence>
                    {
                        new() { InfluenceTypeId = infPhysId, Value = 90 }, 
                        new() { InfluenceTypeId = infCriticalId, Value = 100 }
                    }
                },
                new()
                {
                    Name = "Great Moon sword",
                    Weight = 6.0m,
                    EquipmentTypeId = typeGreatSwordId,
                    ReqStrength = 16,
                    ReqDexterity = 11,
                    ReqIntelligence = 28,
                    IconPath = "/images/Weapons/Moon Light GreatSword.png",
                    WeaponInfluences = new List<WeaponInfluence>
                    {
                        new() { InfluenceTypeId = infMagicId, Value = 152 }, 
                        new() { InfluenceTypeId = infCriticalId, Value = 100 }
                    },
                },
                new()
                {
                    Name = "Grass Crest Shield",
                    Weight = 3.0m,
                    EquipmentTypeId = typeShieldId,
                    ReqStrength = 10,
                    IconPath = "/images/Weapons/Grass Crest Shield.png",
                    WeaponInfluences = new List<WeaponInfluence> { new() { InfluenceTypeId = infPhysId, Value = 62 } }
                },
                new()
                {
                    Name = "Big Axe of Dark Knight",
                    Weight = 16.0m,
                    EquipmentTypeId = typeGreatAxeId,
                    ReqStrength = 36,
                    ReqDexterity = 20,
                    IconPath = "/images/Weapons/big_axe_of_DarkKnight.png",
                    WeaponInfluences = new List<WeaponInfluence>
                    {
                        new() { InfluenceTypeId = infPhysId, Value = 229 },
                        new() { InfluenceTypeId = infCriticalId, Value = 100 }
                    }
                },
                new()
                {
                    Name = "Black Knight's Sword",
                    Weight = 16.0m,
                    EquipmentTypeId = typeGreatSwordId,
                    ReqStrength = 20,
                    ReqDexterity = 18,
                    IconPath = "/images/Weapons/Black Knight's Sword.jpg",
                    WeaponInfluences = new List<WeaponInfluence>
                    {
                        new() { InfluenceTypeId = infPhysId, Value = 220 },
                        new() { InfluenceTypeId = infCriticalId, Value = 100 }
                    }
                },
                new()
                {
                    Name = "Bandit's Knife",
                    Weight = 1.5m,
                    EquipmentTypeId = typeDaggerId,
                    ReqStrength = 6,
                    ReqDexterity = 12,
                    IconPath = "/images/Weapons/Bandits Knife.png",
                    WeaponInfluences = new List<WeaponInfluence>
                    {
                        new() { InfluenceTypeId = infPhysId, Value = 56 },
                        new() { InfluenceTypeId = infCriticalId, Value = 147 }
                    }
                },
                new()
                {
                    Name = "Longsword",
                    Weight = 3.0m,
                    EquipmentTypeId = typeStraightSwordId,
                    ReqStrength = 10,
                    ReqDexterity = 10,
                    IconPath = "/images/Weapons/Longsword.png",
                    WeaponInfluences = new List<WeaponInfluence>
                    {
                        new() { InfluenceTypeId = infPhysId, Value = 80 },
                        new() { InfluenceTypeId = infCriticalId, Value = 100 }
                    }
                },
                new()
                {
                    Name = "Balder Side Sword",
                    Weight = 3.0m,
                    EquipmentTypeId = typeStraightSwordId,
                    ReqStrength = 10,
                    ReqDexterity = 14,
                    IconPath = "/images/Weapons/Balder Side Sword.png",
                    WeaponInfluences = new List<WeaponInfluence>
                    {
                        new() { InfluenceTypeId = infPhysId, Value = 80 },
                        new() { InfluenceTypeId = infCriticalId, Value = 100 }
                    }
                },
                new()
                {
                    Name = "Falchion",
                    Weight = 2.5m,
                    EquipmentTypeId = typeCurvedSwordId,
                    ReqStrength = 9,
                    ReqDexterity = 13,
                    IconPath = "/images/Weapons/Falchion.png",
                    WeaponInfluences = new List<WeaponInfluence>
                    {
                        new() { InfluenceTypeId = infPhysId, Value = 82 }, 
                        new() { InfluenceTypeId = infCriticalId, Value = 100 }
                    }
                },
                new()
                {
                    Name = "Estoc",
                    Weight = 3.0m,
                    EquipmentTypeId = typeThrustingId,
                    ReqStrength = 10,
                    ReqDexterity = 12,
                    IconPath = "/images/Weapons/Estoc.png",
                    WeaponInfluences = new List<WeaponInfluence>
                    {
                        new() { InfluenceTypeId = infPhysId, Value = 75 },
                        new() { InfluenceTypeId = infCriticalId, Value = 100 }
                    }
                },
                new()
                {
                    Name = "Ricard's Rapier",
                    Weight = 2.0m,
                    EquipmentTypeId = typeThrustingId,
                    ReqStrength = 8,
                    ReqDexterity = 20,
                    IconPath = "/images/Weapons/Ricards Rapier.png",
                    WeaponInfluences = new List<WeaponInfluence>
                    {
                        new() { InfluenceTypeId = infPhysId, Value = 70 },
                        new() { InfluenceTypeId = infCriticalId, Value = 100 }
                    }
                },
                new()
                {
                    Name = "Battle Axe",
                    Weight = 4.0m,
                    EquipmentTypeId = typeAxeId,
                    ReqStrength = 12,
                    ReqDexterity = 8,
                    IconPath = "/images/Weapons/Battle Axe.png",
                    WeaponInfluences = new List<WeaponInfluence>
                    {
                        new() { InfluenceTypeId = infPhysId, Value = 95 }, 
                        new() { InfluenceTypeId = infCriticalId, Value = 100 }
                    }
                },
                new()
                {
                    Name = "Golem Axe",
                    Weight = 16.0m,
                    EquipmentTypeId = typeAxeId,
                    ReqStrength = 36,
                    ReqDexterity = 8,
                    IconPath = "/images/Weapons/Golem Axe.png",
                    WeaponInfluences = new List<WeaponInfluence>
                    {
                        new() { InfluenceTypeId = infPhysId, Value = 155 }, 
                        new() { InfluenceTypeId = infCriticalId, Value = 100 }
                    }
                },
                new()
                {
                    Name = "Mace",
                    Weight = 4.0m,
                    EquipmentTypeId = typeHammerId,
                    ReqStrength = 12,
                    ReqDexterity = 0,
                    IconPath = "/images/Weapons/Mace.png",
                    WeaponInfluences = new List<WeaponInfluence>
                    {
                        new() { InfluenceTypeId = infPhysId, Value = 91 }, 
                        new() { InfluenceTypeId = infCriticalId, Value = 100 }
                    }
                },
                new()
                {
                    Name = "Dragon Tooth",
                    Weight = 18.0m,
                    EquipmentTypeId = typeGreatHammerId,
                    ReqStrength = 40,
                    ReqDexterity = 0,
                    IconPath = "/images/Weapons/Dragon Tooth.png",
                    WeaponInfluences = new List<WeaponInfluence>
                    {
                        new() { InfluenceTypeId = infPhysId, Value = 185 }, 
                        new() { InfluenceTypeId = infCriticalId, Value = 100 }
                    }
                },

                new()
                {
                    Name = "Smough's Hammer",
                    Weight = 28.0m,
                    EquipmentTypeId = typeGreatHammerId,
                    ReqStrength = 58,
                    ReqDexterity = 0,
                    IconPath = "/images/Weapons/Smoughs Hammer.png",
                    WeaponInfluences = new List<WeaponInfluence>
                    {
                        new() { InfluenceTypeId = infPhysId, Value = 300 }, 
                        new() { InfluenceTypeId = infCriticalId, Value = 100 }
                    }
                },
                new()
                {
                    Name = "Great Scythe",
                    Weight = 5.0m,
                    EquipmentTypeId = typeHalberdId,
                    ReqStrength = 14,
                    ReqDexterity = 14,
                    IconPath = "/images/Weapons/Great Scythe.png",
                    WeaponInfluences = new List<WeaponInfluence>
                    {
                        new() { InfluenceTypeId = infPhysId, Value = 100 },
                        new() { InfluenceTypeId = infCriticalId, Value = 100 }
                    }
                },
                new()
                {
                    Name = "Dragonslayer Spear",
                    Weight = 10.0m,
                    EquipmentTypeId = typeSpearId,
                    ReqStrength = 24,
                    ReqDexterity = 24,
                    ReqFaith = 0,
                    IconPath = "/images/Weapons/Dragonslayer Spear.png",
                    WeaponInfluences = new List<WeaponInfluence>
                    {
                        new() { InfluenceTypeId = infPhysId, Value = 90 }, 
                        new() { InfluenceTypeId = infLightId, Value = 60 }, 
                        new() { InfluenceTypeId = infCriticalId, Value = 100 }
                    }
                },
                new()
                {
                    Name = "Moonlight Butterfly Horn",
                    Weight = 4.0m,
                    EquipmentTypeId = typeSpearId,
                    ReqStrength = 12,
                    ReqIntelligence = 0,
                    IconPath = "/images/Weapons/Moonlight Butterfly Horn.png",
                    WeaponInfluences = new List<WeaponInfluence>
                    {
                        new() { InfluenceTypeId = infMagicId, Value = 120 }, 
                        new() { InfluenceTypeId = infCriticalId, Value = 100 }
                    }
                },
                new()
                {
                    Name = "Demon's Greataxe",
                    Weight = 22.0m,
                    EquipmentTypeId = typeGreatAxeId,
                    ReqStrength = 46,
                    ReqDexterity = 0,
                    IconPath = "/images/Weapons/Demons Greataxe.png",
                    WeaponInfluences = new List<WeaponInfluence>
                    {
                        new() { InfluenceTypeId = infPhysId, Value = 285 }, 
                        new() { InfluenceTypeId = infCriticalId, Value = 100 }
                    }
                },
                new()
                {
                    Name = "Murakumo",
                    Weight = 12.0m,
                    EquipmentTypeId = typeCurvedGreatId,
                    ReqStrength = 28,
                    ReqDexterity = 13,
                    IconPath = "/images/Weapons/Murakumo.png",
                    WeaponInfluences = new List<WeaponInfluence>
                    {
                        new() { InfluenceTypeId = infPhysId, Value = 143 }, 
                        new() { InfluenceTypeId = infCriticalId, Value = 100 }
                    }
                },
                new()
                {
                    Name = "Gold Tracer",
                    Weight = 2.0m,
                    EquipmentTypeId = typeCurvedSwordId,
                    ReqStrength = 9,
                    ReqDexterity = 25,
                    IconPath = "/images/Weapons/Gold Tracer.png",
                    WeaponInfluences = new List<WeaponInfluence>
                    {
                        new() { InfluenceTypeId = infPhysId, Value = 130 }, 
                        new() { InfluenceTypeId = infCriticalId, Value = 100 }
                    }
                },
                new()
                {
                    Name = "Washing Pole",
                    Weight = 8.0m,
                    EquipmentTypeId = typeKatanaId,
                    ReqStrength = 20,
                    ReqDexterity = 16,
                    IconPath = "/images/Weapons/Washing Pole.png",
                    WeaponInfluences = new List<WeaponInfluence>
                    {
                        new() { InfluenceTypeId = infPhysId, Value = 90 }, 
                        new() { InfluenceTypeId = infCriticalId, Value = 100 }
                    }
                },
                new()
                {
                    Name = "Chaos Blade",
                    Weight = 6.0m,
                    EquipmentTypeId = typeKatanaId,
                    ReqStrength = 16,
                    ReqDexterity = 14,
                    IconPath = "/images/Weapons/Chaos Blade.png",
                    WeaponInfluences = new List<WeaponInfluence>
                    {
                        new() { InfluenceTypeId = infPhysId, Value = 144 }, 
                        new() { InfluenceTypeId = infCriticalId, Value = 100 }
                    }
                },
                new()
                {
                    Name = "Black Knight Shield",
                    Weight = 6.0m,
                    EquipmentTypeId = typeShieldId,
                    ReqStrength = 16,
                    IconPath = "/images/Weapons/Black Knight Shield.jpg",
                    WeaponInfluences = new List<WeaponInfluence> { new() { InfluenceTypeId = infPhysId, Value = 74 } }
                }
            };

            context.Equipments.AddRange(weapons);
            await context.SaveChangesAsync();

            var typeHelmId = (await context.EquipmentTypes.FirstAsync(at => at.Name == "Helm")).Id;
            var typeChestArmorId = (await context.EquipmentTypes.FirstAsync(at => at.Name == "Chest ArmorEquipment")).Id;
            var typeGauntletsId = (await context.EquipmentTypes.FirstAsync(at => at.Name == "Gauntlets")).Id;
            var typeLeggingsId = (await context.EquipmentTypes.FirstAsync(at => at.Name == "Leggings")).Id;

            var armors = new List<ArmorEquipment>
            {
                new()
                {
                    Name = "Elite Knight Helm",
                    Weight = 4.5m,
                    Poise = 4.2m,
                    EquipmentTypeId = typeHelmId,
                    IconPath = "/images/Armors/Elite Knight Helm.png",
                    ArmorInfluences = new List<ArmorInfluence> { new() { InfluenceTypeId = infPhysId, Value = 4.5 } }
                },
                new()
                {
                    Name = "Elite Knight ArmorEquipment",
                    Weight = 11.5m,
                    Poise = 16.0m,
                    EquipmentTypeId = typeChestArmorId,
                    IconPath = "/images/Armors/Elite Knight ArmorEquipment.jpg",
                    ArmorInfluences = new List<ArmorInfluence> { new() { InfluenceTypeId = infPhysId, Value = 13.0 } }
                },
                new()
                {
                    Name = "Elite Knight Gauntlets",
                    Weight = 3.5m,
                    Poise = 3.2m,
                    EquipmentTypeId = typeGauntletsId,
                    IconPath = "/images/Armors/Elite Knight Gauntlets.png",
                    ArmorInfluences = new List<ArmorInfluence> { new() { InfluenceTypeId = infPhysId, Value = 3.5 } }
                },
                new()
                {
                    Name = "Elite Knight Leggings",
                    Weight = 7.5m,
                    Poise = 8.5m,
                    EquipmentTypeId = typeLeggingsId,
                    IconPath = "/images/Armors/elite_knight_leggings.jpg", 
                    ArmorInfluences = new List<ArmorInfluence> { new() { InfluenceTypeId = infPhysId, Value = 7.5 } }
                },
                new()
                {
                    Name = "Havel's Helm",
                    Weight = 11.5m,
                    Poise = 10.0m,
                    EquipmentTypeId = typeHelmId,
                    IconPath = "/images/Armors/Havel's Helm.jpg",
                    ArmorInfluences = new List<ArmorInfluence> { new() { InfluenceTypeId = infPhysId, Value = 8.2 } }
                },
                new()
                {
                    Name = "Havel's ArmorEquipment",
                    Weight = 23.5m,
                    Poise = 40.0m,
                    EquipmentTypeId = typeChestArmorId,
                    IconPath = "/images/Armors/Havel's ArmorEquipment.jpg",
                    ArmorInfluences = new List<ArmorInfluence> { new() { InfluenceTypeId = infPhysId, Value = 20.5 } }
                },
                new()
                {
                    Name = "Havel's Gauntlets",
                    Weight = 11.5m,
                    Poise = 9.0m,
                    EquipmentTypeId = typeGauntletsId,
                    IconPath = "/images/Armors/Havel's Gauntlets.jpg",
                    ArmorInfluences = new List<ArmorInfluence> { new() { InfluenceTypeId = infPhysId, Value = 7.2 } }
                },
                new()
                {
                    Name = "Havel's Leggings",
                    Weight = 13.5m,
                    Poise = 20.0m,
                    EquipmentTypeId = typeLeggingsId,
                    IconPath = "/images/Armors/Havel's Leggings.jpg",
                    ArmorInfluences = new List<ArmorInfluence> 
                    { 
                        new() { InfluenceTypeId = infPhysId, Value = 12.0 }, 
                        new() { InfluenceTypeId = infMagicId, Value = 14.0 } 
                    }
                },
                new()
                {
                    Name = "Pyromancer Hood",
                    Weight = 1.5m,
                    Poise = 0.0m,
                    EquipmentTypeId = typeHelmId,
                    IconPath = "/images/Armors/Pyromancer Hood.png",
                    ArmorInfluences = new List<ArmorInfluence> { new() { InfluenceTypeId = infFireId, Value = 5.0 } }
                },
                new()
                {
                    Name = "Pyromancer Garb",
                    Weight = 3.2m,
                    Poise = 0.0m,
                    EquipmentTypeId = typeChestArmorId,
                    IconPath = "/images/Armors/Pyromancer Garb.png",
                    ArmorInfluences = new List<ArmorInfluence> { new() { InfluenceTypeId = infFireId, Value = 10.0 } }
                },
                new()
                {
                    Name = "Pyromancer Gauntlets",
                    Weight = 1.2m,
                    Poise = 0.0m,
                    EquipmentTypeId = typeGauntletsId,
                    IconPath = "/images/Armors/Pyromancer Gauntlets.png",
                    ArmorInfluences = new List<ArmorInfluence> { new() { InfluenceTypeId = infFireId, Value = 2.0 } }
                },
                new()
                {
                    Name = "Pyromancer Leggings",
                    Weight = 2.0m,
                    Poise = 0.0m,
                    EquipmentTypeId = typeLeggingsId,
                    IconPath = "/images/Armors/Pyromancer Leggings.png",
                    ArmorInfluences = new List<ArmorInfluence> { new() { InfluenceTypeId = infFireId, Value = 4.0 } }
                },
                new() 
                { 
                    Name = "Black Iron Helm", 
                    Weight = 6.0m, Poise = 6.0m, 
                    EquipmentTypeId = typeHelmId, 
                    IconPath = "/images/Armors/Black Iron Helm.png", 
                    ArmorInfluences = new List<ArmorInfluence>
                    {
                        new() { InfluenceTypeId = infPhysId, Value = 6.8 }, 
                        new() { InfluenceTypeId = infFireId, Value = 4.0 }
                    }
                },
                new() 
                { 
                    Name = "Black Iron ArmorEquipment", 
                    Weight = 15.6m, Poise = 19.0m, 
                    EquipmentTypeId = typeChestArmorId, 
                    IconPath = "/images/Armors/Black Iron ArmorEquipment.png", 
                    ArmorInfluences = new List<ArmorInfluence>
                    {
                        new() { InfluenceTypeId = infPhysId, Value = 17.5 }, 
                        new() { InfluenceTypeId = infFireId, Value = 9.0 }
                    } 
                },
                new() 
                { 
                    Name = "Black Iron Gauntlets", 
                    Weight = 5.2m, Poise = 5.0m, 
                    EquipmentTypeId = typeGauntletsId, 
                    IconPath = "/images/Armors/Black Iron Gauntlets.png", 
                    ArmorInfluences = new List<ArmorInfluence>
                    {
                        new() { InfluenceTypeId = infPhysId, Value = 4.9 }, 
                        new() { InfluenceTypeId = infFireId, Value = 2.0 }
                    } 
                },
                new() 
                { 
                    Name = "Black Iron Leggings", 
                    Weight = 9.2m, Poise = 10.0m, 
                    EquipmentTypeId = typeLeggingsId, 
                    IconPath = "/images/Armors/Black Iron Leggings.png", 
                    ArmorInfluences = new List<ArmorInfluence>
                    {
                        new() { InfluenceTypeId = infPhysId, Value = 9.5 }, 
                        new() { InfluenceTypeId = infFireId, Value = 5.0 }
                    } 
                },
                new()
                {
                    Name = "Gold-Hemmed Hood", 
                    Weight = 1.4m, 
                    Poise = 0.0m, 
                    EquipmentTypeId = typeHelmId, 
                    IconPath = "/images/Armors/Gold-Hemmed Hood.png", 
                    ArmorInfluences = new List<ArmorInfluence> { new() { InfluenceTypeId = infFireId, Value = 5.0 } }
                },
                new()
                {
                    Name = "Gold-Hemmed Cloak", 
                    Weight = 3.0m, 
                    Poise = 0.0m, 
                    EquipmentTypeId = typeChestArmorId, 
                    IconPath = "/images/Armors/Gold-Hemmed Cloak.png", 
                    ArmorInfluences = new List<ArmorInfluence> { new() { InfluenceTypeId = infFireId, Value = 12.0 } }
                },
                new()
                {
                    Name = "Gold-Hemmed Gloves", 
                    Weight = 1.1m, 
                    Poise = 0.0m, 
                    EquipmentTypeId = typeGauntletsId, 
                    IconPath = "/images/Armors/Gold-Hemmed Gloves.png", 
                    ArmorInfluences = new List<ArmorInfluence> { new() { InfluenceTypeId = infFireId, Value = 3.0 } }
                },
                new()
                {
                    Name = "Gold-Hemmed Skirt", 
                    Weight = 1.9m, 
                    Poise = 0.0m, 
                    EquipmentTypeId = typeLeggingsId, 
                    IconPath = "/images/Armors/Gold-Hemmed Skirt.png", 
                    ArmorInfluences = new List<ArmorInfluence> { new() { InfluenceTypeId = infFireId, Value = 7.0 } }
                },
                new()
                {
                    Name = "Catarina Helm", 
                    Weight = 6.2m, 
                    Poise = 6.0m, 
                    EquipmentTypeId = typeHelmId, 
                    IconPath = "/images/Armors/Catarina Helm.png", 
                    ArmorInfluences = new List<ArmorInfluence> { new() { InfluenceTypeId = infPhysId, Value = 6.0 } }
                },
                new()
                {
                    Name = "Catarina ArmorEquipment", 
                    Weight = 16.7m, 
                    Poise = 17.0m, 
                    EquipmentTypeId = typeChestArmorId, 
                    IconPath = "/images/Armors/Catarina ArmorEquipment.png", 
                    ArmorInfluences = new List<ArmorInfluence> { new() { InfluenceTypeId = infPhysId, Value = 15.0 } } 
                },
                new() 
                { 
                    Name = "Catarina Gauntlets", 
                    Weight = 5.3m, 
                    Poise = 5.0m, 
                    EquipmentTypeId = typeGauntletsId, 
                    IconPath = "/images/Armors/Catarina Gauntlets.png", 
                    ArmorInfluences = new List<ArmorInfluence> { new() { InfluenceTypeId = infPhysId, Value = 5.0 } } 
                },
                new() 
                { 
                    Name = "Catarina Leggings", 
                    Weight = 9.8m, 
                    Poise = 10.0m, 
                    EquipmentTypeId = typeLeggingsId, 
                    IconPath = "/images/Armors/Catarina Leggings.png", 
                    ArmorInfluences = new List<ArmorInfluence> { new() { InfluenceTypeId = infPhysId, Value = 9.0 } } 
                },
                new() 
                { 
                    Name = "Silver Knight Helm", 
                    Weight = 4.2m, 
                    Poise = 5.0m, 
                    EquipmentTypeId = typeHelmId, 
                    IconPath = "/images/Armors/Silver Knight Helm.png", 
                    ArmorInfluences = new List<ArmorInfluence> { new() { InfluenceTypeId = infPhysId, Value = 5.5 } } 
                },
                new() 
                { 
                    Name = "Silver Knight ArmorEquipment", 
                    Weight = 10.8m, 
                    Poise = 14.0m, 
                    EquipmentTypeId = typeChestArmorId, 
                    IconPath = "/images/Armors/Silver Knight ArmorEquipment.png", 
                    ArmorInfluences = new List<ArmorInfluence> { new() { InfluenceTypeId = infPhysId, Value = 12.0 } } 
                },
                new() 
                { 
                    Name = "Silver Knight Gauntlets", 
                    Weight = 3.6m, 
                    Poise = 4.0m, 
                    EquipmentTypeId = typeGauntletsId, 
                    IconPath = "/images/Armors/Silver Knight Gauntlets.png", 
                    ArmorInfluences = new List<ArmorInfluence> { new() { InfluenceTypeId = infPhysId, Value = 4.0 } } 
                },
                new() 
                { 
                    Name = "Silver Knight Leggings", 
                    Weight = 6.4m, 
                    Poise = 8.0m, 
                    EquipmentTypeId = typeLeggingsId, 
                    IconPath = "/images/Armors/Silver Knight Leggings.png", 
                    ArmorInfluences = new List<ArmorInfluence> { new() { InfluenceTypeId = infPhysId, Value = 7.0 } } 
                },
                new() 
                { 
                    Name = "Black Knight Helm", 
                    Weight = 4.5m, 
                    Poise = 5.0m, 
                    EquipmentTypeId = typeHelmId, 
                    IconPath = "/images/Armors/Black Knight Helm.png", 
                    ArmorInfluences = new List<ArmorInfluence> { new() { InfluenceTypeId = infFireId, Value = 4.0 } } 
                },
                new() 
                { 
                    Name = "Black Knight ArmorEquipment", 
                    Weight = 11.5m, 
                    Poise = 14.0m, 
                    EquipmentTypeId = typeChestArmorId, 
                    IconPath = "/images/Armors/Black Knight ArmorEquipment.png", 
                    ArmorInfluences = new List<ArmorInfluence> { new() { InfluenceTypeId = infFireId, Value = 10.0 } } 
                },
                new() 
                { 
                    Name = "Black Knight Gauntlets", 
                    Weight = 3.8m, 
                    Poise = 4.0m, 
                    EquipmentTypeId = typeGauntletsId, 
                    IconPath = "/images/Armors/Black Knight Gauntlets.png", 
                    ArmorInfluences = new List<ArmorInfluence> { new() { InfluenceTypeId = infFireId, Value = 3.0 } } 
                },
                new() 
                { 
                    Name = "Black Knight Leggings", 
                    Weight = 6.8m, 
                    Poise = 8.0m, 
                    EquipmentTypeId = typeLeggingsId, 
                    IconPath = "/images/Armors/Black Knight Leggings.png", 
                    ArmorInfluences = new List<ArmorInfluence> { new() { InfluenceTypeId = infFireId, Value = 6.0 } } 
                },
                new() 
                { 
                    Name = "Ornstein's Helm", 
                    Weight = 5.5m, 
                    Poise = 5.0m, 
                    EquipmentTypeId = typeHelmId, 
                    IconPath = "/images/Armors/Ornsteins Helm.png", 
                    ArmorInfluences = new List<ArmorInfluence> { new() { InfluenceTypeId = infLightId, Value = 6.0 } } 
                },
                new() 
                { 
                    Name = "Ornstein's ArmorEquipment", 
                    Weight = 12.0m, 
                    Poise = 15.0m, 
                    EquipmentTypeId = typeChestArmorId, 
                    IconPath = "/images/Armors/Ornsteins ArmorEquipment.png", 
                    ArmorInfluences = new List<ArmorInfluence> { new() { InfluenceTypeId = infLightId, Value = 13.0 } } 
                },
                new() 
                { 
                    Name = "Ornstein's Gauntlets", 
                    Weight = 4.0m, 
                    Poise = 4.0m, 
                    EquipmentTypeId = typeGauntletsId, 
                    IconPath = "/images/Armors/Ornsteins Gauntlets.png", 
                    ArmorInfluences = new List<ArmorInfluence> { new() { InfluenceTypeId = infLightId, Value = 4.0 } } 
                },
                new() 
                { 
                    Name = "Ornstein's Leggings", 
                    Weight = 8.0m, 
                    Poise = 9.0m, 
                    EquipmentTypeId = typeLeggingsId, 
                    IconPath = "/images/Armors/Ornsteins Leggings.png", 
                    ArmorInfluences = new List<ArmorInfluence> { new() { InfluenceTypeId = infLightId, Value = 8.0 } } 
                },
                new() 
                { 
                    Name = "Smough's Helm", 
                    Weight = 13.0m, 
                    Poise = 11.0m, 
                    EquipmentTypeId = typeHelmId, 
                    IconPath = "/images/Armors/Smoughs Helm.png", 
                    ArmorInfluences = new List<ArmorInfluence> { new() { InfluenceTypeId = infPhysId, Value = 9.0 } } 
                },
                new() 
                { 
                    Name = "Smough's ArmorEquipment", 
                    Weight = 26.0m, 
                    Poise = 42.0m, 
                    EquipmentTypeId = typeChestArmorId, 
                    IconPath = "/images/Armors/Smoughs ArmorEquipment.png", 
                    ArmorInfluences = new List<ArmorInfluence> { new() { InfluenceTypeId = infPhysId, Value = 22.0 } } 
                },
                new() 
                { 
                    Name = "Smough's Gauntlets", 
                    Weight = 12.0m, 
                    Poise = 10.0m, 
                    EquipmentTypeId = typeGauntletsId, 
                    IconPath = "/images/Armors/Smoughs Gauntlets.png", 
                    ArmorInfluences = new List<ArmorInfluence> { new() { InfluenceTypeId = infPhysId, Value = 8.0 } } 
                },
                new() 
                { 
                    Name = "Smough's Leggings", 
                    Weight = 14.5m, 
                    Poise = 21.0m, 
                    EquipmentTypeId = typeLeggingsId, 
                    IconPath = "/images/Armors/Smoughs Leggings.png", 
                    ArmorInfluences = new List<ArmorInfluence> { new() { InfluenceTypeId = infPhysId, Value = 13.0 } } 
                },
                new() 
                { 
                    Name = "Helm of Artorias", 
                    Weight = 4.2m, 
                    Poise = 4.0m, 
                    EquipmentTypeId = typeHelmId, 
                    IconPath = "/images/Armors/Artorias Helm.png", 
                    ArmorInfluences = new List<ArmorInfluence> { new() { InfluenceTypeId = infPhysId, Value = 5.0 } } 
                },
                new() 
                { 
                    Name = "ArmorEquipment of Artorias", 
                    Weight = 10.9m, 
                    Poise = 13.0m, 
                    EquipmentTypeId = typeChestArmorId, 
                    IconPath = "/images/Armors/Artorias ArmorEquipment.png", 
                    ArmorInfluences = new List<ArmorInfluence> { new() { InfluenceTypeId = infPhysId, Value = 11.0 } } 
                },
                new() 
                { 
                    Name = "Gauntlets of Artorias", 
                    Weight = 3.6m, 
                    Poise = 3.0m, 
                    EquipmentTypeId = typeGauntletsId, 
                    IconPath = "/images/Armors/Artorias Gauntlets.png", 
                    ArmorInfluences = new List<ArmorInfluence> { new() { InfluenceTypeId = infPhysId, Value = 3.5 } } 
                },
                new() 
                { 
                    Name = "Leggings of Artorias", 
                    Weight = 6.3m, 
                    Poise = 7.0m, 
                    EquipmentTypeId = typeLeggingsId, 
                    IconPath = "/images/Armors/Artorias Leggings.png", 
                    ArmorInfluences = new List<ArmorInfluence> { new() { InfluenceTypeId = infPhysId, Value = 6.5 } } 
                },
                new() 
                { 
                    Name = "Mask of the Sealer", 
                    Weight = 1.0m, 
                    Poise = 0.0m, 
                    EquipmentTypeId = typeHelmId, 
                    IconPath = "/images/Armors/Mask of the Sealer.png", 
                    ArmorInfluences = new List<ArmorInfluence> { new() { InfluenceTypeId = infMagicId, Value = 4.0 } } 
                },
                new() 
                { 
                    Name = "Crimson Robe", 
                    Weight = 2.4m, 
                    Poise = 0.0m, 
                    EquipmentTypeId = typeChestArmorId, 
                    IconPath = "/images/Armors/Crimson Robe.png", 
                    ArmorInfluences = new List<ArmorInfluence> { new() { InfluenceTypeId = infMagicId, Value = 9.0 } } 
                },
                new() 
                { 
                    Name = "Crimson Gloves", 
                    Weight = 0.9m, 
                    Poise = 0.0m, 
                    EquipmentTypeId = typeGauntletsId, 
                    IconPath = "/images/Armors/Crimson Gloves.png", 
                    ArmorInfluences = new List<ArmorInfluence> { new() { InfluenceTypeId = infMagicId, Value = 2.0 } } 
                },
                new() 
                { 
                    Name = "Crimson Waistcloth", 
                    Weight = 1.5m, 
                    Poise = 0.0m, 
                    EquipmentTypeId = typeLeggingsId, 
                    IconPath = "/images/Armors/Crimson Waistcloth.png", 
                    ArmorInfluences = new List<ArmorInfluence> { new() { InfluenceTypeId = infMagicId, Value = 3.5 } } 
                },
                new() 
                { 
                    Name = "Giant Helm", 
                    Weight = 6.8m, 
                    Poise = 7.0m, 
                    EquipmentTypeId = typeHelmId, 
                    IconPath = "/images/Armors/Giant Helm.png", 
                    ArmorInfluences = new List<ArmorInfluence> { new() { InfluenceTypeId = infPhysId, Value = 6.5 } } 
                },
                new() 
                { 
                    Name = "Giant ArmorEquipment", 
                    Weight = 16.4m, 
                    Poise = 21.0m, 
                    EquipmentTypeId = typeChestArmorId, 
                    IconPath = "/images/Armors/Giant ArmorEquipment.png", 
                    ArmorInfluences = new List<ArmorInfluence> { new() { InfluenceTypeId = infPhysId, Value = 16.0 } } 
                },
                new() 
                { 
                    Name = "Giant Gauntlets", 
                    Weight = 5.9m, 
                    Poise = 6.0m, 
                    EquipmentTypeId = typeGauntletsId, 
                    IconPath = "/images/Armors/Giant Gauntlets.png", 
                    ArmorInfluences = new List<ArmorInfluence> { new() { InfluenceTypeId = infPhysId, Value = 5.0 } } 
                },
                new() 
                { 
                    Name = "Giant Leggings", 
                    Weight = 9.7m, 
                    Poise = 11.0m, 
                    EquipmentTypeId = typeLeggingsId, 
                    IconPath = "/images/Armors/Giant Leggings.png", 
                    ArmorInfluences = new List<ArmorInfluence> { new() { InfluenceTypeId = infPhysId, Value = 9.0 } } 
                }
            };

            context.Equipments.AddRange(armors);
            await context.SaveChangesAsync();
        }
    }
}
