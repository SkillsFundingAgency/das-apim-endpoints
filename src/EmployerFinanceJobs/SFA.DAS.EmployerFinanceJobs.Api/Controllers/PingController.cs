using System.Net;
using Microsoft.AspNetCore.Mvc;

namespace SFA.DAS.EmployerFinanceJobs.Api.Controllers;

[ApiController]
[Route("ping")]
public class PingController : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
    public IResult Ping()
    {
        return TypedResults.Ok("Pong");
    }
}