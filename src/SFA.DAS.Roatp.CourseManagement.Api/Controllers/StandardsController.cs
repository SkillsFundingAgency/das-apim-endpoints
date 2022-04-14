using Microsoft.AspNetCore.Mvc;

namespace SFA.DAS.Roatp.CourseManagement.Api.Controllers
{
    [ApiController]
    [Route("[controller]/")]
    public class StandardsController : ControllerBase
    {
        [HttpGet]
        public IActionResult Index()
        {
            return new OkObjectResult("Not implemented");
        }
    }
}
