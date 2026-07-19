using EntityWebApi.MinimalApi.Internal;
using EntityWebApi.Core;
using EntityWebApi.Core.Attributes;
using EntityWebApi.MinimalApi.Attributes;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using HttpMethod = EntityWebApi.MinimalApi.Enums.HttpMethod;

namespace EntityWebApi.MinimalApi
{
    public static class EndpointRouteBuilderExtensions
    {
        public static List<RouteHandlerBuilder> MapGenericHandler(this IEndpointRouteBuilder endpoints, Type type, string tagTemplate)
        {
            var replacements = RouteValueResolver.GetReplacements(type);
            var methods = type.GetMethods();
            var result = new List<RouteHandlerBuilder>();
            foreach (var method in methods)
            {
                var route = method.GetCustomAttribute<GenericHandlerRouteAttribute>();
                if (route != null)
                {
                    var routeHandlerBuilder = endpoints.MapEndpointHandler(
                        ReplaceMultiple(route.Template, replacements), route.HttpMethod, method);
                    var tag = type.GetCustomAttribute<OpenApiTagAttribute>()?.Template ?? tagTemplate;
                    routeHandlerBuilder.WithTags(ReplaceMultiple(tag, replacements));
                    result.Add(routeHandlerBuilder);
                }
            }
            return result;
        }

        public static RouteHandlerBuilder MapEndpointHandler(this IEndpointRouteBuilder endpoints,
            [StringSyntax("Route")] string route, HttpMethod httpMethod, MethodInfo method)
        {
            var attributes = method.GetCustomAttributesData();
            var endpointDelegate = new DelegateFactory().CreateDelegate(method, attributes);
            return endpoints.MapMethods(route, [httpMethod.ToString()], endpointDelegate);
        }

        private static string ReplaceMultiple(string value, IDictionary<string, string> replacements)
        {
            foreach (var replacement in replacements)
            {
                value = value.Replace($"[{replacement.Key}]", replacement.Value);
            }
            return value;
        }
    }
}
