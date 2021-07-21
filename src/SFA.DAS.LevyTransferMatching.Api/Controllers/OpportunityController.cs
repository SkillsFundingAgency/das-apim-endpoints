using System;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.LevyTransferMatching.Api.Models;
using SFA.DAS.LevyTransferMatching.Application.Queries.GetOpportunity;
using System.Net;
using System.Threading.Tasks;
using SFA.DAS.LevyTransferMatching.Api.Models.Opportunities;
using SFA.DAS.LevyTransferMatching.Application.Commands.CreateApplication;

namespace SFA.DAS.LevyTransferMatching.Api.Controllers
{
    [ApiController]
    public class OpportunityController : ControllerBase
    {
        private readonly IMediator _mediator;

        public OpportunityController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Route("opportunities/{opportunityId}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetOpportunity(int opportunityId)
        {
            var result = await _mediator.Send(new GetOpportunityQuery()
            {
                OpportunityId = opportunityId,
            });

            if (result != null)
            {
                return Ok((PledgeDto)result);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost]
        [Route("/accounts/{accountId}/opportunities/{opportunityId}/apply")]
        public async Task<IActionResult> Apply(long accountId, int opportunityId, [FromBody] ApplyRequest request)
        {
            var result = await _mediator.Send(new CreateApplicationCommand
            {
                EmployerAccountId = accountId,
                PledgeId = opportunityId,
                EncodedAccountId = request.EncodedAccountId
            });

            return Created($"/accounts/{accountId}/opportunities/{opportunityId}/apply", (ApplyResponse)result);
        }
    }
}