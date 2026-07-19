using EntityWebApi.AutoMapper;
using EntityWebApi.Controllers;
using EntityWebApi.Dtos;
using Infrastructure;
using Infrastructure.Attributes;
using Infrastructure.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Linq;

namespace WebApi
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<DataContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")).LogTo(Console.WriteLine);
            });
            services.AddControllers();
            services.AddMapperStore();
            services.AddEntityWebApi();
            services.AddSwaggerGen();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, DataContext dataContext)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            var typeStore = new DtoBuilder()
                .CreateKeyDtos(dataContext)
                .CreateDtos(dataContext)
                .CreatePostDtos(dataContext)
                .CreatePutDtos(dataContext)
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
                .AddControllers(typeof(RestController<,,,,,>), entityTypes)
            );

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            dataContext.Database.Migrate();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
