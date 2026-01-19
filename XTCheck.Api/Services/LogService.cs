using XTCheck.Api.Data;

namespace XTCheck.Api.Services;

public interface ILogService
{
    Task<string> CheckDatabaseConnectionAsync();
}

public class LogService : ILogService
{
    private readonly ILogRepository _logRepository;

    public LogService(ILogRepository logRepository)
    {
        _logRepository = logRepository;
    }

    public async Task<string> CheckDatabaseConnectionAsync()
    {
        return await _logRepository.GetDatabaseVersionAsync();
    }
}
