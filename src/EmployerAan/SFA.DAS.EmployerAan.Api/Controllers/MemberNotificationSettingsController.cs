using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.EmployerAan.Application.MemberNotificationSettings.Queries.GetMemberNotificationSettings;

namespace SFA.DAS.EmployerAan.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class MemberNotificationSettingsController : ControllerBase
{
    private readonly IMediator _mediator;
    public MemberNotificationSettingsController(IMediator mediator) => _mediator = mediator;

    [HttpGet("{memberId}")]
    [ProducesResponseType(typeof(GetMemberNotificationSettingsQueryResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetMemberNotificationEventFormats(
        [FromRoute] Guid memberId,
        CancellationToken cancellationToken)
    {
        var memberNotificationSettings = await _mediator.Send(new GetMemberNotificationSettingsQuery(memberId), cancellationToken);

        return Ok(memberNotificationSettings);
    }
}