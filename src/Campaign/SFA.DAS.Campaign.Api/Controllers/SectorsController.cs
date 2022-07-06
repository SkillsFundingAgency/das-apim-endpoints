using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.Campaign.Api.Models;
using SFA.DAS.Campaign.Application.Queries.Sectors;

namespace SFA.DAS.Campaign.Api.Controllers
{
    [ApiController]
    [Route("[controller]/")]
    public class SectorsController : ControllerBase
    {
        private readonly ILogger<SectorsController> _logger;
        private readonly IMediator _mediator;

        public SectorsController (ILogger<SectorsController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }
        
        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetSectors()
        {
            try
            {
                var result = await _mediator.Send(new GetSectorsQuery());
                return Ok(new GetSectorsResponse
                {
                    Sectors = result.Sectors.Select(c=>(GetRouteResponseItem)c).ToList()
                });
            }
            catch (Exception e)
            {
                _logger.LogError(e,"Error getting list of sectors");
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }
    }
}