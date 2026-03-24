using DarkSoulsBuildsAssistant.Core.Interfaces.Repositories;
using DarkSoulsBuildsAssistant.Infrastructure.Context;

using Microsoft.EntityFrameworkCore;

using AutoMapper;
using AutoMapper.QueryableExtensions;


namespace DarkSoulsBuildsAssistant.Repositories;

public abstract class GenericRepository<TModel, TDto> : IGenericRepository<TDto> 
    where TModel : class 
    where TDto : class
{
    protected readonly BuildsAssistantDbContext DbContext;
    protected readonly DbSet<TModel> DbSet;
    protected readonly IMapper Mapper;

    protected GenericRepository(BuildsAssistantDbContext context, IMapper mapper)
    {
        DbContext = context;
        DbSet = DbContext.Set<TModel>();
        Mapper = mapper;
    }

    protected virtual IQueryable<TModel> GetBaseQuery() => DbSet;

    public virtual async Task<IEnumerable<TDto>> GetAllAsync()
    {
        return await GetBaseQuery()
            .ProjectTo<TDto>(Mapper.ConfigurationProvider)
            .ToListAsync();
    }
    
    public virtual async Task<TDto?> GetByIdAsync(object id)
    {
        var entity = await DbSet.FindAsync(id);
        return entity == null ? null : Mapper.Map<TDto>(entity);
    }

    public virtual async Task AddAsync(TDto dto)
    {
        var entity = Mapper.Map<TModel>(dto);
        await DbSet.AddAsync(entity);
    }

    public virtual async Task UpdateAsync(TDto dto)
    {
        var entity = Mapper.Map<TModel>(dto);
        DbSet.Update(entity);
        await Task.CompletedTask;
    }

    public virtual async Task RemoveAsync(TDto dto)
    {
        var entity = Mapper.Map<TModel>(dto);
        DbSet.Remove(entity);
        await Task.CompletedTask;
    }
}
