using Microsoft.AspNetCore.Mvc;

namespace SFA.DAS.EmployerAccounts.Api.Controllers
{
    [ApiController]
    public class PingController : ControllerBase
    {
        // This method can not be called ping because you can not hit method called ping for some unknown reasons
        [HttpGet]
        [Route("pingouterapi")]        
        public IActionResult Get()
        {            
            return Ok();
        }       
    }
}
