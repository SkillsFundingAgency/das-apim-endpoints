using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SFA.DAS.EmployerPR.Api.Controllers;

[Route("ping")]
[ApiController]
[ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
public class PingController : ControllerBase
{
    [HttpGet, AllowAnonymous]
    public IActionResult Get()
    {
        return Ok("Pong");
    }
}

