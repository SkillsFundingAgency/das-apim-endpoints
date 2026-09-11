using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.EmployerFinance.Application.Queries.GetLevySummaryByAccountId;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerFinance.Api.Controllers;

[Route("finance/levy")]
[ApiController]
public class FinanceLevyController(IMediator mediator, ILogger<FinanceLevyController> logger) : ControllerBase
{
    [HttpGet]
    [Route("{accountId:long}/summary")]
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
}