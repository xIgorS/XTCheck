# XTCheck.Api Copilot Instructions

## Project Overview
- **Type:** .NET 10.0 Web API
- **Language:** C#
- **Architecture:** Layered (Controllers -> Services -> Repositories)
- **Data Access:** Raw ADO.NET (`Microsoft.Data.SqlClient`) within Repositories.
- **Infrastructure:** Docker Compose for SQL Server 2022.

## Architectural Patterns

### Layered Structure
Follow the established breakdown for new features:
1.  **Controllers (`Controllers/`):** Thin entry points. Use `[ApiController]` and `Route` attributes. Delegate all logic to Services.
    -   *Example:* `LogController` injects `ILogService`.
2.  **Services (`Services/`):** Contains business logic.
    -   *Example:* `LogService` injects `ILogRepository`.
3.  **Data Access (`Data/`):** Direct database interaction using ADO.NET.
    -   *Example:* `LogRepository` injects `IDbConnectionFactory`.
4.  **Connection Management:** 
    -   Use `IDbConnectionFactory` to create connections. 
    -   Ensure connections are properly disposed (use `using` statements).
    -   Prefer `OpenAsync` when possible (may require casting `IDbConnection` to `SqlConnection`).

### Dependency Injection
-   **Strict Interface Usage:** Every service and repository MUST have a corresponding interface (`IClassName`).
-   **Registration:** Register all services in `Program.cs`. 
    -   Repositories: `AddScoped`
    -   Services: `AddScoped`
    -   Infrastructure: `AddSingleton` (e.g., `DbConnectionFactory`)

## Coding Conventions

### Data Access (ADO.NET)
-   Avoid ORMs (EF Core/Dapper) unless requested; use raw `SqlCommand` and `SqlDataReader`.
-   Always use parameterized queries (`SqlParameter`) to prevent SQL injection.
-   Use full async stack: `ExecuteScalarAsync`, `ExecuteReaderAsync`, `OpenAsync`.
    ```csharp
    using var connection = _connectionFactory.CreateConnection();
    if (connection is SqlConnection sqlConn) await sqlConn.OpenAsync();
    else connection.Open();
    ```

### Error Handling
-   Controllers should generally catch exceptions and return appropriate HTTP status codes (e.g., 500 for internal errors).
-   Use standard `IActionResult` return types.

## Development Workflow

### Infrastructure
-   **Database:** MS SQL Server running in Docker.
-   **Start DB:** `docker-compose up -d`
-   **Connection String:** Defined in `appsettings.Development.json` under `DefaultConnection`.

### Building and Running
-   **Run:** `dotnet run --project XTCheck.Api`
-   **Build:** `dotnet build`
-   **Tests:** The solution references a `XTCheck.Tests` project, but the folder may be missing. If asked to test, check for existence first or verify if the user wants to scaffold it.

### Common Tasks
-   **Adding a Feature:**
    1.  Define Model/DTO (if needed).
    2.  Create Repository Interface & Implementation (ADO.NET logic).
    3.  Create Service Interface & Implementation (Logic).
    4.  Create Controller.
    5.  Register in `Program.cs`.
