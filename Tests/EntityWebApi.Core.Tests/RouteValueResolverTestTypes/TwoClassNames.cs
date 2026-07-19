using EntityWebApi.Core.Attributes;

namespace EntityWebApi.Core.Tests.RouteValueResolverTestTypes
{
    [ArgumentRouteValue("route", "T"), ArgumentRouteValue("secondRoute", "T2")]
    internal class TwoClassNames<T, T2> { }
}
