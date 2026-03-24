// using Microsoft.EntityFrameworkCore;
// using Models_Context.Context;
// using Models_Context.Models;
// using Repositories.Interfaces;
// using System.Collections.Generic;
// using System.Linq;
//
// namespace Repositories.Implementations
// {
//     public class ArmorRepository : Repository<Armor>, IArmorRepository
//     {
//         public ArmorRepository(RPGDarkSoulsDbContext context) : base(context)
//         {
//         }
//
//         public override IEnumerable<Armor> GetAll()
//         {
//             return _context.Armors
//                 .Include(a => a.ArmorType)
//                 .Include(a => a.ArmorInfluences)
//                     .ThenInclude(ai => ai.Influence) // <--- БУЛО InfluenceType, СТАЛО Influence
//                 .ToList();
//         }
//
//         public override Armor? GetById(int id)
//         {
//             return _context.Armors
//                 .Include(a => a.ArmorType)
//                 .Include(a => a.ArmorInfluences)
//                     .ThenInclude(ai => ai.Influence) // <--- БУЛО InfluenceType, СТАЛО Influence
//                 .FirstOrDefault(a => a.ArmorId == id);
//         }
//
//         public IEnumerable<Armor> GetBySlotId(int slotId)
//         {
//             return _context.Armors
//                 .Where(a => a.ArmorType != null && a.ArmorType.SlotId == slotId)
//                 .Include(a => a.ArmorType)
//                 .Include(a => a.ArmorInfluences)
//                     .ThenInclude(ai => ai.Influence) // <--- БУЛО InfluenceType, СТАЛО Influence
//                 .ToList();
//         }
//
//         public IEnumerable<Armor> GetByArmorTypeName(string typeName)
//         {
//             return _context.Armors
//                .Where(a => a.ArmorType != null && a.ArmorType.Type == typeName)
//                .Include(a => a.ArmorType)
//                .Include(a => a.ArmorInfluences)
//                    .ThenInclude(ai => ai.Influence) // <--- БУЛО InfluenceType, СТАЛО Influence
//                .ToList();
//         }
//     }
// }