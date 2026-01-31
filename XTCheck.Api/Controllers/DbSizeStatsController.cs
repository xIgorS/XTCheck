using Microsoft.AspNetCore.Mvc;
using XTCheck.Api.Services;

namespace XTCheck.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DbSizeStatsController : ControllerBase
{
    private readonly IDbSizeStatsService _dbSizeStatsService;

    public DbSizeStatsController(IDbSizeStatsService dbSizeStatsService)
    {
        _dbSizeStatsService = dbSizeStatsService;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        try
        {
            var stats = await _dbSizeStatsService.GetDbSizeStatsAsync();
            return Ok(stats);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Message = "Failed to retrieve database size stats", Error = ex.Message });
        }
    }

    [HttpGet("dbFreeSpaceAlert")]
    public async Task<IActionResult> GetDbFreeSpaceAlert()
    {
        try
        {
            var alerts = await _dbSizeStatsService.GetDbSizePlusDiskAsync();
            return Ok(alerts);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Message = "Failed to retrieve database free space alerts", Error = ex.Message });
        }
    }
}
