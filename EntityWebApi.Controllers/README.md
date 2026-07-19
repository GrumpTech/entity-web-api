# EntityWebApi.Controllers

Part of [EntityWebApi](https://github.com/GrumpTech/entity-web-api).

EntityWebApi.Controllers supports registering generic controllers by resolving their type arguments.

Register services for EntityWebApi.Controllers:

`services.AddEntityWebApi();`

Configure generic entity controllers:

```
app.UseEntityWebApi(options => options
    .SetDefaultArgumentResolver((argumentName, entityType) =>
        typeStore.GetRequired($"{entityType.Name}{argumentName[1..]}"))
    .AddArgumentResolver("TDbContext", _ => typeof(DataContext))
    .AddArgumentResolver("TEntity", entityType => entityType)
    .AddControllers(typeof(RestController<,,,,,>), entityTypes)
);
```

- **SetDefaultArgumentResolver** Configures how unresolved generic type arguments are mapped to concrete types. This resolver is invoked whenever no explicit resolver has been registered for a generic argument.
- **AddArgumentResolver** Registers a resolver for a specific generic type parameter. In this example: TDbContext resolves to DataContext.
- **AddControllers** Registers controllers for multiple types.
- **AddController** Registers a controller for a single type.