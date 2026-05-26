using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.Approvals.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.ChangeHistory;

namespace SFA.DAS.Approvals.Api.Controllers;

[ApiController]
[Route("[controller]/")]
public class ChangeHistoryController(IMediator mediator, ILogger<ChangeHistoryController> logger) : Controller
{

    [Route("{ApprenticeshipId:long}")]
    public async Task<IActionResult> GetChangeHistory(long apprenticeshipId)
    {
        try
        {
            var queryResult = await mediator.Send(new GetChangeHistoryRequest(apprenticeshipId));

            if (queryResult == null)
            {
                return NotFound();
            }

            var model = (GetChangeHistoryResponse)queryResult;
            return Ok(model);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error attempting to get change history");
            return BadRequest();
        }
    }
}
