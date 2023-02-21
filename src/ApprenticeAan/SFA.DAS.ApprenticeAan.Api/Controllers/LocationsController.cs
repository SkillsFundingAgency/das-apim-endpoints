using System.Net;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApprenticeAan.Application.Locations.Queries.GetAddresses;

namespace SFA.DAS.ApprenticeAan.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
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
        public async Task<IActionResult> GetAddresses([FromQuery] string query)
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
    }
}