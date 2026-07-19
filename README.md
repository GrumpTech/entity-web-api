# EntityWebApi

Generate web APIs from your entities with minimal code.

EntityWebApi helps you expose your domain entities through HTTP endpoints using your own generic controllers or Minimal API handlers. It provides a flexible foundation for building entity-driven APIs while keeping your application architecture clean and maintainable.

## Getting started

### Commands

See [package.json](/package.json) for scripts to build the mustache example layouts.

- Manage user secrets
  dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Server=localhost,1433;Database=TestDb;User Id=sa;Password=[Password];TrustServerCertificate=True"
```
{
    "ConnectionStrings": {
        "DefaultConnection":"Server=localhost,1433;Database=TestDb;User Id=sa;Password=[Password];TrustServerCertificate=True"
    }
}
```
- Run `docker run -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=[Password]" -p 1433:1433 -d mcr.microsoft.com/mssql/server:2022-latest` to start a sql server.
- Run `dotnet ef migrations add [Name]` in the [Infrastructure](/Infrastructure/) folder to create migrations.
- Run `dotnet ef dbcontext scaffold "Data Source=localhost,1433;Initial Catalog=[name];User Id=sa;Password=[Password];TrustServerCertificate=True" Microsoft.EntityFrameworkCore.SqlServer`


- Run `dotnet run` in the [WebApi](/WebApi/) or [MinimalApi](/MinimalApi/) folder to run the web api.

### Source code

For a correct database definition make strings explicitely nullable with a question mark or non-nullable with [Required].

