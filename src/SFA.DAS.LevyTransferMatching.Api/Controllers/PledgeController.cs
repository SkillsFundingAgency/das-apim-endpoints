using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.LevyTransferMatching.Api.Models;
using SFA.DAS.LevyTransferMatching.Application.Commands.CreatePledge;
using SFA.DAS.LevyTransferMatching.Application.Queries.GetPledges;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace SFA.DAS.LevyTransferMatching.Api.Controllers
{
    [ApiController]
    public class PledgeController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PledgeController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Route("pledges")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetPledges()
        {
            var result = await _mediator.Send(new GetPledgesQuery());

            return new OkObjectResult(result.Select(x => (PledgeDto)x));
        }

        [HttpGet]
        [Route("pledges/{pledgeId}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetPledge(int pledgeId)
        {
            var result = await _mediator.Send(new GetPledgesQuery()
            {
                PledgeId = pledgeId,
            });

            var pledge = result.SingleOrDefault();

            if (pledge != null)
            {
                return Ok((PledgeDto)pledge);
            }
            else
            {
                return NotFound();
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