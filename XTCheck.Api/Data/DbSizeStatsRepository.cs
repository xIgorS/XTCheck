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
        using var command = new SqlCommand("monitoring.spGetDbSizeStats", connection)
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
                ExtTime = reader.IsDBNull(extTimeOrdinal) ? default : reader.GetDateTime(extTimeOrdinal),
                InstanceName = reader.IsDBNull(instanceNameOrdinal) ? string.Empty : reader.GetString(instanceNameOrdinal),
                DatabaseName = reader.IsDBNull(databaseNameOrdinal) ? string.Empty : reader.GetString(databaseNameOrdinal),
                LogicalFileName = reader.IsDBNull(logicalFileNameOrdinal) ? string.Empty : reader.GetString(logicalFileNameOrdinal),
                FileGroup = reader.IsDBNull(fileGroupOrdinal) ? string.Empty : reader.GetString(fileGroupOrdinal),
                PhysicalFileName = reader.IsDBNull(physicalFileNameOrdinal) ? string.Empty : reader.GetString(physicalFileNameOrdinal),
                FileType = reader.IsDBNull(fileTypeOrdinal) ? string.Empty : reader.GetString(fileTypeOrdinal),
                AllocatedSpaceMB = reader.IsDBNull(allocatedSpaceMBOrdinal) ? 0 : Convert.ToInt64(reader.GetDecimal(allocatedSpaceMBOrdinal)),
                UsedSpaceMB = reader.IsDBNull(usedSpaceMBOrdinal) ? 0 : Convert.ToInt64(reader.GetDecimal(usedSpaceMBOrdinal)),
                FreeSpaceMB = reader.IsDBNull(freeSpaceMBOrdinal) ? 0 : Convert.ToInt64(reader.GetDecimal(freeSpaceMBOrdinal)),
                UsedPercent = reader.IsDBNull(usedPercentOrdinal) ? 0 : reader.GetInt32(usedPercentOrdinal),
                MaxSizeMB = reader.IsDBNull(maxSizeMBOrdinal) ? 0 : Convert.ToInt64(reader.GetDecimal(maxSizeMBOrdinal)),
                AutogrowSize = reader.IsDBNull(autogrowSizeOrdinal) ? string.Empty : reader.GetString(autogrowSizeOrdinal),
                TotalDriveMB = reader.IsDBNull(totalDriveMBOrdinal) ? 0 : Convert.ToInt64(reader.GetDecimal(totalDriveMBOrdinal)),
                FreeDriveMB = reader.IsDBNull(freeDriveMBOrdinal) ? 0 : Convert.ToInt64(reader.GetDecimal(freeDriveMBOrdinal)),
                FreeDrivePercent = reader.IsDBNull(freeDrivePercentOrdinal) ? 0 : Convert.ToInt64(reader.GetDecimal(freeDrivePercentOrdinal))
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
                AllocatedDBSpaceMB = reader.GetInt32(allocatedDBSpaceMBOrdinal), // This is cast to INT in SQL
                UsedDBSpaceMB = Convert.ToInt64(reader.GetDecimal(usedDBSpaceMBOrdinal)),
                FreeDBSpaceMB = Convert.ToInt64(reader.GetDecimal(freeDBSpaceMBOrdinal)),
                AutogrowEnabled = reader.GetInt32(autogrowEnabledOrdinal) == 1,
                FreeDriveMB = Convert.ToInt64(reader.GetDecimal(freeDriveMBOrdinal)),
                PartSizeMB = reader.GetInt32(partSizeMBOrdinal), // This is likely INT
                TotalFreeSpaceMB = Convert.ToInt64(reader.GetDecimal(totalFreeSpaceMBOrdinal)),
                AlertLevel = reader.GetString(alertLevelOrdinal)
            });
        }

        return results;
    }
}
