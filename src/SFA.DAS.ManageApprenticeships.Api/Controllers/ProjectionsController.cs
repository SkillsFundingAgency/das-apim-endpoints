using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.ManageApprenticeships.Api.Models;
using SFA.DAS.ManageApprenticeships.Application.Queries.GetAccountProjectionSummary;

namespace SFA.DAS.ManageApprenticeships.Api.Controllers
{
    [ApiController]
    [Route("[controller]/")]
    public class ProjectionsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<ProjectionsController> _logger;

        public ProjectionsController(IMediator mediator, ILogger<ProjectionsController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet]
        [Route("{accountId}")]
        public async Task<IActionResult> GetAccountProjectionSummary(long accountId)
        {
            try
            {
                var response = await _mediator.Send(new GetAccountProjectionSummaryQuery
                {
                    AccountId = accountId,
                });                

                return Ok((GetAccountProjectionSummaryResponse)response);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error getting projections");
                return BadRequest();
            }
        }
    }
}
