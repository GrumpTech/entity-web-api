using System.Diagnostics.CodeAnalysis;
using HttpMethod = EntityWebApi.MinimalApi.Enums.HttpMethod;

namespace EntityWebApi.MinimalApi.Attributes
{
    [AttributeUsage(AttributeTargets.Method, Inherited = false)]
    public class GenericHandlerRouteAttribute(HttpMethod httpMethod, [StringSyntax("Route")] string template) : Attribute
    {
        public HttpMethod HttpMethod { get; } = httpMethod;

        [StringSyntax("Route")]
        public string Template { get; } = template;
    }
}