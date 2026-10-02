using Microsoft.AspNetCore.Mvc;

namespace SFA.DAS.EmployerFinanceJobs.Api.Controllers;

[ApiController]
public class PingController : ControllerBase
{
    [HttpGet("/ping")]
    public IActionResult Ping()
    {
        return Ok("Pong");
    }
}