using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApprenticeAan.Application.Infrastructure.Configuration;
using SFA.DAS.ApprenticeAan.Application.InnerApi.MemberProfiles;
using SFA.DAS.ApprenticeAan.Application.MemberProfiles;
using SFA.DAS.ApprenticeAan.Application.MemberProfiles.Queries.GetMemberProfileWithPreferences;
using SFA.DAS.ApprenticeAan.Application.Model;
using SFA.DAS.ApprenticeAan.Application.MyApprenticeships.Queries.GetMyApprenticeship;

namespace SFA.DAS.ApprenticeAan.Api.Controllers;

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
        int apprenticeshipPreferenceId = Constants.Preferences.Apprenticeship;
        var memberProfileWithPreferences = await _mediator.Send(new GetMemberProfileWithPreferencesQuery(memberId, requestedByMemberId, @public), cancellationToken);
        var isApprenticeSectionShareAllowedExist = memberProfileWithPreferences.Preferences.Any(x => x.PreferenceId == apprenticeshipPreferenceId);
        var isApprenticeSectionShareAllowed = isApprenticeSectionShareAllowedExist ? memberProfileWithPreferences.Preferences.FirstOrDefault(x => x.PreferenceId == apprenticeshipPreferenceId)!.Value : isApprenticeSectionShareAllowedExist;
        var myApprenticeship = (@public && !isApprenticeSectionShareAllowed) ? null : await _mediator.Send(new GetMyApprenticeshipQuery { ApprenticeId = memberProfileWithPreferences.ApprenticeId }, cancellationToken);

        return Ok(new GetMemberProfileWithPreferencesModel(memberProfileWithPreferences, myApprenticeship, @public));
    }

    [HttpPost("{memberId}/profile")]
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
