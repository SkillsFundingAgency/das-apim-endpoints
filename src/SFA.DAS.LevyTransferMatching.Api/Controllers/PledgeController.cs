using System;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.LevyTransferMatching.Api.Models;
using SFA.DAS.LevyTransferMatching.Application.Commands.CreatePledge;
using SFA.DAS.LevyTransferMatching.Application.Queries.GetAllPledges;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SFA.DAS.LevyTransferMatching.Application.Queries.GetCreate;

namespace SFA.DAS.LevyTransferMatching.Api.Controllers
{
    [ApiController]
    public class PledgeController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<PledgeController> _logger;

        public PledgeController(IMediator mediator, ILogger<PledgeController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet]
        [Route("pledges")]
        public async Task<IActionResult> GetPledges()
        {
            var result = await _mediator.Send(new GetPledgesQuery());

            return new OkObjectResult(result.Select(x => (PledgeDto)x));
        }

        [HttpGet]
        [Route("accounts/{accountId}/pledges/create")]
        public async Task<IActionResult> Create()
        {
            try
            {
                var queryResult = await _mediator.Send(new GetCreateQuery());

                var response = new GetCreateResponse
                {
                    Levels = queryResult.Levels,
                    Sectors = queryResult.Sectors,
                    JobRoles = queryResult.JobRoles
                };

                return Ok(response);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error attempting to get Create result");
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }

        [HttpPost]
        [Route("accounts/{accountId}/pledges")]
        public async Task<IActionResult> CreatePledge(long accountId, [FromBody]CreatePledgeRequest createPledgeRequest)
        {
            var commandResult = await _mediator.Send(new CreatePledgeCommand
            {
                AccountId = accountId,
                Amount = createPledgeRequest.Amount,
                IsNamePublic = createPledgeRequest.IsNamePublic,
                DasAccountName = createPledgeRequest.DasAccountName,
                JobRoles = createPledgeRequest.JobRoles,
                Levels = createPledgeRequest.Levels,
                Sectors = createPledgeRequest.Sectors,
            });

            return new CreatedResult(
                $"/accounts/{accountId}/pledges/{commandResult.PledgeId}",
                (PledgeIdDto)commandResult.PledgeId);
        }
    }
}