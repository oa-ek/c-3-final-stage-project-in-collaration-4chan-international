// using System.Collections.Generic;
// using Models_Context.Models;
// using Repositories.Interfaces;
// using Services.Interfaces;
//
// namespace Services.Implementations
// {
//     public class CharacterService : ICharacterService
//     {
//         private readonly ICharacterRepository _repo;
//
//         public CharacterService(ICharacterRepository repo)
//         {
//             _repo = repo;
//         }
//
//         public IEnumerable<Character> GetAllCharacters()
//         {
//             return _repo.GetAll();
//         }
//
//         public void SaveCharacter(Character character)
//         {
//             // Перевірка: 0 означає, що це новий персонаж
//             if (character.CharacterId == 0)
//             {
//                 _repo.Add(character);
//             }
//             else
//             {
//                 _repo.Update(character);
//             }
//         }
//     }
// }