using Models_Context.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Interfaces
{
    public interface IArmorRepository : IRepository<Armor>
    {
        // Отримати броню для конкретного слоту (наприклад, тільки шоломи)
        IEnumerable<Armor> GetBySlotId(int slotId);

        // Отримати за назвою типу (наприклад, "Heavy Armor")
        IEnumerable<Armor> GetByArmorTypeName(string typeName);
    }
}
