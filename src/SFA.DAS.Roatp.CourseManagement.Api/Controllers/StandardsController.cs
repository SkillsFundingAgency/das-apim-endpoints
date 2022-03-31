using Microsoft.AspNetCore.Mvc;

namespace SFA.DAS.Roatp.CourseManagement.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StandardsController : ControllerBase
    {
        public IActionResult Index()
        {
            return new OkObjectResult("Not implemented");
        }
    }
}
