using EntityWebApi.Core.Attributes;
using EntityWebApi.Core.Enums;

namespace EntityWebApi.Core.Tests.RouteValueResolverTestTypes
{
    [ArgumentRouteValue("parameter", "T", (RouteValueConversion)2)]
    internal class UnknownRouteValueConversion<T> { }
}
