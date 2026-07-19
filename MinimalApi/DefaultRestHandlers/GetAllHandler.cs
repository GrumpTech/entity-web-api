using AutoMapper;
using EndpointHandlers.Enums;
using EntityWebApi.AutoMapper.Interfaces;
using EntityWebApi.Core.Attributes;
using EntityWebApi.MinimalApi.Attributes;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MinimalApi.DefaultRestHandlers
{
    [GenericHandler]
    [ArgumentRouteValue("Entity", "TEntity")]
    public class GetAllHandler<TDbContext, TEntity, TDto>
        where TDbContext : DbContext
        where TEntity : class
    {
        private readonly TDbContext _dbContext;
        private readonly IMapper _mapper;

        public GetAllHandler(TDbContext dbContext, IMapperStore mapperStore)
        {
            _dbContext = dbContext;
            _mapper = mapperStore.GetMapper("EntityMapper");
        }

        [GenericHandlerRoute(HttpMethod.Get, "[Entity]")]
        public async Task<IEnumerable<TDto>> Handle()
        {
            return await _mapper.ProjectTo<TDto>(_dbContext.Set<TEntity>()).ToListAsync();
        }
    }
}