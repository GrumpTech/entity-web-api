using AutoMapper;
using AutoMapper.EntityFrameworkCore;
using AutoMapper.EquivalencyExpression;
using EntityWebApi.Core;
using EntityWebApi.Core.Attributes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace EntityWebApi.AutoMapper
{
    public static class MapperConfigurationExpressionExtensions
    {
        /// <summary>
        /// Creates map to entity collections without re-creating the collection object with Automapper.Collection.EntityFrameworkCore.
        /// Adds, updates, deletes entities based on the primary key.
        /// </summary>
        public static void AddEntityFrameworkCoreCollectionMappers(this IMapperConfigurationExpression expression, DbContext dbContext)
        {
            expression.AddCollectionMappers();
            expression.SetGeneratePropertyMaps(
                new GenerateEntityFrameworkCorePrimaryKeyPropertyMaps(dbContext.Model)
            );
        }

        /// <summary>
        /// Creates maps between entities and dto's. Finds corresponding dto's with specified suffix in the type store.
        /// </summary>
        public static void CreateEntityMaps(this IMapperConfigurationExpression expression, DbContext dbContext, TypeStore typeStore, string suffix)
        {
            ForEntities(dbContext, typeStore, suffix, (entityType, type) =>
            {
                expression.CreateMap(entityType.ClrType, type);
                expression.CreateMapToEntity(type, entityType);
            });
        }

        /// <summary>
        /// Creates maps from dto's to entities. Finds corresponding dto's with specified suffix in the type store.
        /// </summary>
        public static void CreateMapsToEntity(this IMapperConfigurationExpression expression, DbContext dbContext, TypeStore typeStore, string suffix)
        {
            ForEntities(dbContext, typeStore, suffix, (entityType, type) =>
                expression.CreateMapToEntity(type, entityType));
        }

        /// <summary>
        /// Creates maps from entities to dto's. Finds corresponding dto's with specified suffix in the type store.
        /// </summary>
        public static void CreateMapsFromEntity(this IMapperConfigurationExpression expression, DbContext dbContext, TypeStore typeStore, string suffix)
        {
            ForEntities(dbContext, typeStore, suffix, (entityType, type) =>
                expression.CreateMap(entityType.ClrType, type));
        }


        /// <summary>
        /// Creates map from dto to entity. Finds corresponding dto with specified suffix in the type store.
        /// </summary>
        public static void CreateMapToEntity(this IMapperConfigurationExpression expression, Type type, IEntityType entityType)
        {
            var keyProperties = (entityType.FindPrimaryKey()?.Properties
                .Where(p => p.ValueGenerated.HasFlag(ValueGenerated.OnAdd))
                .Select(p => p.Name) ?? Array.Empty<string>()).ToHashSet();
            var properties = type.GetProperties()
                .Where(p => !keyProperties.Contains(p.Name));
            var map = expression.CreateMap(type, entityType.ClrType);
            foreach (var keyProperty in keyProperties)
            {
                map.ForMember(keyProperty, o => o.PreCondition(s => false));
            }
            foreach (var property in properties)
            {
                if (property.GetSetMethod() == null)
                {
                    map.ForMember(property.Name, o => o.Ignore());
                }
                var hasValueField = property.GetCustomAttribute<HasValueFieldAttribute>()?.Name;
                if (hasValueField != null)
                {
                    var hasValueFunction = HasValue(type, hasValueField);
                    map.ForMember(property.Name, o => o.Condition((src, dest, srcMember) => hasValueFunction(src)));
                }
            }
        }

        /// <summary>
        /// Creates map from entity to dto. Finds corresponding dto with specified suffix in the type store.
        /// </summary>
        private static Func<object, bool> HasValue(Type type, string fieldName)
        {
            var parameter = Expression.Parameter(typeof(object), "f");
            var typeParamter = Expression.Convert(parameter, type);
            var member = Expression.Field(typeParamter, fieldName);
            var expression = Expression.Lambda(typeof(Func<object, bool>), member, parameter);
            return (Func<object, bool>)expression.Compile();
        }

        private static void ForEntities(DbContext dbContext, TypeStore typeStore, string suffix, Action<IEntityType, Type> action)
        {
            var entityTypes = dbContext.Model.GetEntityTypes();
            foreach (var entityType in entityTypes)
            {
                var type = typeStore.Get($"{entityType.ClrType.Name}{suffix}");
                if (type != null)
                {
                    action(entityType, type);
                }
            }
        }
    }
}