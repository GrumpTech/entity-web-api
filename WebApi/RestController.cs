using AutoMapper;
using EntityWebApi.AutoMapper;
using EntityWebApi.AutoMapper.Interfaces;
using EntityWebApi.Core.Attributes;
using EntityWebApi.Core.Enums;
using EntityWebApi.EFCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi
{
    [ApiController]
    [Consumes("application/json")]
    [Produces("application/json")]
    [Route("[Entity]")]
    [OpenApiTag("RestController - [Entity]")]
    [ArgumentRouteValue("Entity", "TEntity")]
    [ArgumentRouteValue("EntityKey", "TKeyDto", RouteValueConversion.ObjectValues)]
    public class RestController<TDbContext, TEntity, TKeyDto, TDto, TPostDto, TPutDto, TPatchDto> : ControllerBase
        where TDbContext : DbContext
        where TEntity : class
        where TKeyDto : class
    {
        private readonly TDbContext _dbContext;
        private readonly IMapper _mapper;

        public RestController(TDbContext dbContext, IMapperStore mapperStore)
        {
            _dbContext = dbContext;
            _mapper = mapperStore.GetMapper("EntityMapper");
        }

        [HttpGet]
        public async Task<IEnumerable<TDto>> Get()
        {
            return await _mapper.ProjectTo<TDto>(_dbContext.Set<TEntity>()).ToListAsync();
        }

        [HttpGet("[EntityKey]")]
        public async Task<ActionResult<TDto>> Get([FromRoute] TKeyDto key)
        {
            var result = await _mapper.ProjectTo<TDto>(_dbContext.Set<TEntity>().WhereHasKey(key)).FirstOrDefaultAsync();
            return result == null ? NotFound() : result;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<TDto>> Post([FromBody] TPostDto dto)
        {
            if (dto == null)
            {
                return BadRequest();
            }
            var entity = _mapper.MapNotNull<TEntity>(dto);
            await _dbContext.AddAsync(entity);
            await _dbContext.SaveChangesAsync();
            var entityUrl = string.Join("/", _dbContext.Set<TEntity>()
                    .EntityType.FindPrimaryKey()?.Properties
                    .Select(p => p.PropertyInfo?.GetValue(entity)?.ToString() ?? string.Empty) ?? []);
            return Created($"{Request.GetEncodedUrl()}/{entityUrl}", _mapper.Map<TDto>(entity));
        }

        [HttpPut("[EntityKey]")]
        public async Task<ActionResult<TDto>> Put([FromRoute] TKeyDto key, [FromBody] TPutDto dto)
        {
            var entity = await _mapper.ProjectTo<TEntity>(
                _mapper.ProjectTo<TDto>(_dbContext.Set<TEntity>().WhereHasKey(key))
            ).FirstOrDefaultAsync();
            if (entity == null)
            {
                return NotFound();
            }
            _dbContext.Attach(entity);
            _mapper.Map(dto, entity);
            await _dbContext.SaveChangesAsync();
            return Ok(_mapper.Map<TDto>(entity));
        }

        [HttpPatch("[EntityKey]")]
        public async Task<ActionResult> Patch([FromRoute] TKeyDto key, [FromBody] TPatchDto patchDto)
        {
            var entity = await _mapper.ProjectTo<TEntity>(
                _mapper.ProjectTo<TDto>(_dbContext.Set<TEntity>().WhereHasKey(key))
            ).FirstOrDefaultAsync();
            if (entity == null)
            {
                return NotFound();
            }
            _dbContext.Attach(entity);
            _mapper.Map(patchDto, entity);
            await _dbContext.SaveChangesAsync();
            return Ok(_mapper.Map<TDto>(entity));
        }

        [HttpDelete("[EntityKey]")]
        public async Task<ActionResult> Delete([FromRoute] TKeyDto key)
        {
            var dbSet = _dbContext.Set<TEntity>();
            var entity = await dbSet.WhereHasKey(key).FirstOrDefaultAsync();
            if (entity == null)
            {
                return NotFound();
            }
            dbSet.Remove(entity);
            await _dbContext.SaveChangesAsync();
            return Ok();
        }
    }
}
