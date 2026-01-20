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

        await using var connection = await _connectionFactory.CreateOpenConnectionAsync();
        using var command = new SqlCommand("dbo.spGetDbSizeStats", connection)
        {
            CommandType = CommandType.StoredProcedure
        };

        using var reader = await command.ExecuteReaderAsync();
        
        // Get column ordinals once outside the loop for better performance
        var databaseNameOrdinal = reader.GetOrdinal("DatabaseName");
        var logicalFileNameOrdinal = reader.GetOrdinal("LogicalFileName");
        var fileGroupOrdinal = reader.GetOrdinal("FileGroup");
        var physicalFileNameOrdinal = reader.GetOrdinal("PhysicalFileName");
        var fileTypeOrdinal = reader.GetOrdinal("FileType");
        var databaseFileSizeMBOrdinal = reader.GetOrdinal("DatabaseFileSizeMB");
        var maxSizeOrdinal = reader.GetOrdinal("MaxSize");
        var autogrowSizeOrdinal = reader.GetOrdinal("AutogrowSize");
        var driveOrdinal = reader.GetOrdinal("Drive");
        var totalDriveGBOrdinal = reader.GetOrdinal("TotalDriveGB");
        var freeDriveGBOrdinal = reader.GetOrdinal("FreeDriveGB");
        var freePercentOrdinal = reader.GetOrdinal("FreePercent");

        while (await reader.ReadAsync())
        {
            results.Add(new DbSizeStats
            {
                DatabaseName = reader.GetString(databaseNameOrdinal),
                LogicalFileName = reader.GetString(logicalFileNameOrdinal),
                FileGroup = reader.GetString(fileGroupOrdinal),
                PhysicalFileName = reader.GetString(physicalFileNameOrdinal),
                FileType = reader.GetString(fileTypeOrdinal),
                DatabaseFileSizeMB = reader.GetDecimal(databaseFileSizeMBOrdinal),
                MaxSize = reader.GetString(maxSizeOrdinal),
                AutogrowSize = reader.GetString(autogrowSizeOrdinal),
                Drive = reader.IsDBNull(driveOrdinal) ? "N/A" : reader.GetString(driveOrdinal),
                TotalDriveGB = reader.IsDBNull(totalDriveGBOrdinal) ? 0 : reader.GetInt64(totalDriveGBOrdinal),
                FreeDriveGB = reader.IsDBNull(freeDriveGBOrdinal) ? 0 : reader.GetInt64(freeDriveGBOrdinal),
                FreePercent = reader.IsDBNull(freePercentOrdinal) ? 0 : reader.GetDecimal(freePercentOrdinal)
            });
        }

        return results;
    }
}
