using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.EmployerFinance.Application.Queries.GetLevySummaryByAccountId;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using SFA.DAS.EmployerFinance.Application.Queries.GetLevyProjectionsByAccountId;

namespace SFA.DAS.EmployerFinance.Api.Controllers;

[Route("finance/levy/{accountId:long}")]
[ApiController]
public class FinanceLevyController(IMediator mediator, ILogger<FinanceLevyController> logger) : ControllerBase
{
    [HttpGet]
    [Route("summary")]
    public async Task<IActionResult> GetLevySummary([FromRoute, Required] long accountId)
    {
        try
        {
            var result = await mediator.Send(new GetLevySummaryByAccountIdQuery(accountId));

            return Ok(result);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error getting levy summary for account {AccountId}", accountId);
            return BadRequest();
        }
    }

    [HttpGet]
    [Route("projections")]
    public async Task<IActionResult> GetLevyProjections([FromRoute, Required] long accountId, [FromQuery] int months = 12)
    {
        try
        {
            var result = await mediator.Send(new GetLevyProjectionsByAccountIdQuery(accountId, months));

            return Ok(result);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error getting levy projections for account {AccountId}", accountId);
            return BadRequest();
        }
    }
}