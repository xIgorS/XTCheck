using System.Data;
using Microsoft.Data.SqlClient;
using XTCheck.Api.Models;

namespace XTCheck.Api.Data;

public interface IDbSizeStatsRepository
{
    Task<IEnumerable<DbSizeStats>> GetDbSizeStatsAsync();
}

public class DbSizeStatsRepository : IDbSizeStatsRepository
{
    private readonly IDbConnectionFactory _connectionFactory;

    public DbSizeStatsRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<IEnumerable<DbSizeStats>> GetDbSizeStatsAsync()
    {
        var results = new List<DbSizeStats>();

        using var connection = _connectionFactory.CreateConnection();
        
        if (connection is SqlConnection sqlConnection)
        {
            await sqlConnection.OpenAsync();
            using var command = new SqlCommand("dbo.spGetDbSizeStats", sqlConnection)
            {
                CommandType = CommandType.StoredProcedure
            };

            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                results.Add(new DbSizeStats
                {
                    DatabaseName = reader.GetString(reader.GetOrdinal("DatabaseName")),
                    LogicalFileName = reader.GetString(reader.GetOrdinal("LogicalFileName")),
                    FileGroup = reader.GetString(reader.GetOrdinal("FileGroup")),
                    PhysicalFileName = reader.GetString(reader.GetOrdinal("PhysicalFileName")),
                    FileType = reader.GetString(reader.GetOrdinal("FileType")),
                    DatabaseFileSizeMB = reader.GetDecimal(reader.GetOrdinal("DatabaseFileSizeMB")),
                    MaxSize = reader.GetString(reader.GetOrdinal("MaxSize")),
                    AutogrowSize = reader.GetString(reader.GetOrdinal("AutogrowSize")),
                    Drive = reader.GetString(reader.GetOrdinal("Drive")),
                    TotalDriveGB = reader.GetInt64(reader.GetOrdinal("TotalDriveGB")),
                    FreeDriveGB = reader.GetInt64(reader.GetOrdinal("FreeDriveGB")),
                    FreePercent = reader.GetDecimal(reader.GetOrdinal("FreePercent"))
                });
            }
        }
        else
        {
            connection.Open();
            using var command = connection.CreateCommand();
            command.CommandText = "dbo.spGetDbSizeStats";
            command.CommandType = CommandType.StoredProcedure;

            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                results.Add(new DbSizeStats
                {
                    DatabaseName = reader.GetString(reader.GetOrdinal("DatabaseName")),
                    LogicalFileName = reader.GetString(reader.GetOrdinal("LogicalFileName")),
                    FileGroup = reader.GetString(reader.GetOrdinal("FileGroup")),
                    PhysicalFileName = reader.GetString(reader.GetOrdinal("PhysicalFileName")),
                    FileType = reader.GetString(reader.GetOrdinal("FileType")),
                    DatabaseFileSizeMB = reader.GetDecimal(reader.GetOrdinal("DatabaseFileSizeMB")),
                    MaxSize = reader.GetString(reader.GetOrdinal("MaxSize")),
                    AutogrowSize = reader.GetString(reader.GetOrdinal("AutogrowSize")),
                    Drive = reader.GetString(reader.GetOrdinal("Drive")),
                    TotalDriveGB = reader.GetInt64(reader.GetOrdinal("TotalDriveGB")),
                    FreeDriveGB = reader.GetInt64(reader.GetOrdinal("FreeDriveGB")),
                    FreePercent = reader.GetDecimal(reader.GetOrdinal("FreePercent"))
                });
            }
        }

        return results;
    }
}
