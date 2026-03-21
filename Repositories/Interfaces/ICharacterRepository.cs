using System.Collections.Generic;
using Models_Context.Models;

namespace Repositories.Interfaces
{
    public interface ICharacterRepository
    {
        // Всі методи тепер приймають і повертають Character
        IEnumerable<Character> GetAll();
        Character GetById(int id);
        void Add(Character character);
        void Update(Character character);
        void Delete(int id);
    }
}