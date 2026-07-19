# EntityWebApi.MinimalApi

Part of [EntityWebApi](https://github.com/GrumpTech/entity-web-api).

EntityWebApi.MinimalApi supports registering generic handlers by resolving their type arguments.

Register services for EntityWebApi.MinimalApi and all generic handlers in an assembly:

`builder.Services.AddEntityMinimalApi(typeof(Program).Assembly);`

Configure the generated handlers:

```
app.UseEntityMinimalApi(options => options
    .SetDefaultArgumentResolver((argumentName, entityType) =>
        typeStore.GetRequired($"{entityType.Name}{argumentName[1..]}"))
    .AddArgumentResolver("TDbContext", _ => typeof(DataContext))
    .AddArgumentResolver("TEntity", entityType => entityType)
    .SetTagTemplate("Rest - [Entity]")
    .AddHandlers(typeof(GetAllHandler<,,>), entityTypes)
    .AddHandlers(typeof(GetHandler<,,,>), entityTypes)
    .AddHandlers(typeof(PostHandler<,,,>), entityTypes)
    .AddHandlers(typeof(PutHandler<,,,,>), entityTypes)
    .AddHandlers(typeof(PatchHandler<,,,,>), entityTypes)
    .AddHandlers(typeof(DeleteHandler<,,>), entityTypes)
);
```

- **SetDefaultArgumentResolver** Configures how unresolved generic type arguments are mapped to concrete types. This resolver is invoked whenever no explicit resolver has been registered for a generic argument.
- **AddArgumentResolver** Registers a resolver for a specific generic type parameter. In this example: TDbContext resolves to DataContext.
- **AddHandlers** Registers a handler for multiple types.
- **AddHandler** Registers a handler for a single type.
- **SetTagTemplate** Sets the OpenAPI/Swagger tag applied to generated endpoints. The placeholder [Entity] is replaced with the entity name.