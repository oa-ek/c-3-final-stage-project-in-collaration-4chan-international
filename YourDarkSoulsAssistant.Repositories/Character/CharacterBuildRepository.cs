using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using DarkSoulsBuildsAssistant.Core.DTOs.Character;
using DarkSoulsBuildsAssistant.Core.Entities.Character;
using DarkSoulsBuildsAssistant.Core.Interfaces.Repositories.Character;
using YourDarkSoulsAssistant.Infrastructure.Context;

namespace YourDarkSoulsAssistant.Repositories.Character;

public class CharacterBuildRepository(BuildsAssistantDbContext context, IMapper mapper)
    : GenericRepository<CharacterBuild, CharacterBuildDTO>(context, mapper), ICharacterBuildRepository
{
    public async Task<IEnumerable<CharacterBuildDTO>> GetBuildsByUserIdAsync(int userId)
    {
        return await DbContext.Set<CharacterBuild>()
            .Where(b => b.UserId == userId)
            .ProjectTo<CharacterBuildDTO>(Mapper.ConfigurationProvider)
            .ToListAsync();
    }
}
