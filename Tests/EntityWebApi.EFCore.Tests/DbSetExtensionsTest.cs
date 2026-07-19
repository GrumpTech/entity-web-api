using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using Shouldly;
using Tests.Shared;
using Tests.Shared.Entities;

namespace EntityWebApi.EFCore.Tests
{
    public class DbSetExtensionsTest
    {
        private readonly DbContextOptions<DataContext> _dbContextOptions;
        public DbSetExtensionsTest()
        {
            _dbContextOptions = new DbContextOptionsBuilder<DataContext>()
               .UseInMemoryDatabase(Guid.NewGuid().ToString())
               .Options;
        }

        [SetUp]
        public async Task Setup()
        {
            var context = new DataContext(_dbContextOptions);
            await context.Database.EnsureDeletedAsync();
            await context.Database.EnsureCreatedAsync();
        }

        [Test]
        public async Task WhereHasKey_ExistingKey()
        {
            var context = new DataContext(_dbContextOptions);
            await context.Items.AddRangeAsync(new int[] { 1, 2, 3 }.Select(i => new Item { Id = i }));
            await context.SaveChangesAsync();

            var item = await context.Items.WhereHasKey(new { Id = 2 }).SingleAsync();

            item.Id.ShouldBe(2);
        }

        [Test]
        public async Task WhereHasKey_NonExistingKey()
        {
            var context = new DataContext(_dbContextOptions);
            await context.Items.AddRangeAsync(new int[] { 1, 3 }.Select(i => new Item { Id = i }));
            await context.SaveChangesAsync();

            var items = await context.Items.WhereHasKey(new { Id = 2 }).ToListAsync();

            items.Count.ShouldBe(0);
        }

        [Test]
        public async Task WhereHasKey_ExistingCompositeKey()
        {
            var context = new DataContext(_dbContextOptions);
            await context.CompositeKeyItems.AddRangeAsync(new CompositeKeyItem[]
            {
                new CompositeKeyItem { Key = 1, Key2 = "a" },
                new CompositeKeyItem { Key = 1, Key2 = "b" },
                new CompositeKeyItem { Key = 2, Key2 = "a" }
            });
            await context.SaveChangesAsync();

            var item = await context.CompositeKeyItems.WhereHasKey(new { Key = 1, Key2 = "a" }).SingleAsync();

            item.Key.ShouldBe(1);
            item.Key2.ShouldBe("a");
        }

        [Test]
        public async Task WhereHasKey_NonExistingCompositeKey()
        {
            var context = new DataContext(_dbContextOptions);
            await context.CompositeKeyItems.AddRangeAsync(new CompositeKeyItem[]
            {
                new CompositeKeyItem { Key = 1, Key2 = "a" },
                new CompositeKeyItem { Key = 2, Key2 = "a" }
            });
            await context.SaveChangesAsync();

            var items = await context.CompositeKeyItems.WhereHasKey(new { Key = 1, Key2 = "b" }).ToListAsync();

            items.Count.ShouldBe(0);
        }

        [Test]
        public void WhereHasKey_NoKey()
        {
            var context = new DataContext(_dbContextOptions);

            Should.Throw<InvalidOperationException>(() => context.NoKeyItems.WhereHasKey(new { Value = 2 }))
                .Message.ShouldBe("EntityWebApi: No key specified for entity 'Tests.Shared.Entities.NoKeyItem'");
        }

        [Test]
        public async Task WhereHasOneOfKeys_ExistingKeys()
        {
            var context = new DataContext(_dbContextOptions);
            await context.Items.AddRangeAsync(new int[] { 1, 2, 3, 4 }.Select(i => new Item { Id = i }));
            await context.SaveChangesAsync();

            var items = await context.Items.WhereHasOneOfKeys(
                new List<object> { new { Id = 2 }, new { Id = 3 } }).ToListAsync();

            items.Count.ShouldBe(2);
            items.Any(i => i.Id == 2).ShouldBe(true);
            items.Any(i => i.Id == 3).ShouldBe(true);
        }

        [Test]
        public async Task WhereHasOneOfKeys_NonExistingKeys()
        {
            var context = new DataContext(_dbContextOptions);
            await context.Items.AddRangeAsync(new int[] { 1, 4 }.Select(i => new Item { Id = i }));
            await context.SaveChangesAsync();

            var items = await context.Items.WhereHasOneOfKeys(
                new List<object> { new { Id = 2 }, new { Id = 3 } }).ToListAsync();

            items.Count.ShouldBe(0);
        }

        [Test]
        public async Task WhereHasOneOfKeys_WithoutKeys()
        {
            var context = new DataContext(_dbContextOptions);
            await context.Items.AddRangeAsync(new int[] { 1, 4 }.Select(i => new Item { Id = i }));
            await context.SaveChangesAsync();

            var items = await context.Items.WhereHasOneOfKeys(
                Array.Empty<object>()).ToListAsync();

            items.Count.ShouldBe(0);
        }

        [Test]
        public async Task WhereHasOneOfKeys_ExistingCompositeKeys()
        {
            var context = new DataContext(_dbContextOptions);
            await context.CompositeKeyItems.AddRangeAsync(new CompositeKeyItem[]
            {
                new CompositeKeyItem { Key = 1, Key2 = "a" },
                new CompositeKeyItem { Key = 1, Key2 = "b" },
                new CompositeKeyItem { Key = 2, Key2 = "a" },
                new CompositeKeyItem { Key = 2, Key2 = "b" }
            });
            await context.SaveChangesAsync();

            var items = await context.CompositeKeyItems.WhereHasOneOfKeys(
                new List<object> { new { Key = 1, key2 = "b" }, new { Key = 2, Key2 = "a" } }).ToListAsync();

            items.Count.ShouldBe(2);
            items.Any(i => i.Key == 1 && i.Key2 == "b").ShouldBe(true);
            items.Any(i => i.Key == 2 && i.Key2 == "a").ShouldBe(true);
        }

        [Test]
        public async Task WhereHasOneOfKeys_NonExistingCompositeKeys()
        {
            var context = new DataContext(_dbContextOptions);
            await context.CompositeKeyItems.AddRangeAsync(new CompositeKeyItem[]
            {
                new CompositeKeyItem { Key = 1, Key2 = "a" },
                new CompositeKeyItem { Key = 1, Key2 = "b" },
                new CompositeKeyItem { Key = 2, Key2 = "b" }
            });
            await context.SaveChangesAsync();

            var items = await context.CompositeKeyItems.WhereHasOneOfKeys(
                new List<object> { new { Key = 2, Key2 = "a" } }).ToListAsync();

            items.Count.ShouldBe(0);
        }

        [Test]
        public void WhereHasOneOfKeys_NoKey()
        {
            var context = new DataContext(_dbContextOptions);

            Should.Throw<InvalidOperationException>(() => context.NoKeyItems.WhereHasOneOfKeys(
                new List<object> { new { Value = 2 } }))
                .Message.ShouldBe("EntityWebApi: No key specified for entity 'Tests.Shared.Entities.NoKeyItem'");
        }

        [Test]
        public async Task WhereHasOneOfKeys_Many()
        {
            var context = new DataContext(_dbContextOptions);
            await context.Items.AddRangeAsync(Enumerable.Range(1, 100).Select(i => new Item { Id = i }));
            await context.SaveChangesAsync();

            var range = Enumerable.Range(11, 50).Select(i => new { Id = i });
            var items = await context.Items.WhereHasOneOfKeys(range).ToListAsync();

            items.Count.ShouldBe(50);
            items.Min(i => i.Id).ShouldBe(11);
            items.Max(i => i.Id).ShouldBe(60);
        }
    }
}