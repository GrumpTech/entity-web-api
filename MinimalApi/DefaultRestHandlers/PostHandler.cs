using AutoMapper;
using EndpointHandlers.Enums;
using EntityWebApi.AutoMapper;
using EntityWebApi.AutoMapper.Interfaces;
using EntityWebApi.Core.Attributes;
using EntityWebApi.MinimalApi.Attributes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace MinimalApi.DefaultRestHandlers
{
    [GenericHandler]
    [ArgumentRouteValue("Entity", "TEntity")]
    public class PostHandler<TDbContext, TEntity, TDto, TPostDto>
        where TDbContext : DbContext
        where TEntity : class
    {
        private readonly TDbContext _dbContext;
        private readonly IMapper _mapper;

        public PostHandler(TDbContext dbContext, IMapperStore mapperStore)
        {
            _dbContext = dbContext;
            _mapper = mapperStore.GetMapper("EntityMapper");
        }

        [GenericHandlerRoute(HttpMethod.Post, "[Entity]")]
        public async Task<IResult> Handle(HttpRequest request, [FromBody] TPostDto dto)
        {
            if (dto == null)
            {
                return Results.BadRequest();
            }
            var entity = _mapper.MapNotNull<TEntity>(dto);
            await _dbContext.AddAsync(entity);
            await _dbContext.SaveChangesAsync();
            var entityUrl = string.Join("/", _dbContext.Set<TEntity>()
                    .EntityType.FindPrimaryKey()?.Properties
                    .Select(p => p.PropertyInfo?.GetValue(entity)?.ToString() ?? string.Empty) ?? []);
            return Results.Created($"{request.GetEncodedUrl()}/{entityUrl}", _mapper.Map<TDto>(entity));
        }
    }
}