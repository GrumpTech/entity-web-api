using EntityWebApi.Controllers.Interfaces;
using EntityWebApi.Core;

namespace EntityWebApi.Controllers.Internal
{
    public class FeatureProviderOptions : IControllerConfiguration
    {
        public List<Type> Controllers { get; } = new();
        public GenericTypeResolver GenericTypeResolver { get; } = new();

        public IControllerConfiguration SetDefaultArgumentResolver(Func<string, Type, Type> resolver)
        {
            GenericTypeResolver.SetDefaultArgumentResolver(resolver);
            return this;
        }

        public IControllerConfiguration AddArgumentResolver(string argumentName, Func<Type, Type> resolver)
        {
            GenericTypeResolver.AddArgumentResolver(argumentName, resolver);
            return this;
        }

        public IControllerConfiguration AddController<TArgument>(Type controller)
        {
            if (!controller.IsGenericType)
            {
                throw new ArgumentException($"EntityWebApi: Controller {controller.Name} is not generic");
            }
            Controllers.Add(GenericTypeResolver.Resolve(controller, typeof(TArgument)));
            return this;
        }

        public IControllerConfiguration AddControllers(Type controller, IEnumerable<Type> arguments)
        {
            if (!controller.IsGenericType)
            {
                throw new ArgumentException($"EntityWebApi: Controller {controller.Name} is not generic");
            }
            Controllers.AddRange(arguments.Select(a => GenericTypeResolver.Resolve(controller, a)));
            return this;
        }
    }
}