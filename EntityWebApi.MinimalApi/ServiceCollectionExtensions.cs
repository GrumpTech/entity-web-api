using EntityWebApi.MinimalApi.Attributes;
using EntityWebApi.MinimalApi.Internal;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace EntityWebApi.MinimalApi
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddEntityWebApi(this IServiceCollection services, params Assembly[] assemblies)
        {
            var handlers = assemblies
                .SelectMany(a => a.GetTypes())
                .Where(t => t.GetCustomAttributes<GenericHandlerAttribute>().Any());
            services.AddSingleton(new HandlerConfiguration(assemblies));
            foreach (var handler in handlers)
            {
                if (!handler.IsGenericType || handler.IsConstructedGenericType)
                {
                    throw new ArgumentException($"EntityWebApi: Expected handler {handler} to be generic.");
                }
                services.AddTransient(handler);
            }
            return services;
        }
    }
}
