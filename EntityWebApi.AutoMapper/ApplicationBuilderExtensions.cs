using AutoMapper;
using EntityWebApi.AutoMapper.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace EntityWebApi.AutoMapper
{
    public static class ApplicationBuilderExtensions
    {
        /// <summary>
        /// Stores an AutoMapper mapper under the specified name.
        /// </summary>
        public static IApplicationBuilder AddToMapperStore(this IApplicationBuilder applicationBuilder, string name, IMapper mapper)
        {
            var mapperStore = applicationBuilder.ApplicationServices.GetRequiredService<IMapperStore>();
            mapperStore.AddMapper(name, mapper);
            return applicationBuilder;
        }

        /// <summary>
        /// Creates an AutoMapper mapper from the configuration and stores it under the specified name.
        /// </summary>
        public static IApplicationBuilder AddToMapperStore(this IApplicationBuilder applicationBuilder, string name, Action<IMapperConfigurationExpression> configure)
        {
            return applicationBuilder.AddToMapperStore(name, new MapperConfiguration(configure).CreateMapper());
        }
    }
}