using AutoMapper;
using EndpointHandlers.Enums;
using EntityWebApi.AutoMapper.Interfaces;
using EntityWebApi.Core.Attributes;
using EntityWebApi.Core.Enums;
using EntityWebApi.EFCore;
using EntityWebApi.MinimalApi.Attributes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace MinimalApi.DefaultRestHandlers
{
    [GenericHandler]
    [ArgumentRouteValue("Entity", "TEntity")]
    [ArgumentRouteValue("EntityKey", "TKeyDto", RouteValueConversion.ObjectValues)]
    public class PatchHandler<TDbContext, TEntity, TKeyDto, TDto, TPatchDto>
        where TDbContext : DbContext
        where TEntity : class
        where TKeyDto : class
    {
        private readonly TDbContext _dbContext;
        private readonly IMapper _mapper;

        public PatchHandler(TDbContext dbContext, IMapperStore mapperStore)
        {
            _dbContext = dbContext;
            _mapper = mapperStore.GetMapper("EntityMapper");
        }

        [GenericHandlerRoute(HttpMethod.Patch, "[Entity]/[EntityKey]")]
        public async Task<IResult> Patch([AsParameters] TKeyDto key, [FromBody] TPatchDto patchDto)
        {
            var entity = await _mapper.ProjectTo<TEntity>(
                _mapper.ProjectTo<TDto>(_dbContext.Set<TEntity>().WhereHasKey(key))
            ).FirstOrDefaultAsync();
            if (entity == null)
            {
                return Results.NotFound();
            }
            _dbContext.Attach(entity);
            _mapper.Map(patchDto, entity);
            await _dbContext.SaveChangesAsync();
            return Results.Ok(_mapper.Map<TDto>(entity));
        }
    }
}