using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.Recruit.Application.Queries.GetAddresses;
using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.Recruit.Application.Queries.GetGeoPoint;
using SFA.DAS.Recruit.Application.Queries.GetPostcodeData;

namespace SFA.DAS.Recruit.Api.Controllers
{
    [ApiController]
    [Route("[controller]/")]
    public class LocationsController(ILogger<LocationsController> logger, IMediator mediator) : ControllerBase
    {
        [HttpGet]
        [Route("")]
        public async Task<IActionResult> Index([FromQuery]string query)
        {
            try
            {
                var queryResponse = await mediator.Send(new GetAddressesQuery(query));

                if (queryResponse.AddressesResponse == null || !queryResponse.AddressesResponse.Addresses.Any())
                    return NotFound();

                return Ok(queryResponse.AddressesResponse);
            }
            catch (Exception e)
            {
                logger.LogError(e, "Error attempting to get list of addresses");
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet]
        [Route("geopoint")]
        public async Task<IActionResult> GetGeopoint([FromQuery] string postcode)
        {
            try
            {
                var queryResponse = await mediator.Send(new GetGeoPointQuery(postcode));

                if (queryResponse.GetPointResponse == null)
                    return NotFound();

                return Ok(queryResponse.GetPointResponse);
            }
            catch (Exception e)
            {
                logger.LogError(e, "Error attempting to get geopoint of postcode");
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }
    }
}
