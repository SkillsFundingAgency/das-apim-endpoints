using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.EmployerAan.Application.Employer.Queries.GetEmployerMemberSummary;
using SFA.DAS.EmployerAan.Application.MemberProfiles.Queries.GetMemberProfileWithPreferences;
using SFA.DAS.EmployerAan.Infrastructure;
using SFA.DAS.EmployerAan.Models;

namespace SFA.DAS.EmployerAan.Api.Controllers;

[Route("Members")]
public class MemberProfilesController : ControllerBase
{
    private readonly IMediator _mediator;

    public MemberProfilesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{memberId}/profile")]
    [ProducesResponseType(typeof(GetMemberProfileWithPreferencesModel), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetMemberProfileWithPreferences(
        [FromRoute] Guid memberId,
        [FromHeader(Name = Constants.ApiHeaders.RequestedByMemberIdHeader)] Guid requestedByMemberId,
        CancellationToken cancellationToken,
        bool @public = true)
    {
        var memberProfileWithPreferences = await _mediator.Send(new GetMemberProfileWithPreferencesQuery(memberId, requestedByMemberId, @public), cancellationToken);
        var employerMemberSummary = await _mediator.Send(new GetEmployerMemberSummaryQuery(memberProfileWithPreferences.AccountId), cancellationToken);

        return Ok(new GetMemberProfileWithPreferencesModel(memberProfileWithPreferences, employerMemberSummary));
    }
}
