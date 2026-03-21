using System.Collections.Generic;
using System.Linq;
using Models_Context.Context;
using Models_Context.Models;
using Repositories.Interfaces;

namespace Repositories.Implementations
{
    public class CharacterRepository : ICharacterRepository
    {
        private readonly RPGDarkSoulsDbContext _context;

        public CharacterRepository(RPGDarkSoulsDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Character> GetAll()
        {
            // Беремо дані з таблиці Characters
            return _context.Characters.ToList();
        }

        public Character GetById(int id)
        {
            // Шукаємо по CharacterId
            return _context.Characters.FirstOrDefault(c => c.CharacterId == id);
        }

        public void Add(Character character)
        {
            _context.Characters.Add(character);
            _context.SaveChanges();
        }

        public void Update(Character character)
        {
            _context.Characters.Update(character);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var entity = GetById(id);
            if (entity != null)
            {
                _context.Characters.Remove(entity);
                _context.SaveChanges();
            }
        }
    }
}