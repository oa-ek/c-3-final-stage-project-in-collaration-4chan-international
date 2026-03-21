using System.Collections.Generic;
using Models_Context.Models;

namespace Repositories.Interfaces
{
    public interface ICharacterBuildRepository
    {
        IEnumerable<CharacterBuild> GetAll();
        CharacterBuild GetById(int id);
        void Add(CharacterBuild build);
        void Update(CharacterBuild build);
        void Delete(int id);
    }
}