using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.Approvals.Application.ChangeHistory.Queries;

namespace SFA.DAS.Approvals.Api.Controllers;

[ApiController]
[Route("[controller]/")]
public class ChangeHistoryController(IMediator mediator, ILogger<ChangeHistoryController> logger) : ControllerBase
{
    [HttpGet]
    [Route("{ApprenticeshipId:long}")]
    public async Task<IActionResult> GetChangeHistory(long apprenticeshipId)
    {
        try
        {
            var queryResult = await mediator.Send(new GetChangeHistoryQuery(apprenticeshipId));

            if (queryResult == null)
            {
                return NotFound();
            }

            var model = queryResult;
            return Ok(model);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error attempting to get change history for apprenticeshipId: {ApprenticeshipId}", apprenticeshipId);
            return BadRequest();
        }
    }
}