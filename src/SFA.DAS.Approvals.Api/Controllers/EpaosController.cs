using System;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.Approvals.Api.Models;
using SFA.DAS.Approvals.Application.Epaos.Queries;

namespace SFA.DAS.Approvals.Api.Controllers
{
    [ApiController]
    [Route("[controller]/")]
    public class EpaosController: ControllerBase
    {
        private readonly ILogger<EpaosController> _logger;
        private readonly IMediator _mediator;

        public EpaosController (ILogger<EpaosController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var result = await _mediator.Send(new GetEpaosQuery());

                var model = new GetEpaosListResponse
                {
                    Epaos = result.Epaos.Select(c=>(GetEpaoResponse)c).ToList()
                };
                
                return Ok(model);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error getting EPAO data");
                return BadRequest();
            }
        }
    }
}