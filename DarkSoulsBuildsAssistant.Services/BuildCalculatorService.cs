using Repositories.Interfaces;
using Services.DataTransferObj; // Перевір, чи папка називається DTO чи DataTransferObj
using Services.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace Services.Implementations
{
    public class BuildCalculatorService : IBuildCalculatorService
    {
        private readonly IWeaponRepository _weaponRepo;
        private readonly IArmorRepository _armorRepo;

        public BuildCalculatorService(IWeaponRepository weaponRepo, IArmorRepository armorRepo)
        {
            _weaponRepo = weaponRepo;
            _armorRepo = armorRepo;
        }

        public BuildStats CalculateStats(int vigor, int endurance, int mind, List<int> weaponIds, List<int> armorIds)
        {
            var stats = new BuildStats();
            decimal currentWeight = 0;

            // 1. ЗБРОЯ (Рахуємо вагу та АТАКУ)
            if (weaponIds != null)
            {
                foreach (var id in weaponIds)
                {
                    if (id <= 0) continue;
                    var weapon = _weaponRepo.GetById(id);
                    if (weapon == null) continue;

                    if (weapon.Weight.HasValue) currentWeight += weapon.Weight.Value;

                    // ЗБРОЯ використовує InfluenceType (залежно від твоєї моделі WeaponIfnluence)
                    if (weapon.WeaponInfluences != null)
                    {
                        foreach (var inf in weapon.WeaponInfluences)
                        {
                            // УВАГА: Тут ми використовуємо InfluenceType
                            var typeName = inf.InfluenceType?.Name;
                            var val = (int)(inf.Value ?? 0);

                            if (typeName == "Physical") stats.AttackPhysical += val;
                            else if (typeName == "Magic") stats.AttackMagic += val;
                            else if (typeName == "Fire") stats.AttackFire += val;
                            else if (typeName == "Lightning") stats.AttackLightning += val;
                            else if (typeName == "Critical") stats.RWeaponCritical += val;

                        }
                    }
                }
            }

            // 2. БРОНЯ (Рахуємо вагу та ЗАХИСТ)
            if (armorIds != null)
            {
                foreach (var id in armorIds)
                {
                    if (id <= 0) continue;
                    var armor = _armorRepo.GetById(id);
                    if (armor == null) continue;

                    if (armor.Weight.HasValue) currentWeight += armor.Weight.Value;
                    if (armor.Poise.HasValue) stats.Poise += armor.Poise.Value;

                    // БРОНЯ використовує Influence (без слова Type, згідно з твоїм файлом ArmorInfluence.cs)
                    if (armor.ArmorInfluences != null)
                    {
                        foreach (var inf in armor.ArmorInfluences)
                        {
                            // УВАГА: Тут ми використовуємо Influence (бо так названо в моделі)
                            var typeName = inf.Influence?.Name;
                            var val = inf.Value ?? 0;

                            if (typeName == "Physical") stats.DefensePhysical += val;
                            else if (typeName == "Magic") stats.DefenseMagic += val;
                            else if (typeName == "Fire") stats.DefenseFire += val;
                            else if (typeName == "Lightning") stats.DefenseLightning += val;

                        }
                    }
                }
            }

            // 3. БАЗОВІ СТАТИ
            stats.TotalWeight = currentWeight;
            stats.HP = (int)(300 + vigor * 20);
            stats.Stamina = (int)(80 + endurance * 2);
            stats.FocusPoints = (int)(50 + mind * 3);
            stats.MaxEquipLoad = 40 + (endurance * 1.5m);

            // 4. ROLL TYPE
            if (stats.MaxEquipLoad > 0)
                stats.LoadPercentage = (stats.TotalWeight / stats.MaxEquipLoad) * 100;
            else
                stats.LoadPercentage = 100;

            if (stats.LoadPercentage < 30.0m) stats.RollType = "Fast Roll";
            else if (stats.LoadPercentage < 70.0m) stats.RollType = "Mid Roll";
            else if (stats.LoadPercentage <= 100.0m) stats.RollType = "Fat Roll";
            else stats.RollType = "Overloaded";

            return stats;
        }
    }
}