using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.LevyTransferMatching.Api.Models;
using SFA.DAS.LevyTransferMatching.Application.Commands.CreatePledge;
using SFA.DAS.LevyTransferMatching.Application.Queries.GetAllPledges;
using SFA.DAS.LevyTransferMatching.Application.Queries.GetLocations;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SFA.DAS.LevyTransferMatching.Api.Controllers
{
    [ApiController]
    public class PledgeController
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

            return new OkObjectResult(result.Select(x => (PledgeDto)x));
        }
		
        [HttpPost]
        [Route("accounts/{accountId}/pledges")]
        public async Task<IActionResult> CreatePledge(long accountId, [FromBody]CreatePledgeRequest createPledgeRequest)
        {
            var locationInformation = new List<GetLocationInformationResult>(); 
            foreach(string location in createPledgeRequest.Locations)
            {
                var queryResult = await _mediator.Send(new GetLocationInformationQuery() { Location = location });
                locationInformation.Add(queryResult);
            }

            var commandResult = await _mediator.Send(new CreatePledgeCommand
            {
                AccountId = accountId,
                Amount = createPledgeRequest.Amount,
                IsNamePublic = createPledgeRequest.IsNamePublic,
                DasAccountName = createPledgeRequest.DasAccountName,
                JobRoles = createPledgeRequest.JobRoles,
                Levels = createPledgeRequest.Levels,
                Sectors = createPledgeRequest.Sectors,
                Locations = locationInformation
            });

            return new CreatedResult(
                $"/accounts/{accountId}/pledges/{commandResult.PledgeId}",
                (PledgeIdDto)commandResult.PledgeId);
        }
    }
}