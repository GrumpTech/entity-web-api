using EntityWebApi.Core.Attributes;

namespace EntityWebApi.Core.Tests.RouteValueResolverTestTypes
{
    [ArgumentRouteValue("route", "T")]
    internal class ClassName<T> { }
}
