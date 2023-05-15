using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApprenticeAan.Application.ApprenticeAccount.Queries.GetApprenticeAccount;
using SFA.DAS.ApprenticeAan.Application.Apprentices.Commands.CreateApprenticeMember;
using SFA.DAS.ApprenticeAan.Application.Apprentices.Queries.GetApprentice;

namespace SFA.DAS.ApprenticeAan.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class ApprenticesController : ControllerBase
{
    private readonly IMediator _mediator;

    public ApprenticesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [Route("{apprenticeId}/account")]
    [ProducesResponseType(typeof(GetApprenticeAccountQueryResult), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAccount(Guid apprenticeId, CancellationToken cancellationToken)
    {
        var apprenticeAccountDetails = await _mediator.Send(new GetApprenticeAccountQuery { ApprenticeId = apprenticeId }, cancellationToken);
        if (apprenticeAccountDetails == null) return NotFound();
        return Ok(apprenticeAccountDetails);
    }

    [HttpGet]
    [Route("{apprenticeId}")]
    [ProducesResponseType(typeof(GetApprenticeQueryResult), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetApprentice(Guid apprenticeId, CancellationToken cancellationToken)
    {
        var apprentice = await _mediator.Send(new GetApprenticeQuery(apprenticeId), cancellationToken);

        if (apprentice == null) return NotFound();

        return Ok(apprentice);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> CreateApprenticeMember([FromBody] CreateApprenticeMemberCommand command, CancellationToken cancellationToken)
    {
        await _mediator.Send(command, cancellationToken);
        return Ok();
    }
}
