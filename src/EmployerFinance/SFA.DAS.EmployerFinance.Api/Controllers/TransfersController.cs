using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.EmployerFinance.Api.Models.Transfers;
using SFA.DAS.EmployerFinance.Application.Queries.Transfers.GetCounts;
using SFA.DAS.EmployerFinance.Application.Queries.Transfers.GetFinancialBreakdown;

namespace SFA.DAS.EmployerFinance.Api.Controllers;

[ApiController]
[Route("[controller]/")]
public class TransfersController(IMediator mediator, ILogger<TransfersController> logger) : ControllerBase
{
    [HttpGet]
    [Route("{accountId}/counts")]
    public async Task<IActionResult> GetCounts(long accountId)
    {
        try
        {
            var response = await mediator.Send(new GetCountsQuery()
            {
                AccountId = accountId,
            });

            var model = new GetCountsResponse
            {
                PledgesCount = response.PledgesCount,
                ApplicationsCount = response.ApplicationsCount,
                CurrentYearEstimatedCommittedSpend = response.CurrentYearEstimatedCommittedSpend
            };

            return Ok(model);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error getting transfer counts");
            return BadRequest();
        }
    }

    [HttpGet]
    [Route("{accountId}/financial-breakdown")]
    public async Task<IActionResult> GetFinancialBreakdown(long accountId)
    {
        try
        {
            var response = await mediator.Send(new GetFinancialBreakdownQuery
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
                AmountPledged = response.AmountPledged
            };

            return Ok(model);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error getting financial breakdown");
            return BadRequest();
        }
    }
}