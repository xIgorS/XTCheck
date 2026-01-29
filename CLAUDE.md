# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

XTCheck is a .NET 10.0 Web API for database monitoring and logging.

- **Language:** C# with nullable reference types enabled
- **Architecture:** Layered (Controllers → Services → Repositories)
- **Data Access:** Raw ADO.NET using `Microsoft.Data.SqlClient` (no ORM)
- **Database:** SQL Server 2022 running in Docker via `docker-compose.yml`
- **Database Name:** `Log` (see connection string in `appsettings.json`)

## Development Commands

### Infrastructure
```bash
# Start SQL Server
docker-compose up -d

# Stop SQL Server
docker-compose down
```

### Build and Run
```bash
# Build the solution
dotnet build

# Run the API
dotnet run --project XTCheck.Api

# Build specific project
dotnet build XTCheck.Api/XTCheck.Api.csproj
```

### Testing
The solution references a `XTCheck.Tests` project in the .sln file, but the project folder may not exist. Verify existence before running tests.

## Architecture

### Layered Structure
The codebase follows strict separation of concerns:

1. **Controllers/** - Thin HTTP entry points
   - Use `[ApiController]` and `[Route("api/[controller]")]` attributes
   - Inject service interfaces (e.g., `ILogService`)
   - Handle exceptions and return appropriate HTTP status codes
   - Delegate all business logic to services

2. **Services/** - Business logic layer
   - Each service has an interface (e.g., `ILogService` → `LogService`)
   - Inject repository interfaces
   - Keep logic focused and testable

3. **Data/** - Data access layer using raw ADO.NET
   - Each repository has an interface (e.g., `ILogRepository` → `LogRepository`)
   - Inject `IDbConnectionFactory` to create connections
   - Use parameterized queries exclusively (prevent SQL injection)
   - Prefer async methods: `ExecuteScalarAsync`, `ExecuteReaderAsync`
   - Pattern: `await using var connection = await _connectionFactory.CreateOpenConnectionAsync();`

4. **Models/** - DTOs and domain models

5. **Database/StoredProcedures/** - SQL stored procedure definitions

### Connection Management
- Use `IDbConnectionFactory.CreateOpenConnectionAsync()` which returns an already-opened connection
- Always use `await using` for automatic disposal
- Connection string is in `appsettings.json` under `ConnectionStrings:DefaultConnection`

## Dependency Injection Patterns

All services and repositories MUST follow this pattern:
- Define an interface (`IClassName`)
- Implement the interface
- Register in [Program.cs:8-12](XTCheck.Api/Program.cs#L8-L12):
  - Infrastructure: `AddSingleton` (e.g., `DbConnectionFactory`)
  - Services: `AddScoped` (e.g., `LogService`)
  - Repositories: `AddScoped` (e.g., `LogRepository`)

## Data Access Conventions

**IMPORTANT:** Avoid ORMs (EF Core/Dapper) unless explicitly requested. Use raw ADO.NET:

```csharp
await using var connection = await _connectionFactory.CreateOpenConnectionAsync();
using var command = new SqlCommand("SELECT @@VERSION", connection);
var result = await command.ExecuteScalarAsync();
```

Key practices:
- Always use parameterized queries with `SqlParameter`
- Use full async stack throughout
- Dispose connections properly with `await using`

## Adding New Features

Follow this workflow:
1. Define Model/DTO in `Models/` (if needed)
2. Create repository interface and implementation in `Data/`
   - Interface: `IXRepository`
   - Implementation: Use ADO.NET, inject `IDbConnectionFactory`
3. Create service interface and implementation in `Services/`
   - Interface: `IXService`
   - Implementation: Inject repository interface
4. Create controller in `Controllers/`
   - Inherit from `ControllerBase`
   - Use `[ApiController]` attribute
   - Inject service interface
5. Register all new services in `Program.cs` with `AddScoped`

## Database Setup

- **Server:** localhost:1433
- **Database:** Log
- **Auth:** sa / DockerPassword123!
- **Docker:** Required for Apple Silicon compatibility (platform: linux/amd64)
- **Stored Procedures:** Located in `Database/StoredProcedures/`
  - Example: `spGetDbSizeStats.sql` - retrieves database file size statistics
