using Models_Context.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Interfaces
{
    public interface IWeaponRepository : IRepository<Weapon>
    {
        IEnumerable<Weapon> GetByWeaponTypeId(int typeId);

        // Знайти зброю за назвою типу (наприклад, "Dagger")
        IEnumerable<Weapon> GetByWeaponTypeName(string typeName);
    }
}
