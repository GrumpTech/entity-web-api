using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace EntityWebApi.Controllers
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Configures route convention for controllers EntityWebApi.
        /// </summary>
        public static IServiceCollection AddEntityWebApi(this IServiceCollection serviceCollection)
        {
            serviceCollection
                .AddOptions<MvcOptions>()
                .Configure(o =>
                    o.Conventions.Add(new ControllerRouteConvention())
                );
            return serviceCollection;
        }
    }
}