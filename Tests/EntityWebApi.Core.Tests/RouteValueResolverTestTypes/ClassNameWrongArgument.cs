using EntityWebApi.Core.Attributes;

namespace EntityWebApi.Core.Tests.RouteValueResolverTestTypes
{
    [ArgumentRouteValue("route", "T2")]
    internal class ClassNameWrongArgument<T> { }
}
