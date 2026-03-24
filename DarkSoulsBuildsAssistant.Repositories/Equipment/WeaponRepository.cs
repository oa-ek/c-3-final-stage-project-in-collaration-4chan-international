using Models_Context.Context;
using Models_Context.Models;
using Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Repositories.Implementations
{
    public class WeaponRepository : Repository<Weapon>, IWeaponRepository
    {
        public WeaponRepository(RPGDarkSoulsDbContext context) : base(context)
        {
        }

        // Перевизначаємо GetAll для повного завантаження даних
        public override IEnumerable<Weapon> GetAll()
        {
            return _context.Weapons
                // Завантажуємо тип зброї (WeaponType)
                .Include(w => w.WeaponType)
                // Завантажуємо характеристики (WeaponInfluences)
                .Include(w => w.WeaponInfluences)
                    // Всередині характеристики завантажуємо її тип (InfluenceType)
                    .ThenInclude(wi => wi.InfluenceType)
                .ToList();
        }

        // Перевизначаємо GetById
        public override Weapon? GetById(int id)
        {
            return _context.Weapons
                .Include(w => w.WeaponType)
                .Include(w => w.WeaponInfluences)
                    .ThenInclude(wi => wi.InfluenceType)
                .FirstOrDefault(w => w.WeaponId == id);
        }

        // Пошук за ID типу зброї
        public IEnumerable<Weapon> GetByWeaponTypeId(int typeId)
        {
            return _context.Weapons
                .Where(w => w.WeaponTypeId == typeId)
                .Include(w => w.WeaponType)
                .Include(w => w.WeaponInfluences)
                    .ThenInclude(wi => wi.InfluenceType)
                .ToList();
        }

        // Пошук за назвою типу зброї
        public IEnumerable<Weapon> GetByWeaponTypeName(string typeName)
        {
            return _context.Weapons
                // Перевіряємо на null, щоб уникнути помилок, якщо тип не вказано
                .Where(w => w.WeaponType != null && w.WeaponType.Type == typeName)
                .Include(w => w.WeaponType)
                .Include(w => w.WeaponInfluences)
                    .ThenInclude(wi => wi.InfluenceType)
                .ToList();
        }
    }
}
