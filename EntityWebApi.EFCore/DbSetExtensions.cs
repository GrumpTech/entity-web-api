using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace EntityWebApi.EFCore
{
    public static class DbSetExtensions
    {
        public static IQueryable<TEntity> WhereHasKey<TEntity>(this DbSet<TEntity> dbSet, object key)
            where TEntity : class
        {
            return dbSet.Where(dbSet.HasKeyExpression(key));
        }
        public static IQueryable<TEntity> WhereHasOneOfKeys<TEntity>(this DbSet<TEntity> dbSet, IEnumerable<object> keys)
            where TEntity : class
        {
            return dbSet.Where(dbSet.HasOneOfKeysExpression(keys));
        }

        public static Expression<Func<TEntity, bool>> HasKeyExpression<TEntity>(this DbSet<TEntity> dbSet, object key)
            where TEntity : class
        {
            var keyProperties = GetKeyProperties(dbSet).ToList();
            var parameter = Expression.Parameter(typeof(TEntity), "e");

            var hasPropertiesExpression = HasProperties(parameter, key, keyProperties);
            return Expression.Lambda<Func<TEntity, bool>>(hasPropertiesExpression, new[] { parameter });
        }

        public static Expression<Func<TEntity, bool>> HasOneOfKeysExpression<TEntity>(this DbSet<TEntity> dbSet, IEnumerable<object> keys)
            where TEntity : class
        {
            var parameter = Expression.Parameter(typeof(TEntity), "e");
            if (!keys.Any())
            {
                return Expression.Lambda<Func<TEntity, bool>>(Expression.Constant(false), new[] { parameter });
            }
            var keyProperties = GetKeyProperties(dbSet).ToList();
            var expression = HasProperties(parameter, keys.First(), keyProperties);
            foreach (var key in keys.Skip(1))
            {
                expression = Expression.Or(expression, HasProperties(parameter, key, keyProperties));
            }
            return Expression.Lambda<Func<TEntity, bool>>(expression, new[] { parameter });
        }

        private static BinaryExpression HasProperties(ParameterExpression parameter, object obj, IEnumerable<string> properties)
        {
            var constant = Expression.Constant(obj);
            var createEqualityExpression = (string property) => Expression.Equal(
                Expression.Property(parameter, property), Expression.Property(constant, property));
            var equality = createEqualityExpression(properties.First());
            foreach (var property in properties.Skip(1))
            {
                equality = Expression.And(equality, createEqualityExpression(property));
            }
            return equality;
        }

        private static IEnumerable<string> GetKeyProperties<TEntity>(DbSet<TEntity> dbSet)
            where TEntity : class
        {
            var result = dbSet.EntityType.FindPrimaryKey()?.Properties.Select(p => p.Name);
            if (result == null || !result.Any())
            {
                throw new InvalidOperationException($"EntityWebApi: No key specified for entity '{typeof(TEntity)}'");
            }
            return result;
        }
    }
}