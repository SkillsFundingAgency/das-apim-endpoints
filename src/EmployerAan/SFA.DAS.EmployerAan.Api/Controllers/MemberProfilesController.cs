using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.EmployerAan.Application.MemberProfiles.Queries.GetMemberProfileWithPreferences;

namespace SFA.DAS.EmployerAan.Api.Controllers;
public class MemberProfilesController : ControllerBase
{
    private readonly IMediator _mediator;

    public MemberProfilesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{memberId}/profile")]
    [ProducesResponseType(typeof(GetMemberProfileWithPreferencesQueryResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetMemberProfileWithPreferences([FromRoute] Guid memberId,
            CancellationToken cancellationToken,
            bool @public = true)
    {
        return Ok(await _mediator.Send(new GetMemberProfileWithPreferencesQuery(memberId, @public), cancellationToken));
    }
}
