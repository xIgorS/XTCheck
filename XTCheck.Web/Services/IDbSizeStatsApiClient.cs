using XTCheck.Web.Models;

namespace XTCheck.Web.Services;

public interface IDbSizeStatsApiClient
{
    Task<IEnumerable<DbSizeStats>> GetDbSizeStatsAsync();
    Task<IEnumerable<DatabaseSizeAggregate>> GetAggregatedDbSizeStatsAsync();
    Task<IEnumerable<FileGroupAggregate>> GetFileGroupStatsAsync(string databaseName);
    Task<IEnumerable<DiskSpaceInfo>> GetDiskSpaceInfoAsync();
}
