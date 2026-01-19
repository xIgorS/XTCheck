using Microsoft.AspNetCore.Mvc;
using XTCheck.Api.Services;

namespace XTCheck.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LogController : ControllerBase
{
    private readonly ILogService _logService;

    public LogController(ILogService logService)
    {
        _logService = logService;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        try
        {
            var version = await _logService.CheckDatabaseConnectionAsync();
            return Ok(new { DatabaseVersion = version, Message = "Connection Successful" });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Message = "Connection Failed", Error = ex.Message });
        }
    }
}
