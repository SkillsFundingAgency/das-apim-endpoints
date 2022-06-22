using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.Approvals.Api.Models;
using SFA.DAS.Approvals.Application.LevyTransferMatching.Queries;

namespace SFA.DAS.Approvals.Api.Controllers
{
    [ApiController]
    [Route("[controller]/")]
    public class PledgeApplicationsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<PledgeApplicationsController> _logger;

        public PledgeApplicationsController(IMediator mediator, ILogger<PledgeApplicationsController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var result = await _mediator.Send(new GetPledgeApplicationQuery { PledgeApplicationId = id });
                if (result == null)
                {
                    return NotFound();
                }
                return Ok((GetPledgeApplicationResponse)result);
            }
            catch (Exception e)
            {
                _logger.LogError($"Error getting Pledge Application data {id}", e);
                return BadRequest();
            }
        }
    }
}
