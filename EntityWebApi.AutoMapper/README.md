# EntityWebApi.AutoMapper

Part of [EntityWebApi](https://github.com/GrumpTech/entity-web-api).

This library automates object mapping through naming conventions, making it easy to map between Entity Framework Core entities and DTOs with minimal configuration.

- `services.AddMapperStore()`: Registers the application's mapper store in the dependency injection container, providing a central registry where named mapper configurations can be added, and later retrieved throughout the application.
- `app.AddToMapperStore("EntityMapper", config => {})`: Creates and registers a named mapper called EntityMapper, allowing you to define all mapping rules in one place and retrieve the configured mapper later from the mapper store.
- `AddEntityFrameworkCoreCollectionMappers()`: Registers collection mappers that correctly update Entity Framework Core collections instead of replacing them. This enables proper add, update, and remove operations on navigation collections.
- `CreateEntityMaps(dataContext, typeStore, "Dto")`: Creates mappings from Entity Framework Core entities to DTO classes in the typeStore whose names end with Dto.
- `CreateMapsToEntity(dataContext, typeStore, "PutDto")`: Creates mappings from DTO classes in the typeStore whose names end with PutDto to entities.