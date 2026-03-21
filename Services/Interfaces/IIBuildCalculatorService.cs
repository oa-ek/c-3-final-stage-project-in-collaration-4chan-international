using Services.DataTransferObj;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IBuildCalculatorService
    {
        /// <summary>
        /// Розраховує параметри білда.
        /// </summary>
        /// <param name="endurance">Рівень витривалості (Endurance)</param>
        /// <param name="weaponIds">Список ID вибраної зброї</param>
        /// <param name="armorIds">Список ID вибраної броні</param>
        /// <returns>Готовий об'єкт зі статистикою (вага, тип перекату)</returns>
        BuildStats CalculateStats(int vigor, int endurance, int mind, List<int> weaponIds, List<int> armorIds);
    }
}
