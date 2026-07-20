# EntityWebApi.Core

Part of [EntityWebApi](https://github.com/GrumpTech/entity-web-api).

This library contains the a core object, attributes, and enums of EntityWebApi

The `TypeStore` is a central registry that stores and resolves generated types, enabling the library to locate types when generating mappings or resolving generic types.

The `DtoPropertyAttribute` customizes how an entity property is generated in a DTO by allowing you to exclude it, reference a different DTO type for navigation properties, make it optional, or control whether it is read-only, write-only, or read/write.