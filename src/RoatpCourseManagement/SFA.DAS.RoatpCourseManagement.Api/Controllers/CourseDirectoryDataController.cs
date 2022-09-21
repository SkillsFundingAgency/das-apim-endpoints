using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.RoatpCourseManagement.Services;
using System.Threading.Tasks;

namespace SFA.DAS.RoatpCourseManagement.Api.Controllers
{
    [ApiController]
    public class CourseDirectoryDataController : Controller
    {
        private readonly ICourseDirectoryService _courseDirectoryService;
        private readonly ILogger<CourseDirectoryDataController> _logger;

        public CourseDirectoryDataController(ICourseDirectoryService courseDirectoryService, ILogger<CourseDirectoryDataController> logger)
        {
            _courseDirectoryService = courseDirectoryService;
            _logger = logger;
        }

        [HttpGet]
        [Route("lookup/course-directory-data")]
        public async Task<IActionResult> GetProvidersData()
        {
            _logger.LogInformation("Request to retrieve course directory data received");
            var jsonData = await _courseDirectoryService.GetAllProvidersData();
            return new JsonResult(jsonData);
        }
    }
}
