# EntityWebApi.EFCore

Part of [EntityWebApi](https://github.com/GrumpTech/entity-web-api).

EntityWebApi.EFCore adds Entity Framework Core integration to EntityWebApi, providing utilities for querying entities and generating key-based expressions using EF Core model metadata.

- **WhereHasKey** Filters a DbSet<TEntity> to the entity whose primary key matches the supplied key object.

- **HasKeyExpression** Creates an Expression<Func<TEntity, bool>> that evaluates to true when an entity's primary key matches the supplied key object.

- **WhereHasOneOfKeys** Filters a DbSet<TEntity> to entities whose primary key matches one of the supplied key objects. 

- **HasOneOfKeysExpression** Creates an Expression<Func<TEntity, bool>> that evaluates to true when an entity's primary key matches one of the supplied key objects. 

**Notes**

- The primary key is resolved from the EF Core model; no key property names need to be specified.
- Supports both single-column and composite primary keys.
- The expression methods can be combined with other LINQ expressions.