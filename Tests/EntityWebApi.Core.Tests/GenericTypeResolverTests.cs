using EntityWebApi.Core.Tests.GenericTypeResolverTestTypes;
using NUnit.Framework;
using Shouldly;

namespace EntityWebApi.Core.Tests
{
    public class GenericTypeResolverTests
    {
        [Test]
        public void Resolve_DefaultArgumentResolver()
        {
            var resolver = new GenericTypeResolver();
            resolver.SetDefaultArgumentResolver((argumentName, type) => typeof(First));

            var result = resolver.Resolve(typeof(GenericType<,>), typeof(First));

            result.ShouldBe(typeof(GenericType<First, First>));
        }

        [Test]
        public void Resolve_ArgumentResolver()
        {
            var resolver = new GenericTypeResolver();
            resolver.SetDefaultArgumentResolver((argumentName, type) => typeof(First));
            resolver.AddArgumentResolver("U", (type) => typeof(Second));

            var result = resolver.Resolve(typeof(GenericType<,>), typeof(First));

            result.ShouldBe(typeof(GenericType<First, Second>));
        }

        [Test]
        public void Resolve_BothArgumentResolvers()
        {
            var resolver = new GenericTypeResolver();
            resolver.AddArgumentResolver("T", (type) => typeof(First));
            resolver.AddArgumentResolver("U", (type) => typeof(Second));

            var result = resolver.Resolve(typeof(GenericType<,>), typeof(First));

            result.ShouldBe(typeof(GenericType<First, Second>));
        }

        [Test]
        public void Resolve_NoArgumentResolver()
        {
            var resolver = new GenericTypeResolver();
            var genericType = typeof(GenericType<,>);
            resolver.AddArgumentResolver("T", (type) => typeof(First));

            Should.Throw<InvalidOperationException>(() => resolver.Resolve(genericType, typeof(First)))
                .Message.ShouldBe($"EntityWebApi: Generic type constructor could not resolve generic argument 'U' for type {genericType.Name}");
        }
    }
}