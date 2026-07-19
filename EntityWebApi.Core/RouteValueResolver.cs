using EntityWebApi.Core.Attributes;
using EntityWebApi.Core.Enums;
using System.Reflection;

namespace EntityWebApi.Core
{
    public static class RouteValueResolver
    {
        public static IDictionary<string, string> GetReplacements(Type type)
        {
            var result = new Dictionary<string, string>();
            if (!type.IsGenericType)
            {
                return result;
            }
            var routeValueAttributes = (ArgumentRouteValueAttribute[])type
                .GetCustomAttributes(typeof(ArgumentRouteValueAttribute));
            var argumentIndexes = GetArgumentIndexes(type);
            var genericArguments = type.GenericTypeArguments;
            foreach (var attribute in routeValueAttributes)
            {
                if (!argumentIndexes.TryGetValue(attribute.GenericArgument, out var index))
                {
                    throw new ArgumentException($"EntityWebApi: No generic argument with name {attribute.GenericArgument} found for type {type.Name}");
                }
                switch (attribute.Conversion)
                {
                    case RouteValueConversion.ClassName:
                        result.Add(attribute.RouteKey, genericArguments[index].Name);
                        break;
                    case RouteValueConversion.ObjectValues:
                        var argumentType = genericArguments[index];
                        var propertyNames = argumentType.GetProperties().Select(p => p.Name);
                        if (propertyNames.Any())
                        {
                            result.Add(attribute.RouteKey, $"{{{string.Join("}/{", propertyNames)}}}");
                        }
                        break;
                    default:
                        throw new NotImplementedException($"EntityWebApi: Unknown conversion {attribute.Conversion} for type {type.Name}");
                }
            }
            return result;
        }

        private static IDictionary<string, int> GetArgumentIndexes(Type type)
        {
            var idx = 0;
            return type.GetGenericTypeDefinition()
                .GetGenericArguments()
                .ToDictionary(t => t.Name, t => idx++);
        }
    }
}
