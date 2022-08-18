using Microsoft.AspNetCore.Mvc;

namespace SFA.DAS.EmployerAccounts.Api.Controllers
{
    [ApiController]
    public class PingController : ControllerBase
    {
        [HttpGet]
        [Route("pingouterapi")]        
        public IActionResult Get()
        {            
            return Ok();
        }       
    }
}
