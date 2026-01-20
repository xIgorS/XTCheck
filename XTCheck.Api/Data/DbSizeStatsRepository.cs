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
                var driveOrdinal = reader.GetOrdinal("Drive");
                var totalDriveGBOrdinal = reader.GetOrdinal("TotalDriveGB");
                var freeDriveGBOrdinal = reader.GetOrdinal("FreeDriveGB");
                var freePercentOrdinal = reader.GetOrdinal("FreePercent");

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
                    Drive = reader.IsDBNull(driveOrdinal) ? "N/A" : reader.GetString(driveOrdinal),
                    TotalDriveGB = reader.IsDBNull(totalDriveGBOrdinal) ? 0 : reader.GetInt64(totalDriveGBOrdinal),
                    FreeDriveGB = reader.IsDBNull(freeDriveGBOrdinal) ? 0 : reader.GetInt64(freeDriveGBOrdinal),
                    FreePercent = reader.IsDBNull(freePercentOrdinal) ? 0 : reader.GetDecimal(freePercentOrdinal)
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
                var driveOrdinal = reader.GetOrdinal("Drive");
                var totalDriveGBOrdinal = reader.GetOrdinal("TotalDriveGB");
                var freeDriveGBOrdinal = reader.GetOrdinal("FreeDriveGB");
                var freePercentOrdinal = reader.GetOrdinal("FreePercent");

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
                    Drive = reader.IsDBNull(driveOrdinal) ? "N/A" : reader.GetString(driveOrdinal),
                    TotalDriveGB = reader.IsDBNull(totalDriveGBOrdinal) ? 0 : reader.GetInt64(totalDriveGBOrdinal),
                    FreeDriveGB = reader.IsDBNull(freeDriveGBOrdinal) ? 0 : reader.GetInt64(freeDriveGBOrdinal),
                    FreePercent = reader.IsDBNull(freePercentOrdinal) ? 0 : reader.GetDecimal(freePercentOrdinal)
                });
            }
        }

        return results;
    }
}
