using EntityWebApi.Core.Tests.RouteValueResolverTestTypes;
using NUnit.Framework;
using Shouldly;

namespace EntityWebApi.Core.Tests
{
    public class RouteValueResolverTests
    {
        [Test]
        public void GetReplacements_NoAttributes()
        {
            var replacements = RouteValueResolver.GetReplacements(typeof(NoAttributes));

            replacements.Any().ShouldBeFalse();
        }

        [Test]
        public void GetReplacements_ClassName()
        {
            var replacements = RouteValueResolver.GetReplacements(typeof(ClassName<First>));

            replacements.Count.ShouldBe(1);
            replacements["route"].ShouldBe(nameof(First));
        }

        [Test]
        public void GetReplacements_TwoClassNames()
        {
            var replacements = RouteValueResolver.GetReplacements(typeof(TwoClassNames<First, Second>));

            replacements.Count.ShouldBe(2);
            replacements["route"].ShouldBe(nameof(First));
            replacements["secondRoute"].ShouldBe(nameof(Second));
        }

        [Test]
        public void GetReplacements_ClassNameWrongArgument()
        {
            var type = typeof(ClassNameWrongArgument<First>);
            Should.Throw<ArgumentException>(() => RouteValueResolver.GetReplacements(type))
                .Message.ShouldBe($"EntityWebApi: No generic argument with name T2 found for type {type.Name}");
        }

        [Test]
        public void GetReplacements_ObjectValues()
        {
            var replacements = RouteValueResolver.GetReplacements(typeof(ObjectValues<First>));

            replacements.Count.ShouldBe(1);
            replacements["parameter"].ShouldBe($"{{{nameof(First.Argument)}}}/{{{nameof(First.SecondArgument)}}}");
        }

        [Test]
        public void GetReplacements_TwoTimesObjectValues()
        {
            var replacements = RouteValueResolver.GetReplacements(typeof(TwoTimesObjectValues<First, Second>));

            replacements.Count.ShouldBe(2);
            replacements["parameter"].ShouldBe($"{{{nameof(First.Argument)}}}/{{{nameof(First.SecondArgument)}}}");
            replacements["secondParameter"].ShouldBe($"{{{nameof(Second.Argument)}}}");
        }

        [Test]
        public void GetReplacements_ObjectValuesWrongArgument()
        {
            var type = typeof(ObjectValuesWrongArgument<First>);
            Should.Throw<ArgumentException>(() => RouteValueResolver.GetReplacements(type))
                .Message.ShouldBe($"EntityWebApi: No generic argument with name T2 found for type {type.Name}");
        }

        [Test]
        public void GetReplacements_ClassNameAndObjectValues()
        {
            var replacements = RouteValueResolver.GetReplacements(typeof(ClassNameAndObjectValues<First>));

            replacements.Count.ShouldBe(2);
            replacements["route"].ShouldBe(nameof(First));
            replacements["parameter"].ShouldBe($"{{{nameof(First.Argument)}}}/{{{nameof(First.SecondArgument)}}}");
        }

        [Test]
        public void GetReplacements_UnknownRouteValueConversion()
        {
            var type = typeof(UnknownRouteValueConversion<First>);
            Should.Throw<NotImplementedException>(() => RouteValueResolver.GetReplacements(type))
                .Message.ShouldBe($"EntityWebApi: Unknown conversion 2 for type {type.Name}");
        }
    }
}