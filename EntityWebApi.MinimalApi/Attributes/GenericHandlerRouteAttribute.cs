using System.Diagnostics.CodeAnalysis;
using HttpMethod = EndpointHandlers.Enums.HttpMethod;

namespace EntityWebApi.MinimalApi.Attributes
{
    [AttributeUsage(AttributeTargets.Method, Inherited = false)]
    public class GenericHandlerRouteAttribute : Attribute
    {
        public HttpMethod HttpMethod { get; }

        [StringSyntax("Route")]
        public string Template { get; }

        public GenericHandlerRouteAttribute(HttpMethod httpMethod, [StringSyntax("Route")] string template)
        {
            Template = template;
            HttpMethod = httpMethod;
        }
    }
}