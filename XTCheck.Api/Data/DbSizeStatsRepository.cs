using System.Data;
using Microsoft.Data.SqlClient;
using XTCheck.Api.Models;

namespace XTCheck.Api.Data;

public interface IDbSizeStatsRepository
{
    Task<IEnumerable<DbSizeStats>> GetDbSizeStatsAsync();
    Task<IEnumerable<DbSizeAlertStats>> GetDbSizePlusDiskAsync();
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
        
        var extTimeOrdinal = reader.GetOrdinal("ExtTime");
        var instanceNameOrdinal = reader.GetOrdinal("InstanceName");
        var databaseNameOrdinal = reader.GetOrdinal("DatabaseName");
        var logicalFileNameOrdinal = reader.GetOrdinal("LogicalFileName");
        var fileGroupOrdinal = reader.GetOrdinal("FileGroup");
        var physicalFileNameOrdinal = reader.GetOrdinal("PhysicalFileName");
        var fileTypeOrdinal = reader.GetOrdinal("FileType");
        var allocatedSpaceMBOrdinal = reader.GetOrdinal("AllocatedSpaceMB");
        var usedSpaceMBOrdinal = reader.GetOrdinal("UsedSpaceMB");
        var freeSpaceMBOrdinal = reader.GetOrdinal("FreeSpaceMB");
        var usedPercentOrdinal = reader.GetOrdinal("UsedPercent");
        var maxSizeMBOrdinal = reader.GetOrdinal("MaxSizeMB");
        var autogrowSizeOrdinal = reader.GetOrdinal("AutogrowSize");
        var totalDriveMBOrdinal = reader.GetOrdinal("TotalDriveMB");
        var freeDriveMBOrdinal = reader.GetOrdinal("FreeDriveMB");
        var freeDrivePercentOrdinal = reader.GetOrdinal("FreeDrivePercent");

        while (await reader.ReadAsync())
        {
            results.Add(new DbSizeStats
            {
                ExtTime = reader.GetDateTime(extTimeOrdinal),
                InstanceName = reader.GetString(instanceNameOrdinal),
                DatabaseName = reader.GetString(databaseNameOrdinal),
                LogicalFileName = reader.GetString(logicalFileNameOrdinal),
                FileGroup = reader.GetString(fileGroupOrdinal),
                PhysicalFileName = reader.GetString(physicalFileNameOrdinal),
                FileType = reader.GetString(fileTypeOrdinal),
                AllocatedSpaceMB = reader.GetDecimal(allocatedSpaceMBOrdinal),
                UsedSpaceMB = reader.GetDecimal(usedSpaceMBOrdinal),
                FreeSpaceMB = reader.GetDecimal(freeSpaceMBOrdinal),
                UsedPercent = reader.GetInt32(usedPercentOrdinal),
                MaxSizeMB = reader.GetDecimal(maxSizeMBOrdinal),
                AutogrowSize = reader.GetString(autogrowSizeOrdinal),
                TotalDriveMB = reader.GetDecimal(totalDriveMBOrdinal),
                FreeDriveMB = reader.GetDecimal(freeDriveMBOrdinal),
                FreeDrivePercent = reader.GetDecimal(freeDrivePercentOrdinal)
            });
        }

        return results;
    }

    public async Task<IEnumerable<DbSizeAlertStats>> GetDbSizePlusDiskAsync()
    {
        var results = new List<DbSizeAlertStats>();

        await using var connection = await _connectionFactory.CreateOpenConnectionAsync();
        using var command = new SqlCommand("monitoring.GetDBSizePlusDisk", connection)
        {
            CommandType = CommandType.StoredProcedure
        };

        using var reader = await command.ExecuteReaderAsync();

        var databaseNameOrdinal = reader.GetOrdinal("DatabaseName");
        var fileGroupOrdinal = reader.GetOrdinal("FileGroup");
        var allocatedDBSpaceMBOrdinal = reader.GetOrdinal("AllocatedDBSpaceMB");
        var usedDBSpaceMBOrdinal = reader.GetOrdinal("UsedDBSpaceMB");
        var freeDBSpaceMBOrdinal = reader.GetOrdinal("FreeDBSpaceMB");
        var autogrowEnabledOrdinal = reader.GetOrdinal("AutogrowEnabled");
        var freeDriveMBOrdinal = reader.GetOrdinal("FreeDriveMB");
        var partSizeMBOrdinal = reader.GetOrdinal("PartSizeMB");
        var totalFreeSpaceMBOrdinal = reader.GetOrdinal("TotalFreeSpaceMB");
        var alertLevelOrdinal = reader.GetOrdinal("AlertLevel");

        while (await reader.ReadAsync())
        {
            results.Add(new DbSizeAlertStats
            {
                DatabaseName = reader.GetString(databaseNameOrdinal),
                FileGroup = reader.GetString(fileGroupOrdinal),
                AllocatedDBSpaceMB = reader.GetInt64(allocatedDBSpaceMBOrdinal),
                UsedDBSpaceMB = reader.GetInt64(usedDBSpaceMBOrdinal),
                FreeDBSpaceMB = reader.GetInt64(freeDBSpaceMBOrdinal),
                AutogrowEnabled = reader.GetInt32(autogrowEnabledOrdinal) == 1,
                FreeDriveMB = reader.GetInt64(freeDriveMBOrdinal),
                PartSizeMB = reader.GetInt64(partSizeMBOrdinal),
                TotalFreeSpaceMB = reader.GetInt64(totalFreeSpaceMBOrdinal),
                AlertLevel = reader.GetString(alertLevelOrdinal)
            });
        }

        return results;
    }
}
