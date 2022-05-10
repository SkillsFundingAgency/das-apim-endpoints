using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.Roatp.CourseManagement.Application.Locations.Queries;
using System.Threading.Tasks;

namespace SFA.DAS.Roatp.CourseManagement.Api.Controllers
{
    [ApiController]
    public class GetProviderLocationsController : ControllerBase
    {
        private readonly ILogger<GetProviderLocationsController> _logger;
        private readonly IMediator _mediator;

        public GetProviderLocationsController(ILogger<GetProviderLocationsController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpGet]
        [Route("providers/{ukprn}/locations")]
        public async Task<IActionResult> GetAllProviderLocations([FromRoute] int ukprn)
        {
            _logger.LogInformation("Request received for all locations for ukprn: {ukprn}", ukprn);
            var query = new GetAllProviderLocationsQuery(ukprn);
            var result = await _mediator.Send(query);
            if (result.ProviderLocations == null)
            {
                _logger.LogInformation("Invalid ukprn: {ukprn}", ukprn);
                return BadRequest();
            }
            _logger.LogInformation($"Found {result.ProviderLocations.Count} locations for ukprn: {ukprn}");
            return Ok(result.ProviderLocations);
        }
    }
}
