using EntityWebApi.MinimalApi.Attributes;
using EntityWebApi.MinimalApi.Interfaces;
using EntityWebApi.MinimalApi.Internal;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace EntityWebApi.MinimalApi
{
    public static class WebApplicationExtensions
    {
        public static IApplicationBuilder UseEntityWebApi(this WebApplication application, Action<IMinimalApiConfiguration> configure)
        {
            var handlerConfiguration = application.Services.GetService<HandlerConfiguration>() ??
                throw new InvalidOperationException("EntityWebApi: ServiceCollection.AddEntityWebApi not called");
            var configuration = new MinimalApiConfiguration();
            configure(configuration);
            ValidateHandlers(application, configuration.Handlers);
            var handlers = configuration.Handlers.Distinct();
            foreach (var handler in handlers)
            {
                var builders = application.MapGenericHandler(handler, configuration.TagTemplate);
                foreach (var builder in builders)
                {
                    configuration.ConfigureRoute?.Invoke(builder, handler);
                }
            }
            return application;
        }

        private static void ValidateHandlers(WebApplication application, IEnumerable<Type> handlers)
        {
            var serviceProviderIsService = application.Services.GetRequiredService<IServiceProviderIsService>();
            foreach (var handler in handlers)
            {
                if (!serviceProviderIsService.IsService(handler))
                {
                    if (!handler.GetCustomAttributes<GenericHandlerAttribute>().Any())
                    {
                        throw new InvalidOperationException($"EntityWebApi: Handler '{handler.Name}' has no 'GenericHandlerAttribute'");
                    }
                    throw new InvalidOperationException($"EntityWebApi: Handler '{handler.Name}' in assembly '{handler.Assembly.FullName}' is not registered as service");
                }
            }
        }
    }
}