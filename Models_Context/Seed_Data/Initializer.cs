using Microsoft.EntityFrameworkCore;
using Models_Context.Context;
using Models_Context.Models;
using System.Collections.Generic;

namespace Models_Context.Seed_Data
{
    public static class Initializer
    {
        public static void Initialize(RPGDarkSoulsDbContext context)
        {
            // Перестворюємо базу, щоб оновити дані
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            // --- 1. СЛОТИ ---
            var slotHead = new Slot { Name = "Head" };
            var slotChest = new Slot { Name = "Chest" };
            var slotHands = new Slot { Name = "Hands" };
            var slotLegs = new Slot { Name = "Legs" };
            var slotRHand = new Slot { Name = "RightHand" };
            var slotLHand = new Slot { Name = "LeftHand" };

            context.Slots.AddRange(slotHead, slotChest, slotHands, slotLegs, slotRHand, slotLHand);
            context.SaveChanges();

            // --- 2. ТИПИ ВПЛИВУ ---
            var infPhys = new InfluenceType { Name = "Physical" };
            var infMagic = new InfluenceType { Name = "Magic" };
            var infFire = new InfluenceType { Name = "Fire" };
            var infLight = new InfluenceType { Name = "Lightning" };
            var infCritical = new InfluenceType { Name = "Critical" };


            context.InfluenceTypes.AddRange(infPhys, infMagic, infFire, infLight, infCritical);
            context.SaveChanges();

            // --- 3. ТИПИ ПРЕДМЕТІВ ---
            var typeStraight_sword = new WeaponType { Type = "Straight sword", SlotId = slotRHand.SlotId };
            var type_Halberd = new WeaponType {Type="Halberd", SlotId=slotRHand.SlotId };
            var type_Spear = new WeaponType { Type = "Spear", SlotId = slotRHand.SlotId };
            var typeGreat_Axe = new WeaponType { Type = "Great_Axe", SlotId = slotRHand.SlotId };
            var typeGreatsword = new WeaponType { Type = "Greatsword", SlotId = slotRHand.SlotId };
            var typeHuge = new WeaponType { Type = "Huge sword", SlotId = slotRHand.SlotId };
            var typeKatana = new WeaponType { Type = "Katana", SlotId = slotRHand.SlotId };
            var typeShield = new WeaponType { Type = "Shield", SlotId = slotLHand.SlotId };
            var typeHelm = new ArmorType { Type = "Helm", SlotId = slotHead.SlotId };
            var typeChestArmor = new ArmorType { Type = "Chest Armor", SlotId = slotChest.SlotId };
            var typeGauntlets = new ArmorType { Type = "Gauntlets", SlotId = slotHands.SlotId };
            var typeLeggings = new ArmorType { Type = "Leggings", SlotId = slotLegs.SlotId };
            var typeDagger = new WeaponType { Type = "Dagger", SlotId = slotRHand.SlotId };
            var typeCurvedSword = new WeaponType { Type = "Curved Sword", SlotId = slotRHand.SlotId };
            var typeThrusting = new WeaponType { Type = "Thrusting Sword", SlotId = slotRHand.SlotId };
            var typeAxe = new WeaponType { Type = "Axe", SlotId = slotRHand.SlotId };
            var typeHammer = new WeaponType { Type = "Hammer", SlotId = slotRHand.SlotId };
            var typeGreatHammer = new WeaponType { Type = "Great Hammer", SlotId = slotRHand.SlotId };
            var typeCurvedGreat = new WeaponType { Type = "Curved Greatsword", SlotId = slotRHand.SlotId };

            context.WeaponTypes.AddRange(
                typeStraight_sword, type_Halberd, type_Spear, typeGreat_Axe, typeGreatsword, typeHuge, typeKatana, typeShield,
                typeDagger, typeCurvedSword, typeThrusting, typeAxe, typeHammer, typeGreatHammer, typeCurvedGreat
            );
            context.ArmorTypes.AddRange(typeHelm, typeChestArmor, typeGauntlets, typeLeggings);
            context.SaveChanges();

            // --- 4. ЗБРОЯ (ПРОПИСУЄМО ТОЧНІ ШЛЯХИ ДО ФОТО) ---
            var weapons = new List<Weapon>
            {
                new Weapon
                {
                    Name = "Black Knight's Greatsword",
                    Weight = 14.0m,
                    WeaponTypeId = typeHuge.TypeId,
                    ReqStrenght = 32,
                    ReqDexterity = 18,
                    IconPath = "/images/Weapons/Black Knight's Greatsword.jpg",
                    WeaponInfluences = new List<WeaponIfnluence> { new WeaponIfnluence { InfluenceTypeId = infPhys.TypeId, Value = 220 }, new WeaponIfnluence { InfluenceTypeId = infCritical.TypeId, Value = 100 } }
                },
                new Weapon
                {
                    Name = "Greatsword of Artorias",
                    Weight = 10.0m,
                    WeaponTypeId = typeGreatsword.TypeId,
                    ReqStrenght = 24,
                    ReqDexterity = 18,
                    ReqIntelligence = 18,
                    ReqFaith = 18,
                    IconPath = "/images/Weapons/Greatsword of Artorias.jpg", 
                    WeaponInfluences = new List<WeaponIfnluence> { new WeaponIfnluence { InfluenceTypeId = infPhys.TypeId, Value = 178 }, new WeaponIfnluence { InfluenceTypeId = infCritical.TypeId, Value = 100 } }
                },
                new Weapon
                {
                    Name="Dark Sword",
                    Weight=6.0m,
                    WeaponTypeId = typeStraight_sword.TypeId,
                    ReqStrenght=16,
                    ReqDexterity=16,
                    IconPath="/images/Weapons/Dark Sword.png",
                    WeaponInfluences= new List<WeaponIfnluence>{ new WeaponIfnluence {InfluenceTypeId=infPhys.TypeId, Value=82 }, new WeaponIfnluence { InfluenceTypeId = infCritical.TypeId, Value = 100 } }

                },
                new Weapon
                {
                    Name="Astor Straight Sword",
                    Weight=3.0m,
                    WeaponTypeId = typeStraight_sword.TypeId,
                    ReqStrenght=10,
                    ReqDexterity=10,
                    ReqFaith=14,
                    IconPath="/images/Weapons/Astor Straight Sword.png",
                    WeaponInfluences= new List<WeaponIfnluence>{ new WeaponIfnluence {InfluenceTypeId=infPhys.TypeId, Value=80 }, new WeaponIfnluence { InfluenceTypeId = infMagic.TypeId, Value = 80 }, new WeaponIfnluence { InfluenceTypeId = infCritical.TypeId, Value = 100 } }

                },
                new Weapon
                {
                    Name="Black Knight's Halberd",
                    Weight=14.0m,
                    WeaponTypeId = type_Halberd.TypeId,
                    ReqStrenght=32,
                    ReqDexterity=18,
                    IconPath="/images/Weapons/Black Knight's Halberd.jpg",
                    WeaponInfluences= new List<WeaponIfnluence>{ new WeaponIfnluence {InfluenceTypeId=infPhys.TypeId, Value=245 }, new WeaponIfnluence { InfluenceTypeId = infMagic.TypeId, Value = 80 }, new WeaponIfnluence { InfluenceTypeId = infCritical.TypeId, Value = 100 } }

                },
                new Weapon
                {
                    Name = "Winged Spear",
                    Weight = 4.5m,
                    WeaponTypeId = type_Spear.TypeId,
                    ReqStrenght = 13,
                    ReqDexterity = 15,
                    IconPath = "/images/Weapons/Winged Spear.png", // Увага: файл називається claymor.png
                    WeaponInfluences = new List<WeaponIfnluence> { new WeaponIfnluence { InfluenceTypeId = infPhys.TypeId, Value = 86 }, new WeaponIfnluence { InfluenceTypeId = infMagic.TypeId, Value = 80 }, new WeaponIfnluence { InfluenceTypeId = infCritical.TypeId, Value = 100 } }
                },
                new Weapon
                {
                    Name = "Claymor",
                    Weight = 9.0m,
                    WeaponTypeId = typeGreatsword.TypeId,
                    ReqStrenght = 16,
                    ReqDexterity = 13,
                    IconPath = "/images/Weapons/claymor.png", // Увага: файл називається claymor.png
                    WeaponInfluences = new List<WeaponIfnluence> { new WeaponIfnluence { InfluenceTypeId = infPhys.TypeId, Value = 138 }, new WeaponIfnluence { InfluenceTypeId = infCritical.TypeId, Value = 100 } }
                },
                new Weapon
                {
                    Name = "Zweihander",
                    Weight = 10.0m,
                    WeaponTypeId = typeHuge.TypeId,
                    ReqStrenght = 24,
                    ReqDexterity = 10,
                    IconPath = "/images/Weapons/Zweihander.png", // Ваша назва файлу
                    WeaponInfluences = new List<WeaponIfnluence> { new WeaponIfnluence { InfluenceTypeId = infPhys.TypeId, Value = 130 }, new WeaponIfnluence { InfluenceTypeId = infCritical.TypeId, Value = 100 } }
                },
                new Weapon
                {
                    Name = "Uchigatana",
                    Weight = 5.0m,
                    WeaponTypeId = typeKatana.TypeId,
                    ReqStrenght = 14,
                    ReqDexterity = 14,
                    IconPath = "/images/Weapons/Uchigatana.png",
                    WeaponInfluences = new List<WeaponIfnluence> { new WeaponIfnluence { InfluenceTypeId = infPhys.TypeId, Value = 90 }, new WeaponIfnluence { InfluenceTypeId = infCritical.TypeId, Value = 100 } }
                },
                new Weapon
                {
                    Name = "Great Moon sword",
                    Weight = 6.0m,
                    WeaponTypeId = typeGreatsword.TypeId,
                    ReqStrenght = 16,
                    ReqDexterity = 11,
                    ReqIntelligence = 28,
                    IconPath = "/images/Weapons/Moon Light GreatSword.png",
                    WeaponInfluences = new List<WeaponIfnluence> { new WeaponIfnluence { InfluenceTypeId = infMagic.TypeId, Value = 152 }, new WeaponIfnluence { InfluenceTypeId = infCritical.TypeId, Value = 100 } },

                },
                new Weapon
                {
                    Name = "Grass Crest Shield",
                    Weight = 3.0m,
                    WeaponTypeId = typeShield.TypeId,
                    ReqStrenght = 10,
                    IconPath = "/images/Weapons/Grass Crest Shield.png",
                    WeaponInfluences = new List<WeaponIfnluence> { new WeaponIfnluence { InfluenceTypeId = infPhys.TypeId, Value = 62 } }
                },
                new Weapon
                {
                    Name = "Big Axe of Dark Knight",
                    Weight = 16.0m,
                    WeaponTypeId = typeGreat_Axe.TypeId,
                    ReqStrenght = 36,
                    ReqDexterity = 20,
                    IconPath = "/images/Weapons/big_axe_of_DarkKnight.png",
                    WeaponInfluences = new List<WeaponIfnluence> { new WeaponIfnluence { InfluenceTypeId = infPhys.TypeId, Value = 229 }, new WeaponIfnluence { InfluenceTypeId = infCritical.TypeId, Value = 100 } }
                },
                new Weapon
                {
                    Name = "Black Knight's Sword",
                    Weight = 16.0m,
                    WeaponTypeId = typeGreatsword.TypeId,
                    ReqStrenght = 20,
                    ReqDexterity = 18,
                    IconPath = "/images/Weapons/Black Knight's Sword.jpg",
                    WeaponInfluences = new List<WeaponIfnluence> { new WeaponIfnluence { InfluenceTypeId = infPhys.TypeId, Value = 220 }, new WeaponIfnluence { InfluenceTypeId = infCritical.TypeId, Value = 100 } }
                }

            };
            // === 20 НОВИХ ОДИНИЦЬ ЗБРОЇ ===

            // 1. Bandit's Knife (Dagger - Високий Крит)
            weapons.Add(new Weapon
            {
                Name = "Bandit's Knife",
                Weight = 1.5m,
                WeaponTypeId = typeDagger.TypeId,
                ReqStrenght = 6,
                ReqDexterity = 12,
                IconPath = "/images/Weapons/Bandits Knife.png",
                WeaponInfluences = new List<WeaponIfnluence> { new WeaponIfnluence { InfluenceTypeId = infPhys.TypeId, Value = 56 }, new WeaponIfnluence { InfluenceTypeId = infCritical.TypeId, Value = 147 } }
            });

            // 2. Longsword (Straight Sword)
            weapons.Add(new Weapon
            {
                Name = "Longsword",
                Weight = 3.0m,
                WeaponTypeId = typeStraight_sword.TypeId,
                ReqStrenght = 10,
                ReqDexterity = 10,
                IconPath = "/images/Weapons/Longsword.png",
                WeaponInfluences = new List<WeaponIfnluence> { new WeaponIfnluence { InfluenceTypeId = infPhys.TypeId, Value = 80 }, new WeaponIfnluence { InfluenceTypeId = infCritical.TypeId, Value = 100 } }
            });

            // 3. Balder Side Sword (Straight Sword)
            weapons.Add(new Weapon
            {
                Name = "Balder Side Sword",
                Weight = 3.0m,
                WeaponTypeId = typeStraight_sword.TypeId,
                ReqStrenght = 10,
                ReqDexterity = 14,
                IconPath = "/images/Weapons/Balder Side Sword.png",
                WeaponInfluences = new List<WeaponIfnluence> { new WeaponIfnluence { InfluenceTypeId = infPhys.TypeId, Value = 80 }, new WeaponIfnluence { InfluenceTypeId = infCritical.TypeId, Value = 100 } }
            });

            // 4. Falchion (Curved Sword)
            weapons.Add(new Weapon
            {
                Name = "Falchion",
                Weight = 2.5m,
                WeaponTypeId = typeCurvedSword.TypeId,
                ReqStrenght = 9,
                ReqDexterity = 13,
                IconPath = "/images/Weapons/Falchion.png",
                WeaponInfluences = new List<WeaponIfnluence> { new WeaponIfnluence { InfluenceTypeId = infPhys.TypeId, Value = 82 }, new WeaponIfnluence { InfluenceTypeId = infCritical.TypeId, Value = 100 } }
            });

            // 5. Estoc (Thrusting Sword)
            weapons.Add(new Weapon
            {
                Name = "Estoc",
                Weight = 3.0m,
                WeaponTypeId = typeThrusting.TypeId,
                ReqStrenght = 10,
                ReqDexterity = 12,
                IconPath = "/images/Weapons/Estoc.png",
                WeaponInfluences = new List<WeaponIfnluence> { new WeaponIfnluence { InfluenceTypeId = infPhys.TypeId, Value = 75 }, new WeaponIfnluence { InfluenceTypeId = infCritical.TypeId, Value = 100 } }
            });

            // 6. Ricard's Rapier (Thrusting Sword)
            weapons.Add(new Weapon
            {
                Name = "Ricard's Rapier",
                Weight = 2.0m,
                WeaponTypeId = typeThrusting.TypeId,
                ReqStrenght = 8,
                ReqDexterity = 20,
                IconPath = "/images/Weapons/Ricards Rapier.png",
                WeaponInfluences = new List<WeaponIfnluence> { new WeaponIfnluence { InfluenceTypeId = infPhys.TypeId, Value = 70 }, new WeaponIfnluence { InfluenceTypeId = infCritical.TypeId, Value = 100 } }
            });

            // 7. Battle Axe (Axe)
            weapons.Add(new Weapon
            {
                Name = "Battle Axe",
                Weight = 4.0m,
                WeaponTypeId = typeAxe.TypeId,
                ReqStrenght = 12,
                ReqDexterity = 8,
                IconPath = "/images/Weapons/Battle Axe.png",
                WeaponInfluences = new List<WeaponIfnluence> { new WeaponIfnluence { InfluenceTypeId = infPhys.TypeId, Value = 95 }, new WeaponIfnluence { InfluenceTypeId = infCritical.TypeId, Value = 100 } }
            });

            // 8. Golem Axe (Axe)
            weapons.Add(new Weapon
            {
                Name = "Golem Axe",
                Weight = 16.0m,
                WeaponTypeId = typeAxe.TypeId,
                ReqStrenght = 36,
                ReqDexterity = 8,
                IconPath = "/images/Weapons/Golem Axe.png",
                WeaponInfluences = new List<WeaponIfnluence> { new WeaponIfnluence { InfluenceTypeId = infPhys.TypeId, Value = 155 }, new WeaponIfnluence { InfluenceTypeId = infCritical.TypeId, Value = 100 } }
            });

            // 9. Mace (Hammer)
            weapons.Add(new Weapon
            {
                Name = "Mace",
                Weight = 4.0m,
                WeaponTypeId = typeHammer.TypeId,
                ReqStrenght = 12,
                ReqDexterity = 0,
                IconPath = "/images/Weapons/Mace.png",
                WeaponInfluences = new List<WeaponIfnluence> { new WeaponIfnluence { InfluenceTypeId = infPhys.TypeId, Value = 91 }, new WeaponIfnluence { InfluenceTypeId = infCritical.TypeId, Value = 100 } }
            });

            // 10. Dragon Tooth (Great Hammer)
            weapons.Add(new Weapon
            {
                Name = "Dragon Tooth",
                Weight = 18.0m,
                WeaponTypeId = typeGreatHammer.TypeId,
                ReqStrenght = 40,
                ReqDexterity = 0,
                IconPath = "/images/Weapons/Dragon Tooth.png",
                WeaponInfluences = new List<WeaponIfnluence> { new WeaponIfnluence { InfluenceTypeId = infPhys.TypeId, Value = 185 }, new WeaponIfnluence { InfluenceTypeId = infCritical.TypeId, Value = 100 } }
            });

            // 11. Smough's Hammer (Great Hammer)
            weapons.Add(new Weapon
            {
                Name = "Smough's Hammer",
                Weight = 28.0m,
                WeaponTypeId = typeGreatHammer.TypeId,
                ReqStrenght = 58,
                ReqDexterity = 0,
                IconPath = "/images/Weapons/Smoughs Hammer.png",
                WeaponInfluences = new List<WeaponIfnluence> { new WeaponIfnluence { InfluenceTypeId = infPhys.TypeId, Value = 300 }, new WeaponIfnluence { InfluenceTypeId = infCritical.TypeId, Value = 100 } }
            });

            // 12. Great Scythe (Halberd/Reaper)
            weapons.Add(new Weapon
            {
                Name = "Great Scythe",
                Weight = 5.0m,
                WeaponTypeId = type_Halberd.TypeId,
                ReqStrenght = 14,
                ReqDexterity = 14,
                IconPath = "/images/Weapons/Great Scythe.png",
                WeaponInfluences = new List<WeaponIfnluence> { new WeaponIfnluence { InfluenceTypeId = infPhys.TypeId, Value = 100 }, new WeaponIfnluence { InfluenceTypeId = infCritical.TypeId, Value = 100 } }
            });

            // 13. Dragonslayer Spear (Spear - Lightning)
            weapons.Add(new Weapon
            {
                Name = "Dragonslayer Spear",
                Weight = 10.0m,
                WeaponTypeId = type_Spear.TypeId,
                ReqStrenght = 24,
                ReqDexterity = 24,
                ReqFaith = 0,
                IconPath = "/images/Weapons/Dragonslayer Spear.png",
                WeaponInfluences = new List<WeaponIfnluence> { new WeaponIfnluence { InfluenceTypeId = infPhys.TypeId, Value = 90 }, new WeaponIfnluence { InfluenceTypeId = infLight.TypeId, Value = 60 }, new WeaponIfnluence { InfluenceTypeId = infCritical.TypeId, Value = 100 } }
            });

            // 14. Moonlight Butterfly Horn (Spear - Magic)
            weapons.Add(new Weapon
            {
                Name = "Moonlight Butterfly Horn",
                Weight = 4.0m,
                WeaponTypeId = type_Spear.TypeId,
                ReqStrenght = 12,
                ReqIntelligence = 0,
                IconPath = "/images/Weapons/Moonlight Butterfly Horn.png",
                WeaponInfluences = new List<WeaponIfnluence> { new WeaponIfnluence { InfluenceTypeId = infMagic.TypeId, Value = 120 }, new WeaponIfnluence { InfluenceTypeId = infCritical.TypeId, Value = 100 } }
            });

            // 15. Demon's Greataxe (Great Axe)
            weapons.Add(new Weapon
            {
                Name = "Demon's Greataxe",
                Weight = 22.0m,
                WeaponTypeId = typeGreat_Axe.TypeId,
                ReqStrenght = 46,
                ReqDexterity = 0,
                IconPath = "/images/Weapons/Demons Greataxe.png",
                WeaponInfluences = new List<WeaponIfnluence> { new WeaponIfnluence { InfluenceTypeId = infPhys.TypeId, Value = 285 }, new WeaponIfnluence { InfluenceTypeId = infCritical.TypeId, Value = 100 } }
            });

            // 16. Murakumo (Curved Greatsword)
            weapons.Add(new Weapon
            {
                Name = "Murakumo",
                Weight = 12.0m,
                WeaponTypeId = typeCurvedGreat.TypeId,
                ReqStrenght = 28,
                ReqDexterity = 13,
                IconPath = "/images/Weapons/Murakumo.png",
                WeaponInfluences = new List<WeaponIfnluence> { new WeaponIfnluence { InfluenceTypeId = infPhys.TypeId, Value = 143 }, new WeaponIfnluence { InfluenceTypeId = infCritical.TypeId, Value = 100 } }
            });

            // 17. Gold Tracer (Curved Sword)
            weapons.Add(new Weapon
            {
                Name = "Gold Tracer",
                Weight = 2.0m,
                WeaponTypeId = typeCurvedSword.TypeId,
                ReqStrenght = 9,
                ReqDexterity = 25,
                IconPath = "/images/Weapons/Gold Tracer.png",
                WeaponInfluences = new List<WeaponIfnluence> { new WeaponIfnluence { InfluenceTypeId = infPhys.TypeId, Value = 130 }, new WeaponIfnluence { InfluenceTypeId = infCritical.TypeId, Value = 100 } }
            });

            // 18. Washing Pole (Katana)
            weapons.Add(new Weapon
            {
                Name = "Washing Pole",
                Weight = 8.0m,
                WeaponTypeId = typeKatana.TypeId,
                ReqStrenght = 20,
                ReqDexterity = 16,
                IconPath = "/images/Weapons/Washing Pole.png",
                WeaponInfluences = new List<WeaponIfnluence> { new WeaponIfnluence { InfluenceTypeId = infPhys.TypeId, Value = 90 }, new WeaponIfnluence { InfluenceTypeId = infCritical.TypeId, Value = 100 } }
            });

            // 19. Chaos Blade (Katana)
            weapons.Add(new Weapon
            {
                Name = "Chaos Blade",
                Weight = 6.0m,
                WeaponTypeId = typeKatana.TypeId,
                ReqStrenght = 16,
                ReqDexterity = 14,
                IconPath = "/images/Weapons/Chaos Blade.png",
                WeaponInfluences = new List<WeaponIfnluence> { new WeaponIfnluence { InfluenceTypeId = infPhys.TypeId, Value = 144 }, new WeaponIfnluence { InfluenceTypeId = infCritical.TypeId, Value = 100 } }
            });

            // 20. Black Knight Shield (Shield)
            weapons.Add(new Weapon
            {
                Name = "Black Knight Shield",
                Weight = 6.0m,
                WeaponTypeId = typeShield.TypeId,
                ReqStrenght = 16,
                IconPath = "/images/Weapons/Black Knight Shield.jpg",
                WeaponInfluences = new List<WeaponIfnluence> { new WeaponIfnluence { InfluenceTypeId = infPhys.TypeId, Value = 74 } }
            });
            context.Weapons.AddRange(weapons);

            // --- 5. БРОНЯ (ПРОПИСУЄМО ТОЧНІ ШЛЯХИ) ---

            // Elite Knight Set
            context.Armors.Add(new Armor
            {
                Name = "Elite Knight Helm",
                Weight = 4.5m,
                Poise = 4.2m,
                ArmorTypeId = typeHelm.TypeId,
                IconPath = "/images/Armors/Elite Knight Helm.png",
                ArmorInfluences = new List<ArmorInfluence> { new ArmorInfluence { InfluenceTypeId = infPhys.TypeId, Value = 4.5 } }
            });
            context.Armors.Add(new Armor
            {
                Name = "Elite Knight Armor",
                Weight = 11.5m,
                Poise = 16.0m,
                ArmorTypeId = typeChestArmor.TypeId,
                IconPath = "/images/Armors/Elite Knight Armor.jpg",
                ArmorInfluences = new List<ArmorInfluence> { new ArmorInfluence { InfluenceTypeId = infPhys.TypeId, Value = 13.0 } }
            });
            context.Armors.Add(new Armor
            {
                Name = "Elite Knight Gauntlets",
                Weight = 3.5m,
                Poise = 3.2m,
                ArmorTypeId = typeGauntlets.TypeId,
                IconPath = "/images/Armors/Elite Knight Gauntlets.png",
                ArmorInfluences = new List<ArmorInfluence> { new ArmorInfluence { InfluenceTypeId = infPhys.TypeId, Value = 3.5 } }
            });
            context.Armors.Add(new Armor
            {
                Name = "Elite Knight Leggings",
                Weight = 7.5m,
                Poise = 8.5m,
                ArmorTypeId = typeLeggings.TypeId,
                IconPath = "/images/Armors/elite_knight_leggings.jpg", 
                ArmorInfluences = new List<ArmorInfluence> { new ArmorInfluence { InfluenceTypeId = infPhys.TypeId, Value = 7.5 } }
            });

            // Havel's Set
            context.Armors.Add(new Armor
            {
                Name = "Havel's Helm",
                Weight = 11.5m,
                Poise = 10.0m,
                ArmorTypeId = typeHelm.TypeId,
                IconPath = "/images/Armors/Havel's Helm.jpg",
                ArmorInfluences = new List<ArmorInfluence> { new ArmorInfluence { InfluenceTypeId = infPhys.TypeId, Value = 8.2 } }
            });
            context.Armors.Add(new Armor
            {
                Name = "Havel's Armor",
                Weight = 23.5m,
                Poise = 40.0m,
                ArmorTypeId = typeChestArmor.TypeId,
                IconPath = "/images/Armors/Havel's Armor.jpg",
                ArmorInfluences = new List<ArmorInfluence> { new ArmorInfluence { InfluenceTypeId = infPhys.TypeId, Value = 20.5 } }
            });
            context.Armors.Add(new Armor
            {
                Name = "Havel's Gauntlets",
                Weight = 11.5m,
                Poise = 9.0m,
                ArmorTypeId = typeGauntlets.TypeId,
                IconPath = "/images/Armors/Havel's Gauntlets.jpg",
                ArmorInfluences = new List<ArmorInfluence> { new ArmorInfluence { InfluenceTypeId = infPhys.TypeId, Value = 7.2 } }
            });
            context.Armors.Add(new Armor
            {
                Name = "Havel's Leggings",
                Weight = 13.5m,
                Poise = 20.0m,
                ArmorTypeId = typeLeggings.TypeId,
                IconPath = "/images/Armors/Havel's Leggings.jpg",
                ArmorInfluences = new List<ArmorInfluence> { new ArmorInfluence { InfluenceTypeId = infPhys.TypeId, Value = 12.0 }, new ArmorInfluence { InfluenceTypeId = infMagic.TypeId, Value = 14.0 } }
            });

            // Pyromancer Set
            context.Armors.Add(new Armor
            {
                Name = "Pyromancer Hood",
                Weight = 1.5m,
                Poise = 0.0m,
                ArmorTypeId = typeHelm.TypeId,
                IconPath = "/images/Armors/Pyromancer Hood.png",
                ArmorInfluences = new List<ArmorInfluence> { new ArmorInfluence { InfluenceTypeId = infFire.TypeId, Value = 5.0 } }
            });
            context.Armors.Add(new Armor
            {
                Name = "Pyromancer Garb",
                Weight = 3.2m,
                Poise = 0.0m,
                ArmorTypeId = typeChestArmor.TypeId,
                IconPath = "/images/Armors/Pyromancer Garb.png",
                ArmorInfluences = new List<ArmorInfluence> { new ArmorInfluence { InfluenceTypeId = infFire.TypeId, Value = 10.0 } }
            });
            context.Armors.Add(new Armor
            {
                Name = "Pyromancer Gauntlets",
                Weight = 1.2m,
                Poise = 0.0m,
                ArmorTypeId = typeGauntlets.TypeId,
                IconPath = "/images/Armors/Pyromancer Gauntlets.png",
                ArmorInfluences = new List<ArmorInfluence> { new ArmorInfluence { InfluenceTypeId = infFire.TypeId, Value = 2.0 } }
            });
            context.Armors.Add(new Armor
            {
                Name = "Pyromancer Leggings",
                Weight = 2.0m,
                Poise = 0.0m,
                ArmorTypeId = typeLeggings.TypeId,
                IconPath = "/images/Armors/Pyromancer Leggings.png",
                ArmorInfluences = new List<ArmorInfluence> { new ArmorInfluence { InfluenceTypeId = infFire.TypeId, Value = 4.0 } }
            });
            // === 10 НОВИХ СЕТІВ БРОНІ ===

            // 1. Black Iron Set (Tarkus) - High Phys/Fire
            context.Armors.Add(new Armor 
            { 
                Name = "Black Iron Helm", 
                Weight = 6.0m, Poise = 6.0m, 
                ArmorTypeId = typeHelm.TypeId, 
                IconPath = "/images/Armors/Black Iron Helm.png", 
                ArmorInfluences = new List<ArmorInfluence> { new ArmorInfluence { InfluenceTypeId = infPhys.TypeId, Value = 6.8 }, new ArmorInfluence { InfluenceTypeId = infFire.TypeId, Value = 4.0 } }
            });

            context.Armors.Add(new Armor 
            { 
                Name = "Black Iron Armor", 
                Weight = 15.6m, Poise = 19.0m, 
                ArmorTypeId = typeChestArmor.TypeId, 
                IconPath = "/images/Armors/Black Iron Armor.png", 
                ArmorInfluences = new List<ArmorInfluence> { new ArmorInfluence { InfluenceTypeId = infPhys.TypeId, Value = 17.5 }, new ArmorInfluence { InfluenceTypeId = infFire.TypeId, Value = 9.0 } } 
            });
            context.Armors.Add(new Armor 
            { 
                Name = "Black Iron Gauntlets", 
                Weight = 5.2m, Poise = 5.0m, 
                ArmorTypeId = typeGauntlets.TypeId, 
                IconPath = "/images/Armors/Black Iron Gauntlets.png", 
                ArmorInfluences = new List<ArmorInfluence> { new ArmorInfluence { InfluenceTypeId = infPhys.TypeId, Value = 4.9 }, new ArmorInfluence { InfluenceTypeId = infFire.TypeId, Value = 2.0 } } 
            });
            context.Armors.Add(new Armor 
            { 
                Name = "Black Iron Leggings", 
                Weight = 9.2m, Poise = 10.0m, 
                ArmorTypeId = typeLeggings.TypeId, 
                IconPath = "/images/Armors/Black Iron Leggings.png", 
                ArmorInfluences = new List<ArmorInfluence> { new ArmorInfluence { InfluenceTypeId = infPhys.TypeId, Value = 9.5 }, new ArmorInfluence { InfluenceTypeId = infFire.TypeId, Value = 5.0 } } 
            });

            // 2. Gold-Hemmed Black Set (Ceaseless) - Light/Fire/Poison
            context.Armors.Add(new Armor { Name = "Gold-Hemmed Hood", Weight = 1.4m, Poise = 0.0m, ArmorTypeId = typeHelm.TypeId, IconPath = "/images/Armors/Gold-Hemmed Hood.png", ArmorInfluences = new List<ArmorInfluence> { new ArmorInfluence { InfluenceTypeId = infFire.TypeId, Value = 5.0 } } });
            context.Armors.Add(new Armor { Name = "Gold-Hemmed Cloak", Weight = 3.0m, Poise = 0.0m, ArmorTypeId = typeChestArmor.TypeId, IconPath = "/images/Armors/Gold-Hemmed Cloak.png", ArmorInfluences = new List<ArmorInfluence> { new ArmorInfluence { InfluenceTypeId = infFire.TypeId, Value = 12.0 } } });
            context.Armors.Add(new Armor { Name = "Gold-Hemmed Gloves", Weight = 1.1m, Poise = 0.0m, ArmorTypeId = typeGauntlets.TypeId, IconPath = "/images/Armors/Gold-Hemmed Gloves.png", ArmorInfluences = new List<ArmorInfluence> { new ArmorInfluence { InfluenceTypeId = infFire.TypeId, Value = 3.0 } } });
            context.Armors.Add(new Armor { Name = "Gold-Hemmed Skirt", Weight = 1.9m, Poise = 0.0m, ArmorTypeId = typeLeggings.TypeId, IconPath = "/images/Armors/Gold-Hemmed Skirt.png", ArmorInfluences = new List<ArmorInfluence> { new ArmorInfluence { InfluenceTypeId = infFire.TypeId, Value = 7.0 } } });

            // 3. Catarina Set (Onion)
            context.Armors.Add(new Armor { Name = "Catarina Helm", Weight = 6.2m, Poise = 6.0m, ArmorTypeId = typeHelm.TypeId, IconPath = "/images/Armors/Catarina Helm.png", ArmorInfluences = new List<ArmorInfluence> { new ArmorInfluence { InfluenceTypeId = infPhys.TypeId, Value = 6.0 } } });
            context.Armors.Add(new Armor { Name = "Catarina Armor", Weight = 16.7m, Poise = 17.0m, ArmorTypeId = typeChestArmor.TypeId, IconPath = "/images/Armors/Catarina Armor.png", ArmorInfluences = new List<ArmorInfluence> { new ArmorInfluence { InfluenceTypeId = infPhys.TypeId, Value = 15.0 } } });
            context.Armors.Add(new Armor { Name = "Catarina Gauntlets", Weight = 5.3m, Poise = 5.0m, ArmorTypeId = typeGauntlets.TypeId, IconPath = "/images/Armors/Catarina Gauntlets.png", ArmorInfluences = new List<ArmorInfluence> { new ArmorInfluence { InfluenceTypeId = infPhys.TypeId, Value = 5.0 } } });
            context.Armors.Add(new Armor { Name = "Catarina Leggings", Weight = 9.8m, Poise = 10.0m, ArmorTypeId = typeLeggings.TypeId, IconPath = "/images/Armors/Catarina Leggings.png", ArmorInfluences = new List<ArmorInfluence> { new ArmorInfluence { InfluenceTypeId = infPhys.TypeId, Value = 9.0 } } });

            // 4. Silver Knight Set
            context.Armors.Add(new Armor { Name = "Silver Knight Helm", Weight = 4.2m, Poise = 5.0m, ArmorTypeId = typeHelm.TypeId, IconPath = "/images/Armors/Silver Knight Helm.png", ArmorInfluences = new List<ArmorInfluence> { new ArmorInfluence { InfluenceTypeId = infPhys.TypeId, Value = 5.5 } } });
            context.Armors.Add(new Armor { Name = "Silver Knight Armor", Weight = 10.8m, Poise = 14.0m, ArmorTypeId = typeChestArmor.TypeId, IconPath = "/images/Armors/Silver Knight Armor.png", ArmorInfluences = new List<ArmorInfluence> { new ArmorInfluence { InfluenceTypeId = infPhys.TypeId, Value = 12.0 } } });
            context.Armors.Add(new Armor { Name = "Silver Knight Gauntlets", Weight = 3.6m, Poise = 4.0m, ArmorTypeId = typeGauntlets.TypeId, IconPath = "/images/Armors/Silver Knight Gauntlets.png", ArmorInfluences = new List<ArmorInfluence> { new ArmorInfluence { InfluenceTypeId = infPhys.TypeId, Value = 4.0 } } });
            context.Armors.Add(new Armor { Name = "Silver Knight Leggings", Weight = 6.4m, Poise = 8.0m, ArmorTypeId = typeLeggings.TypeId, IconPath = "/images/Armors/Silver Knight Leggings.png", ArmorInfluences = new List<ArmorInfluence> { new ArmorInfluence { InfluenceTypeId = infPhys.TypeId, Value = 7.0 } } });

            // 5. Black Knight Set
            context.Armors.Add(new Armor { Name = "Black Knight Helm", Weight = 4.5m, Poise = 5.0m, ArmorTypeId = typeHelm.TypeId, IconPath = "/images/Armors/Black Knight Helm.png", ArmorInfluences = new List<ArmorInfluence> { new ArmorInfluence { InfluenceTypeId = infFire.TypeId, Value = 4.0 } } });
            context.Armors.Add(new Armor { Name = "Black Knight Armor", Weight = 11.5m, Poise = 14.0m, ArmorTypeId = typeChestArmor.TypeId, IconPath = "/images/Armors/Black Knight Armor.png", ArmorInfluences = new List<ArmorInfluence> { new ArmorInfluence { InfluenceTypeId = infFire.TypeId, Value = 10.0 } } });
            context.Armors.Add(new Armor { Name = "Black Knight Gauntlets", Weight = 3.8m, Poise = 4.0m, ArmorTypeId = typeGauntlets.TypeId, IconPath = "/images/Armors/Black Knight Gauntlets.png", ArmorInfluences = new List<ArmorInfluence> { new ArmorInfluence { InfluenceTypeId = infFire.TypeId, Value = 3.0 } } });
            context.Armors.Add(new Armor { Name = "Black Knight Leggings", Weight = 6.8m, Poise = 8.0m, ArmorTypeId = typeLeggings.TypeId, IconPath = "/images/Armors/Black Knight Leggings.png", ArmorInfluences = new List<ArmorInfluence> { new ArmorInfluence { InfluenceTypeId = infFire.TypeId, Value = 6.0 } } });

            // 6. Ornstein's Set (Dragonslayer)
            context.Armors.Add(new Armor { Name = "Ornstein's Helm", Weight = 5.5m, Poise = 5.0m, ArmorTypeId = typeHelm.TypeId, IconPath = "/images/Armors/Ornsteins Helm.png", ArmorInfluences = new List<ArmorInfluence> { new ArmorInfluence { InfluenceTypeId = infLight.TypeId, Value = 6.0 } } });
            context.Armors.Add(new Armor { Name = "Ornstein's Armor", Weight = 12.0m, Poise = 15.0m, ArmorTypeId = typeChestArmor.TypeId, IconPath = "/images/Armors/Ornsteins Armor.png", ArmorInfluences = new List<ArmorInfluence> { new ArmorInfluence { InfluenceTypeId = infLight.TypeId, Value = 13.0 } } });
            context.Armors.Add(new Armor { Name = "Ornstein's Gauntlets", Weight = 4.0m, Poise = 4.0m, ArmorTypeId = typeGauntlets.TypeId, IconPath = "/images/Armors/Ornsteins Gauntlets.png", ArmorInfluences = new List<ArmorInfluence> { new ArmorInfluence { InfluenceTypeId = infLight.TypeId, Value = 4.0 } } });
            context.Armors.Add(new Armor { Name = "Ornstein's Leggings", Weight = 8.0m, Poise = 9.0m, ArmorTypeId = typeLeggings.TypeId, IconPath = "/images/Armors/Ornsteins Leggings.png", ArmorInfluences = new List<ArmorInfluence> { new ArmorInfluence { InfluenceTypeId = infLight.TypeId, Value = 8.0 } } });

            // 7. Smough's Set (Executioner) - Super Heavy
            context.Armors.Add(new Armor { Name = "Smough's Helm", Weight = 13.0m, Poise = 11.0m, ArmorTypeId = typeHelm.TypeId, IconPath = "/images/Armors/Smoughs Helm.png", ArmorInfluences = new List<ArmorInfluence> { new ArmorInfluence { InfluenceTypeId = infPhys.TypeId, Value = 9.0 } } });
            context.Armors.Add(new Armor { Name = "Smough's Armor", Weight = 26.0m, Poise = 42.0m, ArmorTypeId = typeChestArmor.TypeId, IconPath = "/images/Armors/Smoughs Armor.png", ArmorInfluences = new List<ArmorInfluence> { new ArmorInfluence { InfluenceTypeId = infPhys.TypeId, Value = 22.0 } } });
            context.Armors.Add(new Armor { Name = "Smough's Gauntlets", Weight = 12.0m, Poise = 10.0m, ArmorTypeId = typeGauntlets.TypeId, IconPath = "/images/Armors/Smoughs Gauntlets.png", ArmorInfluences = new List<ArmorInfluence> { new ArmorInfluence { InfluenceTypeId = infPhys.TypeId, Value = 8.0 } } });
            context.Armors.Add(new Armor { Name = "Smough's Leggings", Weight = 14.5m, Poise = 21.0m, ArmorTypeId = typeLeggings.TypeId, IconPath = "/images/Armors/Smoughs Leggings.png", ArmorInfluences = new List<ArmorInfluence> { new ArmorInfluence { InfluenceTypeId = infPhys.TypeId, Value = 13.0 } } });

            // 8. Artorias' Set (Wolf Knight)
            context.Armors.Add(new Armor { Name = "Helm of Artorias", Weight = 4.2m, Poise = 4.0m, ArmorTypeId = typeHelm.TypeId, IconPath = "/images/Armors/Artorias Helm.png", ArmorInfluences = new List<ArmorInfluence> { new ArmorInfluence { InfluenceTypeId = infPhys.TypeId, Value = 5.0 } } });
            context.Armors.Add(new Armor { Name = "Armor of Artorias", Weight = 10.9m, Poise = 13.0m, ArmorTypeId = typeChestArmor.TypeId, IconPath = "/images/Armors/Artorias Armor.png", ArmorInfluences = new List<ArmorInfluence> { new ArmorInfluence { InfluenceTypeId = infPhys.TypeId, Value = 11.0 } } });
            context.Armors.Add(new Armor { Name = "Gauntlets of Artorias", Weight = 3.6m, Poise = 3.0m, ArmorTypeId = typeGauntlets.TypeId, IconPath = "/images/Armors/Artorias Gauntlets.png", ArmorInfluences = new List<ArmorInfluence> { new ArmorInfluence { InfluenceTypeId = infPhys.TypeId, Value = 3.5 } } });
            context.Armors.Add(new Armor { Name = "Leggings of Artorias", Weight = 6.3m, Poise = 7.0m, ArmorTypeId = typeLeggings.TypeId, IconPath = "/images/Armors/Artorias Leggings.png", ArmorInfluences = new List<ArmorInfluence> { new ArmorInfluence { InfluenceTypeId = infPhys.TypeId, Value = 6.5 } } });

            // 9. Crimson Set (Sealer)
            context.Armors.Add(new Armor { Name = "Mask of the Sealer", Weight = 1.0m, Poise = 0.0m, ArmorTypeId = typeHelm.TypeId, IconPath = "/images/Armors/Mask of the Sealer.png", ArmorInfluences = new List<ArmorInfluence> { new ArmorInfluence { InfluenceTypeId = infMagic.TypeId, Value = 4.0 } } });
            context.Armors.Add(new Armor { Name = "Crimson Robe", Weight = 2.4m, Poise = 0.0m, ArmorTypeId = typeChestArmor.TypeId, IconPath = "/images/Armors/Crimson Robe.png", ArmorInfluences = new List<ArmorInfluence> { new ArmorInfluence { InfluenceTypeId = infMagic.TypeId, Value = 9.0 } } });
            context.Armors.Add(new Armor { Name = "Crimson Gloves", Weight = 0.9m, Poise = 0.0m, ArmorTypeId = typeGauntlets.TypeId, IconPath = "/images/Armors/Crimson Gloves.png", ArmorInfluences = new List<ArmorInfluence> { new ArmorInfluence { InfluenceTypeId = infMagic.TypeId, Value = 2.0 } } });
            context.Armors.Add(new Armor { Name = "Crimson Waistcloth", Weight = 1.5m, Poise = 0.0m, ArmorTypeId = typeLeggings.TypeId, IconPath = "/images/Armors/Crimson Waistcloth.png", ArmorInfluences = new List<ArmorInfluence> { new ArmorInfluence { InfluenceTypeId = infMagic.TypeId, Value = 3.5 } } });

            // 10. Giant Set
            context.Armors.Add(new Armor { Name = "Giant Helm", Weight = 6.8m, Poise = 7.0m, ArmorTypeId = typeHelm.TypeId, IconPath = "/images/Armors/Giant Helm.png", ArmorInfluences = new List<ArmorInfluence> { new ArmorInfluence { InfluenceTypeId = infPhys.TypeId, Value = 6.5 } } });
            context.Armors.Add(new Armor { Name = "Giant Armor", Weight = 16.4m, Poise = 21.0m, ArmorTypeId = typeChestArmor.TypeId, IconPath = "/images/Armors/Giant Armor.png", ArmorInfluences = new List<ArmorInfluence> { new ArmorInfluence { InfluenceTypeId = infPhys.TypeId, Value = 16.0 } } });
            context.Armors.Add(new Armor { Name = "Giant Gauntlets", Weight = 5.9m, Poise = 6.0m, ArmorTypeId = typeGauntlets.TypeId, IconPath = "/images/Armors/Giant Gauntlets.png", ArmorInfluences = new List<ArmorInfluence> { new ArmorInfluence { InfluenceTypeId = infPhys.TypeId, Value = 5.0 } } });
            context.Armors.Add(new Armor { Name = "Giant Leggings", Weight = 9.7m, Poise = 11.0m, ArmorTypeId = typeLeggings.TypeId, IconPath = "/images/Armors/Giant Leggings.png", ArmorInfluences = new List<ArmorInfluence> { new ArmorInfluence { InfluenceTypeId = infPhys.TypeId, Value = 9.0 } } });
            context.SaveChanges();
        }
    }
}