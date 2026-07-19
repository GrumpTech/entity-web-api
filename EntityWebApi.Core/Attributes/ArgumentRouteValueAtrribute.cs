using EntityWebApi.Core.Enums;

namespace EntityWebApi.Core.Attributes
{
    /// <summary>
    /// To provide a route value based on a generic argument from the target class for the specified route key.<br />
    /// Example: ArgumentRouteKey("Entity", "TEntity") replaces "[Entity]" in the route with the name of the generic argument "TEntity".<br />
    /// Example 2: ArgumentRouteKey("EntityKey", "TKeyDto", RouteValueConversion.ObjectValues) replaces "[EntityKey]" in the route with "{Id}"
    /// when TKeyDto is an object with only one property named "Id".
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    public class ArgumentRouteValueAttribute(string routeKey,
        string genericArgument, RouteValueConversion routeValueConversion = RouteValueConversion.ClassName) : Attribute
    {
        public string RouteKey { get; } = routeKey;
        public string GenericArgument { get; } = genericArgument;
        public RouteValueConversion Conversion { get; } = routeValueConversion;
    }
}
