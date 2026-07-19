using EntityWebApi.Core.Attributes;
using EntityWebApi.Core.Enums;

namespace EntityWebApi.Core.Tests.RouteValueResolverTestTypes
{
    [ArgumentRouteValue("parameter", "T", RouteValueConversion.ObjectValues),
        ArgumentRouteValue("secondParameter", "T2", RouteValueConversion.ObjectValues)]
    internal class TwoTimesObjectValues<T, T2> { }
}
