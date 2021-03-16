using System;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.EmployerDemand.Api.Models;
using SFA.DAS.EmployerDemand.Application.Locations.Queries.GetLocations;

namespace SFA.DAS.EmployerDemand.Api.Controllers
{
    [ApiController]
    [Route("[controller]/")]
    public class LocationsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<LocationsController> _logger;

        public LocationsController (IMediator mediator, ILogger<LocationsController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetByQuery([FromQuery]string searchTerm)
        {
            try
            {
                var queryResult = await _mediator.Send(new GetLocationsQuery {SearchTerm = searchTerm});

                var response = new GetLocationSearchResponse
                {
                    Locations = queryResult.Locations
                        .Select(c => (GetLocationSearchResponseItem)c),
                };
                return Ok(response);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error attempting to get list of locations, search term:{searchTerm}");
                return BadRequest();
            }
        }
    }
}