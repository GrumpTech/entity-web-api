# EntityWebApi.Dtos

Part of [EntityWebApi](https://github.com/GrumpTech/entity-web-api).

EntityWebApi.Dtos enables dynamic generation of DTO types from Entity Framework Core entity models. It supports creating multiple DTO variants with configuration and attributes.

- `Build()`: Creates and returns the generated DTO types in a `TypeStore`.
- `CreateDtos(DbContext dbContext, bool allFields = false, string suffix = "Dto", string? includeSuffix = null)`:
Creates standard DTO types from Entity Framework Core entities with configurable field inclusion and naming.
- `CreateKeyDtos(DbContext dbContext, string suffix = "KeyDto")`:
Creates DTO types containing only primary key properties.
- `CreatePostDtos(DbContext dbContext, bool allFields = false, string suffix = "PostDto", string includeSuffix = "Dto")`:
Creates DTO types for create operations.
- `CreatePutDtos(DbContext dbContext, bool allFields = false, string suffix = "PutDto", string includeSuffix = "Dto")`:
Creates DTO types for update operations.
- `CreateDtos<TDtoPropertyAttribute>(DbContext dbContext, bool allFields = false, string suffix = "Dto", string? includeSuffix = null)`:
Creates DTO types using a custom DTO property attribute for property configuration.

- `CreateKeyDtos<TDtoPropertyAttribute>(DbContext dbContext, string suffix = "KeyDto")`:
Creates key DTO types using a custom DTO property attribute.

- `CreatePostDtos<TDtoPropertyAttribute>(DbContext dbContext, bool allFields = false, string suffix = "PostDto", string includeSuffix = "Dto")`:
Creates post DTO types using a custom DTO property attribute.

- `CreatePutDtos<TDtoPropertyAttribute>(DbContext dbContext, bool allFields = false, string suffix = "PutDto", string includeSuffix = "Dto")`:
Creates put DTO types using a custom DTO property attribute.

- `CreateDtos(DbContext dbContext, TypeConfiguration configuration)`:
Creates DTO types from EF Core entities using the provided configuration.