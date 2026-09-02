using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.EmployerFinance.Application.Queries.GetLevySummaryByHashedAccountId;
using System;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerFinance.Api.Controllers;

[Route("finance/levy")]
[ApiController]
public class FinanceLevyController(IMediator mediator, ILogger<FinanceLevyController> logger) : ControllerBase
{
    [HttpGet]
    [Route("{hashedAccountId}/summary")]
    public async Task<IActionResult> GetLevySummary(string hashedAccountId)
    {
        try
        {
            var result = await mediator.Send(new GetLevySummaryByHashedAccountIdQuery(hashedAccountId));

            return Ok(result);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error getting levy summary for account {HashedAccountId}", hashedAccountId);
            return BadRequest();
        }
    }
}