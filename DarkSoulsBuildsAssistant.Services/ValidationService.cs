using Repositories.Interfaces;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Implementations
{
    public class ValidationService : IValidationService
    {
        private readonly IWeaponRepository _weaponRepo;

        public ValidationService(IWeaponRepository weaponRepo)
        {
            _weaponRepo = weaponRepo;
        }

        public List<string> ValidateEquipmentRequirements(int strength, int dex, int intel, int faith, List<int> weaponIds)
        {
            var errors = new List<string>();

            foreach (var id in weaponIds)
            {
                if (id <= 0) continue;
                var weapon = _weaponRepo.GetById(id);
                if (weapon == null) continue;

                if (strength < (weapon.ReqStrenght ?? 0))
                    errors.Add($"Not enough Strength for {weapon.Name}. Required: {weapon.ReqStrenght}");

                if (dex < (weapon.ReqDexterity ?? 0))
                    errors.Add($"Not enough Dexterity for {weapon.Name}. Required: {weapon.ReqDexterity}");

                if (intel < (weapon.ReqIntelligence ?? 0))
                    errors.Add($"Not enough Intelligence for {weapon.Name}. Required: {weapon.ReqIntelligence}");

                if (faith < (weapon.ReqFaith ?? 0))
                    errors.Add($"Not enough Faith for {weapon.Name}. Required: {weapon.ReqFaith}");
            }

            return errors;
        }
    }
}
