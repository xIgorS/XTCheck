using XTCheck.Api.Data;
using XTCheck.Api.Models;

namespace XTCheck.Api.Services;

public interface IDbSizeStatsService
{
    Task<IEnumerable<DbSizeAlertStats>> GetDbSizePlusDiskAsync();
}

public class DbSizeStatsService : IDbSizeStatsService
{
    private readonly IDbSizeStatsRepository _dbSizeStatsRepository;

    public DbSizeStatsService(IDbSizeStatsRepository dbSizeStatsRepository)
    {
        _dbSizeStatsRepository = dbSizeStatsRepository;
    }

    public async Task<IEnumerable<DbSizeAlertStats>> GetDbSizePlusDiskAsync()
    {
        return await _dbSizeStatsRepository.GetDbSizePlusDiskAsync();
    }
}
