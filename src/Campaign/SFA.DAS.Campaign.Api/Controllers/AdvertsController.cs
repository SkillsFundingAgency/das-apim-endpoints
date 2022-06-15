using System;
using System.Net;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.Campaign.Api.Models;
using SFA.DAS.Campaign.Application.Queries.Adverts;

namespace SFA.DAS.Campaign.Api.Controllers
{
    [ApiController]
    [Route("[controller]/")]
    public class AdvertsController : ControllerBase
    {
        private readonly ILogger<AdvertsController> _logger;
        private readonly IMediator _mediator;

        public AdvertsController (ILogger<AdvertsController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetAdverts( string postCode, string route, uint distance)
        {
            try
            {
                var queryResult = await _mediator.Send(new GetAdvertsQuery
                {
                    Distance = distance,
                    Postcode = postCode,
                    Route = route
                });

                var model = (GetAdvertsResponse)queryResult;

                return Ok(model);
            }
            catch (Exception e)
            {
                _logger.LogError(e,"Error getting list of adverts");
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }
    }
}