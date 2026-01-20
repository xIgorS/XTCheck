using Microsoft.Data.SqlClient;

namespace XTCheck.Api.Data;

public interface ILogRepository
{
    Task<string> GetDatabaseVersionAsync();
}

public class LogRepository : ILogRepository
{
    private readonly IDbConnectionFactory _connectionFactory;

    public LogRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<string> GetDatabaseVersionAsync()
    {
        await using var connection = await _connectionFactory.CreateOpenConnectionAsync();
        using var command = new SqlCommand("SELECT @@VERSION", connection);
        var result = await command.ExecuteScalarAsync();
        return result?.ToString() ?? "Unknown";
    }
}
