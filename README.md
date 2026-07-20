# EntityWebApi

**Generate web APIs from your entities with minimal code.**

EntityWebApi enables you to expose domain entities through HTTP endpoints using generic controllers or Minimal API handlers, providing a flexible foundation for clean and maintainable entity-driven APIs.

**Important:** This project is **not production-ready**, but it is suitable for fast prototyping. If you find it useful, consider starring the project on GitHub. The project is also open to sponsorship.

## Getting started

### Demos

See [web API](/WebApi/) or [minimal API](/MinimalApi/).

### Commands

- Start a SQL Server instance:
  ```
  docker run -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=[Password]" -p 1433:1433 -d mcr.microsoft.com/mssql/server:2022-latest
  ```

- Add the connectionstring as a user secret:
  ```
  dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Server=localhost,1433;Database=TestDb;User Id=sa;Password=[Password];TrustServerCertificate=True"
  ```

- Create a database migration from the [Infrastructure folder](/Infrastructure/):
  ```
  dotnet ef migrations add [Name]
  ```

- Start an example [web API](/WebApi/) or [minimal API](/MinimalApi/):
  ```
  dotnet run
  ```

- Scaffold entities from an existing database
  ```
  dotnet ef dbcontext scaffold "Data Source=localhost,1433;Initial Catalog=[name];User Id=sa;Password=[Password];TrustServerCertificate=True" Microsoft.EntityFrameworkCore.SqlServer
  ```
**Note:** For a correct database definition make strings explicitely nullable with a question mark or non-nullable with a [Required] attribute.

### APIs

The APIs are documented for each library individually.

- [EntityWebApi.AutoMapper](/EntityWebApi.AutoMapper)
- [EntityWebApi.Controllers](/EntityWebApi.Controllers)
- [EntityWebApi.Core](/EntityWebApi.Core)
- [EntityWebApi.Dtos](/EntityWebApi.Dtos)
- [EntityWebApi.EFCore](/EntityWebApi.EFCore)
- [EntityWebApi.MinimalApi](/EntityWebApi.MinimalApi)