using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.Approvals.Application.ChangeHistory.Queries;
using SFA.DAS.Approvals.Application.ChangeHistory.Queries.GetAll;

namespace SFA.DAS.Approvals.Api.Controllers;

[ApiController]
[Route("change-history/")]
public class ChangeHistoryController(IMediator mediator, ILogger<ChangeHistoryController> logger) : ControllerBase
{
    [HttpGet]
    [Route("{apprenticeshipId:long}")]
    public async Task<IActionResult> GetChangeHistory(long apprenticeshipId)
    {
        try
        {
            var queryResult = await mediator.Send(new GetChangeHistoryQuery(apprenticeshipId));

            var model = queryResult;
            return Ok(model);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error attempting to get change history for apprenticeshipId: {ApprenticeshipId}", apprenticeshipId);
            return BadRequest();
        }
    }

    [HttpGet]
    [Route("{providerId:long}/get-all-change-history")]
    public async Task<IActionResult> GetChangeHistoryForAllLearnersOfProvider(long providerId)
    {
        try
        {
            var queryResult = await mediator.Send(new GetAllChangeHistoryForProviderQuery(providerId));

            var model = queryResult;
            return Ok(model);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error attempting to get change history for ProviderId: {ProviderId}", providerId);
            return BadRequest();
        }
    }
}