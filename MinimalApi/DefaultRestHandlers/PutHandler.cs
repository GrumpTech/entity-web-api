using AutoMapper;
using EntityWebApi.AutoMapper.Interfaces;
using EntityWebApi.Core.Attributes;
using EntityWebApi.Core.Enums;
using EntityWebApi.EFCore;
using EntityWebApi.MinimalApi.Attributes;
using EntityWebApi.MinimalApi.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace MinimalApi.DefaultRestHandlers
{
    [GenericHandler]
    [ArgumentRouteValue("Entity", "TEntity")]
    [ArgumentRouteValue("EntityKey", "TKeyDto", RouteValueConversion.ObjectValues)]
    public class PutHandler<TDbContext, TEntity, TKeyDto, TDto, TPutDto>
        where TDbContext : DbContext
        where TEntity : class
        where TKeyDto : class
    {
        private readonly TDbContext _dbContext;
        private readonly IMapper _mapper;

        public PutHandler(TDbContext dbContext, IMapperStore mapperStore)
        {
            _dbContext = dbContext;
            _mapper = mapperStore.GetMapper("EntityMapper");
        }

        [GenericHandlerRoute(HttpMethod.Put, "[Entity]/[EntityKey]")]
        public async Task<IResult> Handle([AsParameters] TKeyDto key, [FromBody] TPutDto dto)
        {
            var entity = await _mapper.ProjectTo<TEntity>(
                _mapper.ProjectTo<TDto>(_dbContext.Set<TEntity>().WhereHasKey(key))
            ).FirstOrDefaultAsync();

            if (entity == null)
            {
                return Results.NotFound();
            }
            _dbContext.Attach(entity);
            _mapper.Map(dto, entity);
            await _dbContext.SaveChangesAsync();
            return Results.Ok(_mapper.Map<TDto>(entity));
        }
    }
}