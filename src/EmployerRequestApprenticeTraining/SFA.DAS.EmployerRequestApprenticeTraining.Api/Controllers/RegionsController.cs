using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.EmployerRequestApprenticeTraining.Application.Queries.GetClosestRegion;
using SFA.DAS.EmployerRequestApprenticeTraining.Application.Queries.GetLocation;
using SFA.DAS.EmployerRequestApprenticeTraining.Application.Queries.GetRegions;
using System;
using System.Net;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerRequestApprenticeTraining.Api.Controllers
{
    [ApiController]
    [Route("regions/")]
    public class RegionsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<RegionsController> _logger;

        public RegionsController(IMediator mediator, ILogger<RegionsController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet("")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var result = await _mediator.Send(new GetRegionsQuery());

                if (result.Regions != null)
                {
                    return Ok(result.Regions);
                }

                return NotFound();
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error attempting to retrieve regions");
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet("closest")]
        public async Task<IActionResult> GetClosestRegion([FromQuery] string locationName)
        {
            try
            {
                var matchingLocationResult = await _mediator.Send(new GetLocationQuery { ExactSearchTerm = locationName });
                if(matchingLocationResult?.Location != null)
                {
                    var closestRegionResult = await _mediator.Send(new GetClosestRegionQuery 
                    { 
                        Latitude = matchingLocationResult.Location.Location.GeoPoint[0], 
                        Longitude = matchingLocationResult.Location.Location.GeoPoint[1] 
                    });

                    if(closestRegionResult?.Region != null)
                    {
                        return Ok(closestRegionResult.Region);
                    }
                }

                return NotFound();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error attempting to retrieve closest region for {LocationName}", locationName);
                return BadRequest();
            }
        }
    }
}
