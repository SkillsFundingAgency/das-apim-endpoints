using System;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.LevyTransferMatching.Api.Models;
using SFA.DAS.LevyTransferMatching.Application.Queries.GetOpportunity;
using SFA.DAS.LevyTransferMatching.Application.Queries.Standards;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using SFA.DAS.LevyTransferMatching.Api.Models.Opportunities;
using SFA.DAS.LevyTransferMatching.Application.Commands.CreateApplication;
using SFA.DAS.LevyTransferMatching.Application.Queries.Opportunities.GetConfirmation;

namespace SFA.DAS.LevyTransferMatching.Api.Controllers
{
    [ApiController]
    public class OpportunityController : ControllerBase
    {
        private readonly ILogger<OpportunityController> _logger;
        private readonly IMediator _mediator;

        public OpportunityController(ILogger<OpportunityController> logger, IMediator mediator)
        {
            _logger = logger;
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

            return NotFound();
        }

        [HttpPost]
        [Route("/accounts/{accountId}/opportunities/{opportunityId}/apply")]
        public async Task<IActionResult> Apply(long accountId, int opportunityId, [FromBody] ApplyRequest request)
        {
            var result = await _mediator.Send(new CreateApplicationCommand
            {
                EmployerAccountId = accountId,
                PledgeId = opportunityId,
                EncodedAccountId = request.EncodedAccountId,
                Details = request.Details,
                StandardId = request.StandardId,
                NumberOfApprentices = request.NumberOfApprentices,
                StartDate = request.StartDate,
                HasTrainingProvider = request.HasTrainingProvider,
                Sectors = request.Sectors,
                Postcode = request.Postcode,
                FirstName = request.FirstName,
                LastName = request.LastName,
                EmailAddresses = request.EmailAddresses,
                BusinessWebsite = request.BusinessWebsite
            });

            return Created($"/accounts/{accountId}/opportunities/{opportunityId}/apply", (ApplyResponse)result);
        }

        [HttpGet]
        [Route("/accounts/{accountId}/opportunities/{opportunityId}/apply/confirmation")]
        public async Task<IActionResult> Confirmation(long accountId, int opportunityId)
        {
            var result = await _mediator.Send(new GetConfirmationQuery
            {
                OpportunityId = opportunityId
            });

            if (result != null)
            {
                return Ok((GetConfirmationResponse)result);
            }

            return NotFound();
        }

        [HttpGet]
        [Route("opportunities/{opportunityId}/create/application-details")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetApplicationDetails(int opportunityId)
        {
            try
            {
                var opportunity = await _mediator.Send(new GetOpportunityQuery() { OpportunityId = opportunityId });

                if(opportunity == null)
                {
                    return NotFound();
                }

                var standards = await _mediator.Send(new GetStandardsQuery());

                return Ok(new ApplicationDetailsResponse
                {
                    Standards = standards.Standards,
                    Opportunity = opportunity
                });
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error getting Application Details");
                return BadRequest();
            }
        }
    }
}