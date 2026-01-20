using Microsoft.Data.SqlClient;

namespace XTCheck.Api.Data;

public interface IDbConnectionFactory
{
    Task<SqlConnection> CreateOpenConnectionAsync();
}
