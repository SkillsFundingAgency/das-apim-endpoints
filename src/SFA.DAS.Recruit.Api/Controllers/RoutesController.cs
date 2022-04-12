using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.Recruit.Api.Models;
using SFA.DAS.Recruit.Application.Queries.Routes;

namespace SFA.DAS.Recruit.Api.Controllers
{
    [ApiController]
    [Route("[controller]/")]
    public class RoutesController : ControllerBase
    {
        private readonly ILogger<RoutesController> _logger;
        private readonly IMediator _mediator;

        public RoutesController (ILogger<RoutesController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }
        
        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetRoutes()
        {
            try
            {
                var result = await _mediator.Send(new GetRoutesQuery());
                return Ok(new GetRoutesResponse
                {
                    Routes = result.Routes.Select(c=>(GetRouteResponseItem)c).ToList()
                });
            }
            catch (Exception e)
            {
                _logger.LogError(e,"Error getting list of routes");
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }
    }
}