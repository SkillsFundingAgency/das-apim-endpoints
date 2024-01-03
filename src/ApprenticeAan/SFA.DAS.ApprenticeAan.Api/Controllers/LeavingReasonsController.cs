using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApprenticeAan.Application.LeavingReasons.Queries.GetLeavingReasons;
using SFA.DAS.ApprenticeAan.Application.Model;

namespace SFA.DAS.ApprenticeAan.Api.Controllers;

[Route("[controller]")]
public class LeavingReasonsController : Controller
{
    private readonly IMediator _mediator;

    public LeavingReasonsController(IMediator mediator) => _mediator = mediator;

    /// <summary>
    ///     Get list of leaving reasons
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [Produces("application/json")]
    [ProducesResponseType(typeof(List<LeavingCategory>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetLeavingReasons(CancellationToken cancellationToken)
    {
        return Ok(await _mediator.Send(new GetLeavingReasonsQuery(), cancellationToken));
    }
}