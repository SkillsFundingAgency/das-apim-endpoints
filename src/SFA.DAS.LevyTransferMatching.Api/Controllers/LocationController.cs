using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.LevyTransferMatching.Api.Models;
using SFA.DAS.LevyTransferMatching.Application.Queries.GetLocationInformation;
using SFA.DAS.LevyTransferMatching.Application.Queries.GetLocations;
using System.Linq;
using System.Threading.Tasks;

namespace SFA.DAS.LevyTransferMatching.Api.Controllers
{
    [ApiController]
    [Route("locations")]
    public class LocationController : ControllerBase
    {
        private readonly IMediator _mediator;

        public LocationController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetLocations([FromQuery] string searchTerm)
        {
            var queryResult = await _mediator.Send(new GetLocationsQuery { SearchTerm = searchTerm });

            var response = new LocationsDto
            {
                Names = queryResult.Locations?.Select(x => x.DisplayName)
            };

            return new OkObjectResult(response);
        }

        [HttpGet]
        [Route("information")]
        public async Task<IActionResult> GetLocationInformation([FromQuery] string location)
        {
            var queryResult = await _mediator.Send(new GetLocationInformationQuery() { Location = location });

            var response = new LocationInformationDto
            {
                Name = queryResult.Name,
                GeoPoint = queryResult.GeoPoint
            };

            return new OkObjectResult(response);
        }
    }
}
