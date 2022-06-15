using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.Approvals.Application.Apprentices.Queries;

namespace SFA.DAS.Approvals.Api.Controllers
{
    [ApiController]
    [Route("[controller]/")]
    public class ApprenticesController: ControllerBase
    {
        private readonly ILogger<ApprenticesController> _logger;
        private readonly IMediator _mediator;

        public ApprenticesController(ILogger<ApprenticesController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            try
            {
                var result = await _mediator.Send(new GetApprenticeQuery{ ApprenticeId = id});
                if (result == null)
                {
                    return NotFound();
                }                
                return Ok(result);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error getting Apprentice data {id}", id);
                return BadRequest();
            }
        }
    }
}