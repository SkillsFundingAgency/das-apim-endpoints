using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.Roatp.CourseManagement.InnerApi.Locations.Queries;
using System.Threading.Tasks;

namespace SFA.DAS.Roatp.CourseManagement.Api.Controllers
{
    [ApiController]
    public class LocationsController : ControllerBase
    {
        private readonly ILogger<LocationsController> _logger;
        private readonly IMediator _mediator;

        public LocationsController(ILogger<LocationsController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpGet]
        [Route("providers/{ukprn}/locations")]
        public async Task<IActionResult> GetAllLocations([FromRoute] int ukprn)
        {
            _logger.LogInformation("Request received for all locations for ukprn: {ukprn}", ukprn);
            var query = new GetAllProviderLocationsQuery(ukprn);
            var result = await _mediator.Send(query);
            return Ok(result.ProviderLocations);
        }
    }
}
