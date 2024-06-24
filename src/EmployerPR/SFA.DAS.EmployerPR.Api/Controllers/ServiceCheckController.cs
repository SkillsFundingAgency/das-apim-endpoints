using Microsoft.AspNetCore.Mvc;

namespace SFA.DAS.EmployerPR.Api.Controllers;

[Route("serviceCheck")]
[ApiController]
[ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
public class ServiceCheckController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        return Ok();
    }
}
