using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.LevyTransferMatching.Api.Models;
using SFA.DAS.LevyTransferMatching.Application.Commands.CreatePledge;
using SFA.DAS.LevyTransferMatching.Application.Queries.GetAllPledges;
using System.Linq;
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
        public async Task<IActionResult> GetPledges()
        {
            var result = await _mediator.Send(new GetPledgesQuery());

            return Ok(result.Select(x => (PledgeDto)x));
        }

        [HttpGet]
        [Route("pledges/{encodedId}")]
        public async Task<IActionResult> GetPledge(string encodedId)
        {
            var result = await _mediator.Send(new GetPledgesQuery()
            {
                EncodedId = encodedId,
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
        [Route("accounts/{encodedAccountId}/pledges")]
        public async Task<IActionResult> CreatePledge(string encodedAccountId, [FromBody]CreatePledgeRequest createPledgeRequest)
        {
            var commandResult = await _mediator.Send(new CreatePledgeCommand()
            {
                Amount = createPledgeRequest.Amount,
                EncodedAccountId = encodedAccountId,
                IsNamePublic = createPledgeRequest.IsNamePublic,
                DasAccountName = createPledgeRequest.DasAccountName,
                JobRoles = createPledgeRequest.JobRoles,
                Levels = createPledgeRequest.Levels,
                Sectors = createPledgeRequest.Sectors,
            });

            return new CreatedResult(
                $"/accounts/{encodedAccountId}/pledges/{commandResult.EncodedPledgeId}",
                (PledgeReferenceDto)commandResult);
        }
    }
}