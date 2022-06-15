using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.Roatp.CourseManagement.Services;
using System.Threading.Tasks;

namespace SFA.DAS.Roatp.CourseManagement.Api.Controllers
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
