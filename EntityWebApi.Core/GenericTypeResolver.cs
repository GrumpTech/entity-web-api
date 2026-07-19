using System.Reflection;

namespace EntityWebApi.Core
{
    public class GenericTypeResolver
    {
        private readonly Dictionary<string, Func<Type, Type>> _argumentResolvers = new();
        private Func<string, Type, Type>? _defaultArgumentResolver;

        public Type Resolve(Type type, Type argument)
        {
            var arguments = type.GetGenericArguments().Select(a =>
            {
                if (_argumentResolvers.TryGetValue(a.Name, out var argumentResolver))
                {
                    return argumentResolver(argument);
                }
                if (_defaultArgumentResolver != null)
                {
                    return _defaultArgumentResolver(a.Name, argument);
                }
                throw new InvalidOperationException($"EntityWebApi: Generic type constructor could not resolve generic argument '{a.Name}' for type {type.Name}");
            });
            return type.MakeGenericType(arguments.ToArray()).GetTypeInfo();
        }

        public GenericTypeResolver AddArgumentResolver(string argumentName, Func<Type, Type> resolver)
        {
            _argumentResolvers.Add(argumentName, resolver);
            return this;
        }

        public GenericTypeResolver SetDefaultArgumentResolver(Func<string, Type, Type> resolver)
        {
            _defaultArgumentResolver = resolver;
            return this;
        }
    }
}
