using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SFA.DAS.Aodp.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TestController : Controller
    {
        [AllowAnonymous]
        [HttpGet("")]
        public IActionResult Ping()
        {
            return Ok("Test");
        }
    }
}
