using Microsoft.AspNetCore.Mvc;

namespace SFA.DAS.Campaign.Api.Controllers
{
    [ApiController]
    [Route("[controller]/")]
    public class SectorsController : ControllerBase
    {
        public SectorsController ()
        {
            
        }
        
        [HttpGet]
        [Route("")]
        public IActionResult GetSectors()
        {
            return Ok();
        }
    }
}