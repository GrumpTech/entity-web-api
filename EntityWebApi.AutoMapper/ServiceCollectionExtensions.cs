using EntityWebApi.AutoMapper.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace EntityWebApi.AutoMapper
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Registers a store for AutoMapper mappers.
        /// </summary>
        public static IServiceCollection AddMapperStore(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<IMapperStore, MapperStore>();
            return serviceCollection;
        }
    }
}