using EntityWebApi.Core.Attributes;
using EntityWebApi.Core.Enums;

namespace EntityWebApi.Core.Tests.RouteValueResolverTestTypes
{
    [ArgumentRouteValue("parameter", "T", RouteValueConversion.ObjectValues)]
    internal class ObjectValues<T> { }
}
