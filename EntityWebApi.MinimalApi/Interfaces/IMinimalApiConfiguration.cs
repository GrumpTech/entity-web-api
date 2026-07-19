using Microsoft.AspNetCore.Builder;

namespace EntityWebApi.MinimalApi.Interfaces
{
    public interface IMinimalApiConfiguration
    {
        public IMinimalApiConfiguration SetDefaultArgumentResolver(Func<string, Type, Type> resolver);
        public IMinimalApiConfiguration AddArgumentResolver(string argumentName, Func<Type, Type> resolver);
        public IMinimalApiConfiguration AddHandler<TArgument>(Type handler);
        public IMinimalApiConfiguration AddHandlers(Type handler, IEnumerable<Type> arguments);
        public IMinimalApiConfiguration ConfigureRouteHandlerBuilder(Action<RouteHandlerBuilder, Type> configure);
        public IMinimalApiConfiguration SetTagTemplate(string argumentName);
    }
}