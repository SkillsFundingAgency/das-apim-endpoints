using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.ManageApprenticeships.Api.Models.Transfers;
using SFA.DAS.ManageApprenticeships.Application.Queries.Transfers.GetFinancialBreakdown;
using SFA.DAS.ManageApprenticeships.Application.Queries.Transfers.GetIndex;
using System;
using System.Threading.Tasks;

namespace SFA.DAS.ManageApprenticeships.Api.Controllers
{
    [ApiController]
    [Route("Transfers/")]
    public class TransfersController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<TransfersController> _logger;

        public TransfersController(IMediator mediator, ILogger<TransfersController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet]
        [Route("{accountId}")]
        public async Task<IActionResult> GetIndex(long accountId)
        {
            try
            {
                var response = await _mediator.Send(new GetIndexQuery()
                {
                    AccountId = accountId,
                });

                var model = new GetIndexResponse
                {
                    PledgesCount = response.PledgesCount,
                    ApplicationsCount = response.ApplicationsCount
                };

                return Ok(model);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error getting transfers");
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("{accountId}/transfers/financial-breakdown")]
        public async Task<IActionResult> GetFinancialBreakdown(long accountId)
        {
            try
            {
                var response = await _mediator.Send(new GetFinancialBreakdownQuery()
                {
                    AccountId = accountId,
                });

                var model = new GetFinancialBreakdownResponse
                {
                    TransferConnections = response.TransferConnections,
                    AcceptedPledgeApplications = response.AcceptedPledgeApplications,
                    ApprovedPledgeApplications = response.ApprovedPledgeApplications,                    
                    PledgeOriginatedCommitments =  response.PledgeOriginatedCommitments,
                    Commitments = response.Commitments,
                    FundsIn = response.FundsIn,
                    ProjectionStartDate = response.ProjectionStartDate,
                    NumberOfMonths = response.NumberOfMonths,
                };

                return Ok(model);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error getting financial breakdown");
                return BadRequest();
            }
        }
    }
}
