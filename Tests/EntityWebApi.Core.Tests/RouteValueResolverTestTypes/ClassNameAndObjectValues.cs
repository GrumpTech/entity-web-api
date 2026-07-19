using EntityWebApi.Core.Attributes;
using EntityWebApi.Core.Enums;

namespace EntityWebApi.Core.Tests.RouteValueResolverTestTypes
{
    [ArgumentRouteValue("route", "T"), ArgumentRouteValue("parameter", "T", RouteValueConversion.ObjectValues)]
    internal class ClassNameAndObjectValues<T> { }
}
