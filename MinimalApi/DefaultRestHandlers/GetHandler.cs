using AutoMapper;
using EntityWebApi.AutoMapper.Interfaces;
using EntityWebApi.Core.Attributes;
using EntityWebApi.Core.Enums;
using EntityWebApi.EFCore;
using EntityWebApi.MinimalApi.Attributes;
using EntityWebApi.MinimalApi.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace MinimalApi.DefaultRestHandlers
{

    [GenericHandler]
    [ArgumentRouteValue("Entity", "TEntity")]
    [ArgumentRouteValue("EntityKey", "TKeyDto", RouteValueConversion.ObjectValues)]
    public class GetHandler<TDbContext, TEntity, TKeyDto, TDto>
        where TDbContext : DbContext
        where TEntity : class
        where TKeyDto : class
    {
        private readonly TDbContext _dbContext;
        private readonly IMapper _mapper;

        public GetHandler(TDbContext dbContext, IMapperStore mapperStore)
        {
            _dbContext = dbContext;
            _mapper = mapperStore.GetMapper("EntityMapper");
        }

        [GenericHandlerRoute(HttpMethod.Get, "[Entity]/[EntityKey]")]
        public async Task<TDto?> Handle([AsParameters] TKeyDto key)
        {
            var result = await _mapper.ProjectTo<TDto>(_dbContext.Set<TEntity>().WhereHasKey(key)).FirstOrDefaultAsync();
            return result;
        }
    }
}