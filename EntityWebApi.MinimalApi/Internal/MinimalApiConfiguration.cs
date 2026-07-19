using EntityWebApi.Core;
using EntityWebApi.MinimalApi.Interfaces;
using Microsoft.AspNetCore.Builder;

namespace EntityWebApi.MinimalApi.Internal
{
    public class MinimalApiConfiguration : IMinimalApiConfiguration
    {
        public Action<RouteHandlerBuilder, Type>? ConfigureRoute { get; private set; }
        public string TagTemplate { get; private set; } = "";
        public List<Type> Handlers { get; } = new();
        public GenericTypeResolver GenericTypeResolver { get; } = new();

        public IMinimalApiConfiguration SetDefaultArgumentResolver(Func<string, Type, Type> resolver)
        {
            GenericTypeResolver.SetDefaultArgumentResolver(resolver);
            return this;
        }

        public IMinimalApiConfiguration AddArgumentResolver(string argumentName, Func<Type, Type> resolver)
        {
            GenericTypeResolver.AddArgumentResolver(argumentName, resolver);
            return this;
        }

        public IMinimalApiConfiguration AddHandler<TArgument>(Type handler)
        {
            ValidateHandler(handler);
            Handlers.Add(GenericTypeResolver.Resolve(handler, typeof(TArgument)));
            return this;
        }

        public IMinimalApiConfiguration AddHandlers(Type handler, IEnumerable<Type> arguments)
        {
            ValidateHandler(handler);
            Handlers.AddRange(arguments.Select(a => GenericTypeResolver.Resolve(handler, a)));
            return this;
        }

        public IMinimalApiConfiguration ConfigureRouteHandlerBuilder(Action<RouteHandlerBuilder, Type> configure)
        {
            ConfigureRoute = configure;
            return this;
        }

        public IMinimalApiConfiguration SetTagTemplate(string tagTemplate)
        {
            TagTemplate = tagTemplate;
            return this;
        }


        private static void ValidateHandler(Type handler)
        {
            if (!handler.IsGenericType || handler.IsConstructedGenericType)
            {
                throw new ArgumentException($"EntityWebApi: Expected handler {handler.Name} to be generic");
            }
        }
    }
}