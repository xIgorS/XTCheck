# Review Notes

## Summary
- Hide internal error details from clients; return standard problem responses and log details server-side.
- Make the DB connection factory async-friendly to avoid sync fallbacks and casts.
- Add logging in controller and repository for observability; keep service thin.
- Preserve fail-fast on missing connection string but log the startup error clearly.

## File-by-file guidance
- XTCheck.Api/Controllers/LogController.cs
  - Inject ILogger<LogController>.
  - On failure, return a problem-details-style response (status/reason) without ex.Message.
  - Log failures with exception detail; log successful checks at debug/info as needed.

- XTCheck.Api/Data/IDbConnectionFactory.cs
  - Expose an async-capable API (e.g., CreateOpenConnectionAsync returning DbConnection or similar) instead of only IDbConnection.

- XTCheck.Api/Data/DbConnectionFactory.cs
  - Implement the async creation/open method using SqlConnection.OpenAsync.
  - Fail fast if DefaultConnection is missing; log a clear startup error before throwing.

- XTCheck.Api/Data/LogRepository.cs
  - Inject ILogger<LogRepository>.
  - Use the new async factory method; remove sync fallback and casts.
  - Keep the query async (ExecuteScalarAsync) and log start/finish and exceptions (no PII).

## Rationale
- Security: Avoid leaking internal exception details in HTTP responses.
- Performance/Scalability: Consistent async path prevents thread-pool blocking under load.
- Observability: Logging in controller/repository captures both API-level and DB-level failures.
- Operability: Clear startup logging makes misconfiguration obvious (missing connection string).