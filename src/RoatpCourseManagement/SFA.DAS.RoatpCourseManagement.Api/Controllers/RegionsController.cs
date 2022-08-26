using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.Roatp.CourseManagement.Application.Regions.Queries;
using System.Threading.Tasks;

namespace SFA.DAS.RoatpCourseManagement.Api.Controllers
{
    [ApiController]
    public class RegionsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<RegionsController> _logger;

        public RegionsController(ILogger<RegionsController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpGet]
        [Route("lookup/regions")]
        public async Task<IActionResult> GetAllRegions()
        {
            _logger.LogInformation($"Outer API: Getting all the regions and sub-regions");

            var result = await _mediator.Send(new GetAllRegionsQuery());

            return Ok(result);
        }
    }
}
