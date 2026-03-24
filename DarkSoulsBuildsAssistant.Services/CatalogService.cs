using Models_Context.Models;
using Repositories.Interfaces;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Implementations
{
    public class CatalogService : ICatalogService
    {
        private readonly IWeaponRepository _weaponRepo;
        private readonly IArmorRepository _armorRepo;

        public CatalogService(IWeaponRepository weaponRepo, IArmorRepository armorRepo)
        {
            _weaponRepo = weaponRepo;
            _armorRepo = armorRepo;
        }

        public IEnumerable<Weapon> GetAllWeapons()
        {
            // Можна додати сортування за назвою за замовчуванням
            return _weaponRepo.GetAll().OrderBy(w => w.Name);
        }

        public IEnumerable<Weapon> FilterWeaponsByType(int typeId)
        {
            return _weaponRepo.GetByWeaponTypeId(typeId);
        }

        public IEnumerable<Armor> GetAllArmor()
        {
            return _armorRepo.GetAll();
        }

        public IEnumerable<Armor> GetArmorBySlot(int slotId)
        {
            // Викликаємо метод, який ми раніше створили в ArmorRepository
            return _armorRepo.GetBySlotId(slotId);
        }
    }
}
