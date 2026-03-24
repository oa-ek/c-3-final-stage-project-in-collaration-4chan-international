using System.Collections.Generic;
using Models_Context.Models;

namespace Services.Interfaces
{
    public interface ICharacterBuildService
    {
        IEnumerable<CharacterBuild> GetAllBuilds();
        void SaveBuild(CharacterBuild build);
        void DeleteBuild(int id);
    }
}