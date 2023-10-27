using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.EmployerAan.Application.Employer.Queries.GetEmployerMemberSummary;
using SFA.DAS.EmployerAan.Application.MemberProfiles.Queries.GetMemberProfileWithPreferences;
using SFA.DAS.EmployerAan.Application.MemberProfiles.Commands.PutMemberProfile;
using SFA.DAS.EmployerAan.Application.MyApprenticeships.Queries.GetMyApprenticeship;
using SFA.DAS.EmployerAan.Common;
using SFA.DAS.EmployerAan.Infrastructure;
using SFA.DAS.EmployerAan.InnerApi.MemberProfiles;
using SFA.DAS.EmployerAan.Models;

namespace SFA.DAS.EmployerAan.Api.Controllers;

[ApiController]
[Route("members")]
public class MemberProfilesController : ControllerBase
{
    private readonly IMediator _mediator;
    public MemberProfilesController(IMediator mediator) => _mediator = mediator;

    [HttpGet("{memberId}/profile")]
    [ProducesResponseType(typeof(GetMemberProfileWithPreferencesModel), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetMemberProfileWithPreferences(
        [FromRoute] Guid memberId,
        [FromHeader(Name = Constants.ApiHeaders.RequestedByMemberIdHeader)] Guid requestedByMemberId,
        CancellationToken cancellationToken,
        bool @public = true)
    {
        var memberProfileWithPreferences = await _mediator.Send(new GetMemberProfileWithPreferencesQuery(memberId, requestedByMemberId, @public), cancellationToken);

        var isApprenticeshipSectionShared = memberProfileWithPreferences.Preferences.Single(x => x.PreferenceId == Constants.PreferenceIds.Apprenticeship).Value;

        if (@public && !isApprenticeshipSectionShared)
        {
            return Ok(new GetMemberProfileWithPreferencesModel(memberProfileWithPreferences, null, null));
        }

        if (memberProfileWithPreferences.UserType == MemberUserType.Apprentice)
        {
            var myApprenticeship = await _mediator.Send(new GetMyApprenticeshipQuery(memberProfileWithPreferences.ApprenticeId), cancellationToken);
            return Ok(new GetMemberProfileWithPreferencesModel(memberProfileWithPreferences, myApprenticeship, null));
        }

        var employerMemberSummary = await _mediator.Send(new GetEmployerMemberSummaryQuery(memberProfileWithPreferences.AccountId), cancellationToken);
        return Ok(new GetMemberProfileWithPreferencesModel(memberProfileWithPreferences, null, employerMemberSummary));
    }

        [HttpPut("{memberId}/profile")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> PutMemberProfile(
        [FromRoute] Guid memberId,
        [FromHeader(Name = Constants.ApiHeaders.RequestedByMemberIdHeader)] Guid requestedByMemberId,
        [FromBody] UpdateMemberProfileModel request, CancellationToken cancellationToken)
    {
        UpdateMemberProfilesCommand command = new(memberId, requestedByMemberId, request);
        await _mediator.Send(command, cancellationToken);
        return NoContent();
    }
}
