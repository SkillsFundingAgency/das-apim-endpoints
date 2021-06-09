using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.LevyTransferMatching.Api.Models;
using SFA.DAS.LevyTransferMatching.Application.Commands.CreatePledge;
using SFA.DAS.LevyTransferMatching.Models;
using System.Threading.Tasks;

namespace SFA.DAS.LevyTransferMatching.Api.Controllers
{
    [ApiController]
    [Route("accounts/{encodedAccountId}/pledges")]
    public class PledgeController
    {
        private readonly IMediator _mediator;

        public PledgeController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> CreatePledge(string encodedAccountId, [FromBody]CreatePledgeRequest createPledgeRequest)
        {
            var commandResult = await _mediator.Send(new CreatePledgeCommand()
            {
                Amount = createPledgeRequest.Amount,
                EncodedAccountId = encodedAccountId,
                IsNamePublic = createPledgeRequest.IsNamePublic,
                JobRoles = createPledgeRequest.JobRoles,
                Levels = createPledgeRequest.Levels,
                Sectors = createPledgeRequest.Sectors,
            });

            return new CreatedResult(
                $"/accounts/{encodedAccountId}/pledges/{commandResult.PledgeReference.Id}",
                (PledgeReferenceDto)commandResult.PledgeReference);
        }
    }
}