using Microsoft.AspNetCore.Mvc;
using SFA.DAS.LearnerData.Requests;

namespace SFA.DAS.LearnerData.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ShortCoursesController : ControllerBase
    {
        [HttpPost]
        [Route("/providers/{ukprn}/shortCourses")]
        public IActionResult CreateShortCourse(ShortCourseRequest request)
        {
            //echo the input
            return Ok(request);
        }
    }
}
