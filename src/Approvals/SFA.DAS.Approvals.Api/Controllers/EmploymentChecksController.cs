using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.Approvals.Application.EmploymentChecks.Queries.GetEmploymentChecksQuery;

namespace SFA.DAS.Approvals.Api.Controllers;

[ApiController]
[Route("[controller]/")]
public class EmploymentChecksController(IMediator mediator, ILogger<EmploymentChecksController> logger) : ControllerBase
{
    private const int MaxApprenticeshipIds = 1000;

    [HttpGet]
    [Route("")]
    public async Task<IActionResult> Get([FromQuery] List<long> apprenticeshipIds)
    {
        if (apprenticeshipIds == null || apprenticeshipIds.Count == 0)
        {
            return BadRequest("apprenticeshipIds is required and must not be empty.");
        }

        if (apprenticeshipIds.Count > MaxApprenticeshipIds)
        {
            return BadRequest($"apprenticeshipIds must not exceed {MaxApprenticeshipIds}.");
        }

        try
        {
            var result = await mediator.Send(new GetEmploymentChecksQuery
            {
                ApprenticeshipIds = apprenticeshipIds.AsReadOnly()
            });
            
            return Ok(result.Checks);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error getting employment checks");
            return BadRequest();
        }
    }
}
