using System.Net.Http.Json;
using XTCheck.Web.Models;

namespace XTCheck.Web.Services;

public class DbSizeStatsApiClient : IDbSizeStatsApiClient
{
    private readonly HttpClient _httpClient;
    private IEnumerable<DbSizeStats>? _cachedStats;

    public DbSizeStatsApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IEnumerable<DbSizeStats>> GetDbSizeStatsAsync()
    {
        try
        {
            Console.WriteLine($"[WEB] Calling API: {_httpClient.BaseAddress}api/DbSizeStats");
            var result = await _httpClient.GetFromJsonAsync<IEnumerable<DbSizeStats>>("api/DbSizeStats");
            _cachedStats = result ?? Enumerable.Empty<DbSizeStats>();
            Console.WriteLine($"[WEB] API call successful, got {_cachedStats.Count()} records");
            return _cachedStats;
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"[WEB] HTTP Error calling API: {ex.Message}");
            if (ex.Data.Contains("StatusCode"))
                Console.WriteLine($"[WEB] Status Code: {ex.Data["StatusCode"]}");
            throw;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[WEB] General Error calling API: {ex.Message}");
            throw;
        }
    }

    public async Task<IEnumerable<DatabaseSizeAggregate>> GetAggregatedDbSizeStatsAsync()
    {
        var stats = await GetDbSizeStatsAsync();
        
        // Group by database and aggregate
        var aggregated = stats
            .GroupBy(s => new { s.InstanceName, s.DatabaseName })
            .Select(g => new DatabaseSizeAggregate
            {
                InstanceName = g.Key.InstanceName,
                DatabaseName = g.Key.DatabaseName,
                TotalAllocatedMB = g.Sum(s => s.AllocatedSpaceMB),
                TotalUsedMB = g.Sum(s => s.UsedSpaceMB),
                TotalFreeMB = g.Sum(s => s.FreeSpaceMB)
            })
            .OrderByDescending(a => a.TotalAllocatedMB)
            .ToList();

        return aggregated;
    }

    public async Task<IEnumerable<FileGroupAggregate>> GetFileGroupStatsAsync(string databaseName)
    {
        // Use cached stats if available, otherwise fetch
        var stats = _cachedStats ?? await GetDbSizeStatsAsync();
        
        // Filter by database and group by FileGroup
        var fileGroups = stats
            .Where(s => s.DatabaseName == databaseName)
            .GroupBy(s => new { s.DatabaseName, s.FileGroup, s.FileType })
            .Select(g => new FileGroupAggregate
            {
                DatabaseName = g.Key.DatabaseName,
                FileGroup = string.IsNullOrEmpty(g.Key.FileGroup) ? "(Log)" : g.Key.FileGroup,
                FileType = g.Key.FileType,
                AllocatedMB = g.Sum(s => s.AllocatedSpaceMB),
                UsedMB = g.Sum(s => s.UsedSpaceMB),
                FreeMB = g.Sum(s => s.FreeSpaceMB),
                FileCount = g.Count()
            })
            .OrderByDescending(fg => fg.AllocatedMB)
            .ToList();

        return fileGroups;
    }

    public async Task<IEnumerable<DiskSpaceInfo>> GetDiskSpaceInfoAsync()
    {
        // Use cached stats if available, otherwise fetch
        var stats = _cachedStats ?? await GetDbSizeStatsAsync();
        
        // Extract drive letter from physical file path and aggregate disk info
        var diskInfo = stats
            .Select(s => new 
            { 
                DrivePath = ExtractDrivePath(s.PhysicalFileName),
                s.TotalDriveMB,
                s.FreeDriveMB,
                s.FreeDrivePercent
            })
            .Where(d => !string.IsNullOrEmpty(d.DrivePath))
            .GroupBy(d => d.DrivePath)
            .Select(g => new DiskSpaceInfo
            {
                DrivePath = g.Key,
                TotalDriveMB = g.First().TotalDriveMB,
                FreeDriveMB = g.First().FreeDriveMB,
                FreeDrivePercent = g.First().FreeDrivePercent
            })
            .OrderBy(d => d.DrivePath)
            .ToList();

        return diskInfo;
    }

    public async Task<IEnumerable<DbSizeAlertStats>> GetDbFreeSpaceAlertsAsync()
    {
        var result = await _httpClient.GetFromJsonAsync<IEnumerable<DbSizeAlertStats>>("api/DbSizeStats/dbFreeSpaceAlert");
        return result ?? Enumerable.Empty<DbSizeAlertStats>();
    }

    private static string ExtractDrivePath(string physicalFileName)
    {
        if (string.IsNullOrEmpty(physicalFileName))
            return string.Empty;

        // Windows drive letter (e.g., "C:\")
        if (physicalFileName.Length >= 3 && physicalFileName[1] == ':')
            return physicalFileName.Substring(0, 3);

        // Unix-style path (e.g., "/var/opt/mssql/data")
        if (physicalFileName.StartsWith("/"))
        {
            var parts = physicalFileName.Split('/', StringSplitOptions.RemoveEmptyEntries);
            return parts.Length > 0 ? $"/{parts[0]}" : "/";
        }

        return string.Empty;
    }
}
