using System.Collections.Generic;
using Models_Context.Models;

namespace Services.Interfaces
{
    public interface ICharacterService
    {
        IEnumerable<Character> GetAllCharacters();

        // Ось цього методу не вистачало
        void SaveCharacter(Character character);
    }
}