using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.EmployerFinance.Api.Models.Transfers;
using SFA.DAS.EmployerFinance.Application.Queries.Transfers.GetFinancialBreakdown;
using SFA.DAS.EmployerFinance.Application.Queries.Transfers.GetCounts;
using System;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerFinance.Api.Controllers
{
    [ApiController]
    [Route("[controller]/")]
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
        [Route("{accountId}/counts")]
        public async Task<IActionResult> GetCounts(long accountId)
        {
            try
            {
                var response = await _mediator.Send(new GetCountsQuery()
                {
                    AccountId = accountId,
                });

                var model = new GetCountsResponse
                {
                    PledgesCount = response.PledgesCount,
                    ApplicationsCount = response.ApplicationsCount
                };

                return Ok(model);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error getting transfer counts");
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("{accountId}/financial-breakdown")]
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
                    ProjectionStartDate = response.ProjectionStartDate,                    
                    CurrentYearEstimatedCommittedSpend = response.CurrentYearEstimatedCommittedSpend,
                    NextYearEstimatedCommittedSpend = response.NextYearEstimatedCommittedSpend,                    
                    YearAfterNextYearEstimatedCommittedSpend  = response.YearAfterNextYearEstimatedCommittedSpend,
                    AmountPledged = response.AmountPledged
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
