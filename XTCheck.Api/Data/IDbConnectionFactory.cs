using System.Data;
using Microsoft.Data.SqlClient;

namespace XTCheck.Api.Data;

public interface IDbConnectionFactory
{
    IDbConnection CreateConnection();
}
