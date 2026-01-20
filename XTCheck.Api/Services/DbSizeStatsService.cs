using XTCheck.Api.Data;
using XTCheck.Api.Models;

namespace XTCheck.Api.Services;

public interface IDbSizeStatsService
{
    Task<IEnumerable<DbSizeStats>> GetDbSizeStatsAsync();
}

public class DbSizeStatsService : IDbSizeStatsService
{
    private readonly IDbSizeStatsRepository _dbSizeStatsRepository;

    public DbSizeStatsService(IDbSizeStatsRepository dbSizeStatsRepository)
    {
        _dbSizeStatsRepository = dbSizeStatsRepository;
    }

    public async Task<IEnumerable<DbSizeStats>> GetDbSizeStatsAsync()
    {
        return await _dbSizeStatsRepository.GetDbSizeStatsAsync();
    }
}
