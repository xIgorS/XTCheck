using System.Data;
using Microsoft.Data.SqlClient;
using XTCheck.Api.Models;

namespace XTCheck.Api.Data;

public interface IDbSizeStatsRepository
{
    Task<IEnumerable<DbSizeAlertStats>> GetDbSizePlusDiskAsync();
}

public class DbSizeStatsRepository : IDbSizeStatsRepository
{
    private readonly IDbConnectionFactory _connectionFactory;

    public DbSizeStatsRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
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
                AllocatedDBSpaceMB = reader.GetInt32(allocatedDBSpaceMBOrdinal),
                UsedDBSpaceMB = (long)reader.GetDecimal(usedDBSpaceMBOrdinal),
                FreeDBSpaceMB = (long)reader.GetDecimal(freeDBSpaceMBOrdinal),
                AutogrowEnabled = reader.GetInt32(autogrowEnabledOrdinal) == 1,
                FreeDriveMB = (long)reader.GetDecimal(freeDriveMBOrdinal),
                PartSizeMB = reader.GetInt32(partSizeMBOrdinal),
                TotalFreeSpaceMB = (long)reader.GetDecimal(totalFreeSpaceMBOrdinal),
                AlertLevel = reader.GetString(alertLevelOrdinal)
            });
        }

        return results;
    }
}
