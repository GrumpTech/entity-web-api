using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using Shouldly;
using Tests.Shared;

namespace EntityWebApi.Dtos.Tests
{
    public class DtoTests
    {
        private readonly DbContextOptions<DataContext> _dbContextOptions;
        public DtoTests()
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
        public void Construct()
        {
            var dtoBuilder = new DtoBuilder();

            dtoBuilder.ShouldNotBeNull();
        }
    }
}