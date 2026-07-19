
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
    public class DeleteHandler<TDbContext, TEntity, TKeyDto>
        where TDbContext : DbContext
        where TEntity : class
        where TKeyDto : class
    {
        private readonly TDbContext _dbContext;

        public DeleteHandler(TDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [GenericHandlerRoute(HttpMethod.Delete, "[Entity]/[EntityKey]")]
        public async Task<IResult> Handle([AsParameters] TKeyDto key)
        {
            var dbSet = _dbContext.Set<TEntity>();
            var entity = await dbSet.WhereHasKey(key).FirstOrDefaultAsync();
            if (entity == null)
            {
                return Results.NotFound();
            }
            dbSet.Remove(entity);
            await _dbContext.SaveChangesAsync();
            return Results.Ok();
        }
    }
}