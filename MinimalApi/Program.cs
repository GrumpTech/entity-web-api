using EntityWebApi.AutoMapper;
using EntityWebApi.Dtos;
using EntityWebApi.MinimalApi;
using Infrastructure;
using Infrastructure.Attributes;
using Infrastructure.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MinimalApi.DefaultRestHandlers;
using System;
using System.Linq;

namespace MinimalApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var services = builder.Services;
            services.AddDbContext<DataContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")).LogTo(Console.WriteLine);
            });
            services.AddAuthorization();
            services.AddMapperStore();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            services.AddEntityWebApi(typeof(Program).Assembly);

            var app = builder.Build();
            if (builder.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            using (var scope = app.Services.CreateScope())
            {
                var dataContext = scope.ServiceProvider.GetRequiredService<DataContext>();
                dataContext.Database.Migrate();
                var typeStore = new DtoBuilder().CreateKeyDtos(dataContext)
                    .CreateDtos(dataContext)
                    .CreatePostDtos(dataContext)
                    .CreatePutDtos(dataContext)
                    .CreatePutDtos<PatchDtoProperty>(dataContext, false, "PatchDto")
                    .Build();
                app.AddToMapperStore("EntityMapper", config =>
                {
                    config.AddEntityFrameworkCoreCollectionMappers(dataContext);
                    config.CreateEntityMaps(dataContext, typeStore, "Dto");
                    config.CreateMapsToEntity(dataContext, typeStore, "PostDto");
                    config.CreateMapsToEntity(dataContext, typeStore, "PutDto");
                    config.CreateMapsToEntity(dataContext, typeStore, "PatchDto");
                });
                var entityTypes = dataContext.Model.GetEntityTypes().Select(e => e.ClrType);
                app.UseEntityWebApi(options => options
                    .SetDefaultArgumentResolver((argumentName, entityType) =>
                        typeStore.GetRequired($"{entityType.Name}{argumentName[1..]}"))
                    .AddArgumentResolver("TDbContext", entityType => typeof(DataContext))
                    .AddArgumentResolver("TEntity", entityType => entityType)
                    .SetTagTemplate("Rest - [Entity]")
                    .AddHandlers(typeof(GetAllHandler<,,>), entityTypes)
                    .AddHandlers(typeof(GetHandler<,,,>), entityTypes)
                    .AddHandlers(typeof(PostHandler<,,,>), entityTypes)
                    .AddHandlers(typeof(PutHandler<,,,,>), entityTypes)
                    .AddHandlers(typeof(PatchHandler<,,,,>), entityTypes)
                    .AddHandlers(typeof(DeleteHandler<,,>), entityTypes)
                );
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.Run();
        }
    }
}
