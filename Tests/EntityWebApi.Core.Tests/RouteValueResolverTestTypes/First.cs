using System.Diagnostics.CodeAnalysis;

namespace EntityWebApi.Core.Tests.RouteValueResolverTestTypes
{
    [ExcludeFromCodeCoverage]
    internal class First
    {
        public string? Argument { get; set; }
        public int SecondArgument { get; set; }
    }
}
