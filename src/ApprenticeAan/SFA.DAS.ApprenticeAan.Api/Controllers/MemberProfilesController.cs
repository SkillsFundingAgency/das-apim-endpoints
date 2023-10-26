using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApprenticeAan.Application.Common;
using SFA.DAS.ApprenticeAan.Application.Employer.Queries.GetEmployerMemberSummary;
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
        var memberProfileWithPreferences = await _mediator.Send(new GetMemberProfileWithPreferencesQuery(memberId, requestedByMemberId, @public), cancellationToken);

        var isApprenticeshipSectionShared = memberProfileWithPreferences.Preferences.Single(x => x.PreferenceId == Constants.PreferenceIds.Apprenticeship).Value;

        if (@public && !isApprenticeshipSectionShared)
        {
            return Ok(new GetMemberProfileWithPreferencesModel(memberProfileWithPreferences, null, null, @public));
        }

        if (memberProfileWithPreferences.UserType == MemberUserType.Apprentice)
        {
            var myApprenticeship = await _mediator.Send(new GetMyApprenticeshipQuery { ApprenticeId = memberProfileWithPreferences.ApprenticeId }, cancellationToken);
            return Ok(new GetMemberProfileWithPreferencesModel(memberProfileWithPreferences, myApprenticeship, null, @public));
        }
        else
        {
            var employerMemberSummary = await _mediator.Send(new GetEmployerMemberSummaryQuery(memberProfileWithPreferences.AccountId), cancellationToken);

            Apprenticeship apprenticeship = new() { Sectors = employerMemberSummary.Sectors, ActiveApprenticesCount = employerMemberSummary.ActiveCount };

            return Ok(new GetMemberProfileWithPreferencesModel(memberProfileWithPreferences, null, apprenticeship, @public));
        }
    }

    [HttpPut("{memberId}/profile")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> PutMemberProfile(
        Guid memberId,
        [FromHeader(Name = Constants.ApiHeaders.RequestedByMemberIdHeader)] Guid requestedByMemberId,
        [FromBody] UpdateMemberProfileModel request, CancellationToken cancellationToken)
    {

        var profiles = new List<UpdateProfileModel> { new UpdateProfileModel { ProfileId = 1, Value = "test this" } };
        request.Model.Profiles = profiles;

        UpdateMemberProfilesCommand command = new(memberId, requestedByMemberId, request.Model);
        await _mediator.Send(command, cancellationToken);
        return NoContent();
    }
}
