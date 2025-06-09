using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.EmployerFinance.Api.Models;
using SFA.DAS.EmployerFinance.Application.Queries.GetAccountProjectionSummary;

namespace SFA.DAS.EmployerFinance.Api.Controllers;

[ApiController]
[Route("[controller]/")]
public class ProjectionsController(IMediator mediator, ILogger<ProjectionsController> logger) : ControllerBase
{
    [HttpGet]
    [Route("{accountId}")]
    public async Task<IActionResult> GetAccountProjectionSummary(long accountId)
    {
        try
        {
            var response = await mediator.Send(new GetAccountProjectionSummaryQuery
            {
                AccountId = accountId,
            });

            return Ok((GetAccountProjectionSummaryResponse)response);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error getting projections");
            return BadRequest();
        }
    }
}