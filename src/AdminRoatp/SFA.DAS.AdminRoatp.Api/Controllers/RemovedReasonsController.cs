using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.AdminRoatp.Application.Queries.GetRemovedReasons;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Roatp;
using System.Net;

namespace SFA.DAS.AdminRoatp.Api.Controllers;
[Route("removed-reasons")]
[ApiController]
public class RemovedReasonsController(IMediator _mediator, ILogger<RemovedReasonsController> _logger) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(GetRemovedReasonsResponse))]
    public async Task<IActionResult> GetAllRemovedReasons(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Received request to get all removed reasons.");
        GetRemovedReasonsResponse response = await _mediator.Send(new GetRemovedReasonsQuery(), cancellationToken);
        return Ok(response);
    }
}