using Models_Context.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface ICatalogService
    {
        // Отримати всі типи зброї для випадаючого списку (Daggers, Greatswords...)
        // Тобі доведеться додати репозиторій для WeaponType, якщо його немає, 
        // або використовувати Generic Repository.

        IEnumerable<Weapon> GetAllWeapons();
        IEnumerable<Weapon> FilterWeaponsByType(int typeId);

        IEnumerable<Armor> GetAllArmor();
        IEnumerable<Armor> GetArmorBySlot(int slotId); // 1-Head, 2-Chest...
    }
}
