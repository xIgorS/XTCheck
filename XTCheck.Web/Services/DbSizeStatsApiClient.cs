using System.Net.Http.Json;
using XTCheck.Web.Models;

namespace XTCheck.Web.Services;

public class DbSizeStatsApiClient : IDbSizeStatsApiClient
{
    private readonly HttpClient _httpClient;
    public DbSizeStatsApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IEnumerable<DbSizeAlertStats>> GetDbFreeSpaceAlertsAsync()
    {
        var result = await _httpClient.GetFromJsonAsync<IEnumerable<DbSizeAlertStats>>("api/DbSizeStats/dbFreeSpaceAlert");
        return result ?? Enumerable.Empty<DbSizeAlertStats>();
    }
}
