using Microsoft.AspNetCore.Mvc;

namespace SFA.DAS.Campaign.Api.Controllers
{
    [ApiController]
    [Route("[controller]/")]
    public class TrainingCoursesController : ControllerBase
    {
        public TrainingCoursesController ()
        {
            
        }
        
        [HttpGet]
        [Route("")]
        public IActionResult GetStandardsBySector([FromQuery]string sector)
        {
            return Ok();
        }
    }
}