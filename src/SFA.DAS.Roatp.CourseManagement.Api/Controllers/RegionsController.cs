using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.Roatp.CourseManagement.Application.Regions.Queries;
using System.Threading.Tasks;

namespace SFA.DAS.Roatp.CourseManagement.Api.Controllers
{
    [ApiController]
    public class RegionsController : ControllerBase
    {
        private readonly ILogger<RegionsController> _logger;
        private readonly IMediator _mediator;

        public RegionsController(ILogger<RegionsController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpGet]
        [Route("regions")]
        public async Task<IActionResult> GetAllRegions()
        {
            _logger.LogInformation("Request received for all Regions");
            var query = new GetAllRegionsQuery();
            var result = await _mediator.Send(query);
            _logger.LogInformation($"Found {result.Regions.Count} Regions");
            return Ok(result.Regions);
        }
    }
}
