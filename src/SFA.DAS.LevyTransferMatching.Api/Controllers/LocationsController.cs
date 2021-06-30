using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.LevyTransferMatching.Api.Models;
using SFA.DAS.LevyTransferMatching.Application.Queries.GetLocations;
using System.Linq;
using System.Threading.Tasks;

namespace SFA.DAS.LevyTransferMatching.Api.Controllers
{
    [ApiController]
    [Route("locations")]
    public class LocationsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public LocationsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> Search([FromQuery] string searchTerm)
        {
            var queryResult = await _mediator.Send(new GetLocationsQuery { SearchTerm = searchTerm });

            var response = new LocationsDto
            {
                Names = queryResult.Locations?.Select(x => x.DisplayName)
            };

            return Ok(response);
        }

        [HttpGet]
        [Route("information")]
        public async Task<IActionResult> Information([FromQuery] string location)
        {
            var queryResult = await _mediator.Send(new GetLocationInformationQuery() { Location = location });

            var response = new LocationInformationDto
            {
                Name = queryResult.Name,
                GeoPoint = queryResult.GeoPoint
            };

            return Ok(response);
        }
    }
}
