using XTCheck.Web.Models;

namespace XTCheck.Web.Services;

public interface IDbSizeStatsApiClient
{
    Task<IEnumerable<DbSizeAlertStats>> GetDbFreeSpaceAlertsAsync();
}
