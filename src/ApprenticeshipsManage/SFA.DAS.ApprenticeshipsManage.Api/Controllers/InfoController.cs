using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SFA.DAS.ApprenticeshipsManage.Api.Controllers;

[ApiController]
[AllowAnonymous]
public class InfoController(IConfiguration config) : ControllerBase
{
    [HttpGet("info")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult GetInfo()
    {
        var info = new
        {
            Version = config["ApiVersion"] ?? "1.0.0",
            Name = "ApprenticeshipsManage Outer API"
        };
        return Ok(info);
    }
}