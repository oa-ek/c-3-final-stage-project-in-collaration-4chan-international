using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Models_Context.Context;
using Models_Context.Models;
using Repositories.Interfaces;

namespace Repositories.Implementations
{
    public class CharacterBuildRepository : ICharacterBuildRepository
    {
        private readonly RPGDarkSoulsDbContext _context;

        public CharacterBuildRepository(RPGDarkSoulsDbContext context)
        {
            _context = context;
        }

        public IEnumerable<CharacterBuild> GetAll()
        {
            // AsNoTracking щоб ми могти потім вільно редагувати ці об'єкти
            return _context.CharacterBuilds.AsNoTracking().ToList();
        }

        public CharacterBuild GetById(int id)
        {
            return _context.CharacterBuilds.AsNoTracking().FirstOrDefault(b => b.Id == id);
        }

        public void Add(CharacterBuild build)
        {
            _context.CharacterBuilds.Add(build);
            _context.SaveChanges();
        }

        public void Update(CharacterBuild build)
        {
            // Шукаю "живу" сутність у контексті бази даних
            var existingBuild = _context.CharacterBuilds.FirstOrDefault(b => b.Id == build.Id);

            if (existingBuild != null)
            {
                // Вручну переношу значення з форми (build) в об'єкт бази даних (existingBuild)

                
                existingBuild.Name = build.Name;

                
                existingBuild.Vigor = build.Vigor;
                existingBuild.Endurance = build.Endurance;
                existingBuild.Strength = build.Strength;
                existingBuild.Dexterity = build.Dexterity;
                existingBuild.Intelligence = build.Intelligence;
                existingBuild.Faith = build.Faith;

                
                existingBuild.RightHandId = build.RightHandId;
                existingBuild.LeftHandId = build.LeftHandId;
                existingBuild.HeadId = build.HeadId;
                existingBuild.ChestId = build.ChestId;
                existingBuild.HandsId = build.HandsId;
                existingBuild.LegsId = build.LegsId;

                
                _context.SaveChanges();
            }
        }

        public void Delete(int id)
        {
            var entity = _context.CharacterBuilds.Find(id);
            if (entity != null)
            {
                _context.CharacterBuilds.Remove(entity);
                _context.SaveChanges();
            }
        }
    }
}