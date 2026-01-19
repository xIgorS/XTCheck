using System.Data;
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
        using var connection = _connectionFactory.CreateConnection();
        // Since IDbConnection doesn't have OpenAsync, we cast or use specific type if we want async, 
        // or just wrapper. But SqlConnection DOES have OpenAsync.
        // For simplicity with IDbConnection interface, we might use Open() or cast.
        // Let's cast for async support since we know it's SqlConnection under the hood or change interface return type.
        // However, standard IDbConnection usage typically involves Dapper or similar.
        // Setup raw ADO.NET:
        
        if (connection is SqlConnection sqlConnection)
        {
            await sqlConnection.OpenAsync();
            using var command = new SqlCommand("SELECT @@VERSION", sqlConnection);
            var result = await command.ExecuteScalarAsync();
            return result?.ToString() ?? "Unknown";
        }
        else 
        {
            connection.Open();
            using var command = connection.CreateCommand();
            command.CommandText = "SELECT @@VERSION";
            var result = command.ExecuteScalar();
            return result?.ToString() ?? "Unknown";
        }
    }
}
