using EntityWebApi.Core.Attributes;
using EntityWebApi.Core.Enums;

namespace EntityWebApi.Core.Tests.RouteValueResolverTestTypes
{
    [ArgumentRouteValue("parameter", "T2", RouteValueConversion.ObjectValues)]
    internal class ObjectValuesWrongArgument<T> { }
}
