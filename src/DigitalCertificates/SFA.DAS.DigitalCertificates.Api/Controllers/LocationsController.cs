using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.DigitalCertificates.Application.Queries.GetLocations;
using System;
using System.Net;
using System.Threading.Tasks;

namespace SFA.DAS.DigitalCertificates.Api.Controllers
{
    [ApiController]
    [Route("locations/")]
    public class LocationsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<LocationsController> _logger;

        public LocationsController(IMediator mediator, ILogger<LocationsController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetLocations([FromQuery] string query)
        {
            try
            {
                var result = await _mediator.Send(new GetLocationsQuery { Query = query });

                if (result?.Addresses == null)
                {
                    return Ok();
                }

                return Ok(result.Addresses);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error attempting to lookup locations for query {Query}", query);
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }
    }
}
