using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SFA.DAS.Aodp.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PingController : Controller
    {
        [AllowAnonymous]
        [HttpGet("/Ping")]
        public IActionResult Ping()
        {
            return Ok("Pong");
        }
    }
}
