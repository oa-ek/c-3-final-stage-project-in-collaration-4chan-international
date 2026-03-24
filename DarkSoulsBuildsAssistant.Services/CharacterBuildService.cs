// using System.Collections.Generic;
// using Models_Context.Models;
// using Repositories.Interfaces;
// using Services.Interfaces;
//
// namespace Services.Implementations
// {
//     public class CharacterBuildService : ICharacterBuildService
//     {
//         private readonly ICharacterBuildRepository _repo;
//
//         public void DeleteBuild(int id)
//         {
//             _repo.Delete(id);
//         }
//         public CharacterBuildService(ICharacterBuildRepository repo)
//         {
//             _repo = repo;
//         }
//
//         public IEnumerable<CharacterBuild> GetAllBuilds()
//         {
//             return _repo.GetAll();
//         }
//
//         public void SaveBuild(CharacterBuild build)
//         {
//             if (build.Id == 0)
//             {
//                 _repo.Add(build);
//             }
//             else
//             {
//                 _repo.Update(build);
//             }
//         }
//     }
// }