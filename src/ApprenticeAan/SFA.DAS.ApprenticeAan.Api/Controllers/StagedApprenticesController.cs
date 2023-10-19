using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApprenticeAan.Application.ApprenticeAccount.Queries.GetApprenticeAccount;
using SFA.DAS.ApprenticeAan.Application.StagedApprentices.Queries.GetStagedApprentice;

namespace SFA.DAS.ApprenticeAan.Api.Controllers;

[Route("[controller]")]
public class StagedApprenticesController : ControllerBase
{
    private readonly IMediator _mediator;

    public StagedApprenticesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    [ProducesResponseType(typeof(GetApprenticeAccountQueryResult), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]

    public async Task<IActionResult> GetStagedApprentice(GetStagedApprenticeQuery query, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(query, cancellationToken);
        return result == null ? NotFound() : Ok(result);
    }
}
