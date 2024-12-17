using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.EmployerAan.Application.MemberNotificationEventFormat.Queries.GetMemberNotificationEventFormats;

namespace SFA.DAS.EmployerAan.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class MemberNotificationEventFormatsController : ControllerBase
{
    private readonly IMediator _mediator;
    public MemberNotificationEventFormatsController(IMediator mediator) => _mediator = mediator;

    [HttpGet("{memberId}")]
    [ProducesResponseType(typeof(GetMemberNotificationEventFormatsQueryResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetMemberNotificationEventFormats(
        [FromRoute] Guid memberId,
        CancellationToken cancellationToken)
    {
        var eventFormats = await _mediator.Send(new GetMemberNotificationEventFormatsQuery(memberId), cancellationToken);

        return Ok(eventFormats);
    }
}