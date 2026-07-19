using AutoMapper;
using EntityWebApi.AutoMapper.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NUnit.Framework;
using Shouldly;
using Tests.Shared.Dtos;
using Tests.Shared.Entities;

namespace EntityWebApi.AutoMapper.Tests
{
    public class MapperStoreTests
    {
        [Test]
        public void GetMapper_Added()
        {
            const string mapperName = "mapper";
            var mapperStore = CreateMapperStore();
            var mapper = new MapperConfiguration(config => { }).CreateMapper();
            mapperStore.AddMapper(mapperName, mapper);

            mapperStore.GetMapper(mapperName).ShouldBe(mapper);
        }

        [Test]
        public void GetMapper_NotAdded()
        {
            const string mapperName = "mapper";
            var mapperStore = CreateMapperStore();

            Should.Throw<InvalidOperationException>(() => mapperStore.GetMapper(mapperName))
                .Message.ShouldBe($"EntityWebApi: Mapping '{mapperName}' not added.");
        }

        [Test]
        public void GetMapper_AddedTwoMappers()
        {
            const string mapperName = "mapper";
            const string mapperName2 = "mapper2";
            var mapperStore = CreateMapperStore();
            var mapper = new MapperConfiguration(config => { }).CreateMapper();
            var mapper2 = new MapperConfiguration(config => { }).CreateMapper();

            mapperStore.AddMapper(mapperName, mapper);
            mapperStore.AddMapper(mapperName2, mapper2);

            mapperStore.GetMapper(mapperName).ShouldBe(mapper);
            mapperStore.GetMapper(mapperName2).ShouldBe(mapper2);
        }

        [Test]
        public void GetMapper_AddedWithApplicationBuilder()
        {
            const string mapperName = "mapper";
            var serviceProvider = CreateServiceProvider();
            var applicationBuilder = CreateApplicationBuilderMock(serviceProvider);
            var mapper = new MapperConfiguration(config => { }).CreateMapper();

            applicationBuilder.AddToMapperStore(mapperName, mapper);

            serviceProvider.GetRequiredService<IMapperStore>().GetMapper(mapperName).ShouldBe(mapper);
        }

        [Test]
        public void GetMapper_AddedConfigWithApplicationBuilder()
        {
            const string mapperName = "mapper";
            var serviceProvider = CreateServiceProvider();
            var applicationBuilder = CreateApplicationBuilderMock(serviceProvider);

            applicationBuilder.AddToMapperStore(mapperName, config => config.CreateMap<Item, ItemDto>());

            var mapper = serviceProvider.GetRequiredService<IMapperStore>().GetMapper(mapperName);
            var itemDto = mapper.Map<ItemDto>(new Item { Id = 2 });
            itemDto.Id.ShouldBe(2);
        }

        [Test]
        public void AddMapper_TwiceSameName()
        {
            const string mapperName = "mapper";
            var mapperStore = CreateMapperStore();
            mapperStore.AddMapper(mapperName, new MapperConfiguration(config => { }).CreateMapper());

            Should.Throw<InvalidOperationException>(() =>
                mapperStore.AddMapper(mapperName, new MapperConfiguration(config => { }).CreateMapper()))
                .Message.ShouldBe($"EntityWebApi: Mapping '{mapperName}' was already added.");
        }


        private static IServiceProvider CreateServiceProvider()
        {
            return new ServiceCollection()
                .AddMapperStore()
                .BuildServiceProvider();
        }
        private static IMapperStore CreateMapperStore()
        {
            return CreateServiceProvider().GetRequiredService<IMapperStore>();
        }
        private static IApplicationBuilder CreateApplicationBuilderMock(IServiceProvider serviceProvider)
        {
            var mock = new Mock<IApplicationBuilder>();
            mock.Setup(b => b.ApplicationServices).Returns(serviceProvider);
            return mock.Object;
        }
    }
}