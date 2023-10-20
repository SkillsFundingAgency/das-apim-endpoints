using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.FindAnApprenticeship.Api.Models;
using SFA.DAS.FindAnApprenticeship.Application.Queries.GetAddresses;
using SFA.DAS.FindAnApprenticeship.Application.Queries.GetGeoPoint;
using SFA.DAS.FindAnApprenticeship.Application.Queries.GetLocationsBySearch;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace SFA.DAS.FindAnApprenticeship.Api.Controllers
{
    [ApiController]
    [Route("[controller]/")]
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
        [Route("")]
        public async Task<IActionResult> Index([FromQuery] string query)
        {
            try
            {
                var queryResponse = await _mediator.Send(new GetAddressesQuery(query));

                if (queryResponse.AddressesResponse == null || !queryResponse.AddressesResponse.Addresses.Any())
                    return NotFound();

                return Ok(queryResponse.AddressesResponse);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error attempting to get list of addresses");
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet]
        [Route("geopoint")]
        public async Task<IActionResult> GeoPoint([FromQuery] string postcode)
        {
            try
            {
                var queryResponse = await _mediator.Send(new GetGeoPointQuery(postcode));

                if (queryResponse.GetPointResponse == null)
                    return NotFound();

                return Ok(queryResponse.GetPointResponse);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error attempting to get geopoint of postcode");
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet]
        [Route("searchbylocation")]
        public async Task<IActionResult> SearchByLocation([FromQuery] string searchTerm)
        {
            try
            {
                var queryResult = await _mediator.Send(new GetLocationsBySearchQuery { SearchTerm = searchTerm });

                var response = new GetLocationBySearchResponse
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