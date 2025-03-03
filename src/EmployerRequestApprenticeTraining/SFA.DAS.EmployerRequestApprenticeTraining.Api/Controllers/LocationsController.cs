using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.EmployerRequestApprenticeTraining.Application.Queries.GetLocation;
using SFA.DAS.EmployerRequestApprenticeTraining.Application.Queries.GetLocations;
using SFA.DAS.EmployerRequestApprenticeTraining.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerRequestApprenticeTraining.Api.Controllers
{
    [ApiController]
    [Route("locations/")]
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
        public async Task<IActionResult> Get([FromQuery]string searchTerm, [FromQuery] bool exactMatch)
        {
            try
            {
                if (exactMatch)
                {
                    var result = await _mediator.Send(new GetLocationQuery { ExactSearchTerm = searchTerm });
                    if(result.Location != null)
                    {
                        return Ok(new List<LocationSearchResponse> { (LocationSearchResponse)result.Location });
                    }
                }
                else
                {
                    var results = await _mediator.Send(new GetLocationsQuery { SearchTerm = searchTerm });
                    if (results.Locations != null)
                    {
                        return Ok(results.Locations.Select(s => (LocationSearchResponse)s).ToList());
                    }
                }

                return Ok(new List<LocationSearchResponse>());
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error attempting to retrieve list of locations for {SearchTerm} and {ExactMatch}", searchTerm, exactMatch);
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }
    }
}