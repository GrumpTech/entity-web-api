using NUnit.Framework;
using Shouldly;
using Tests.Shared.Dtos;

namespace EntityWebApi.Core.Tests
{
    public class TypeStoreTests
    {
        [Test]
        public void Get_Added()
        {
            var typeStore = new TypeStore();
            typeStore.Add(typeof(ItemDto));

            var type = typeStore.Get(nameof(ItemDto));

            type.ShouldBe(typeof(ItemDto));
        }

        [Test]
        public void Get_NotAdded()
        {
            var typeStore = new TypeStore();

            var type = typeStore.Get("ItemDto");

            type.ShouldBeNull();
        }

        [Test]
        public void GetRequired_Added()
        {
            var typeStore = new TypeStore();
            typeStore.Add(typeof(ItemDto));

            var type = typeStore.GetRequired(nameof(ItemDto));

            type.ShouldBe(typeof(ItemDto));
        }

        [Test]
        public void GetRequired_NotAdded()
        {
            var typeStore = new TypeStore();
            var typeName = "ItemDto";

            Should.Throw<ArgumentException>(() => typeStore.GetRequired(typeName))
                .Message.ShouldBe($"EntityWebApi: No type '{typeName}' found in type store");
        }

        [Test]
        public void Add_TwiceShouldThrowException()
        {
            var typeStore = new TypeStore();

            typeStore.Add(typeof(ItemDto));
            Should.Throw<ArgumentException>(() => typeStore.Add(typeof(ItemDto)))
                .Message.ShouldBe($"EntityWebApi: A type with the same name {typeof(ItemDto).Name} was added before");
        }
    }
}