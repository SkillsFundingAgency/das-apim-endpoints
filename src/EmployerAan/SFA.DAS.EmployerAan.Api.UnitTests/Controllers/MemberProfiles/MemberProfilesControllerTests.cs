using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SFA.DAS.EmployerAan.Api.Controllers;
using SFA.DAS.EmployerAan.Application.Employer.Queries.GetEmployerMemberSummary;
using SFA.DAS.EmployerAan.Application.MemberProfiles.Queries.GetMemberProfileWithPreferences;
using SFA.DAS.EmployerAan.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerAan.Api.UnitTests.Controllers.MemberProfiles;

public class MemberProfilesControllerTests
{
    List<MemberPreference> memberPreferences = new()
        {
            new MemberPreference{ PreferenceId = 1, Value =  true },
            new MemberPreference{ PreferenceId = 2, Value =  true },
            new MemberPreference{ PreferenceId = 4, Value =  false },
        };

    [Test]
    [MoqInlineAutoData(true, true)]
    [MoqInlineAutoData(true, false)]
    [MoqInlineAutoData(false, true)]
    [MoqInlineAutoData(false, false)]
    public async Task When_MediatorCommandSuccessful_Then_ReturnOk(
        bool isPublicView,
        bool isApprenticeshipSectionShow,
        GetMemberProfileWithPreferencesQueryResult memberProfileWithPreferencesQueryResult,
        GetEmployerMemberSummaryQueryResult? employerMemberSummaryQueryResult,
        [Frozen] Mock<IMediator> mockMediator,
        long accountId,
        Guid memberId,
        Guid requestedByMemberId,
        CancellationToken cancellationToken)
    {
        memberProfileWithPreferencesQueryResult.AccountId = accountId;
        GetEmployerMemberSummaryQuery employerMemberSummaryQuery = new(accountId);
        List<MemberPreference> memberPreference = new List<MemberPreference>();
        memberPreference.AddRange(memberPreferences);
        if (isApprenticeshipSectionShow)
        {
            memberPreference.Add(new MemberPreference() { PreferenceId = 3, Value = true });
        }
        memberProfileWithPreferencesQueryResult.Preferences = memberPreference;
        int apprenticeshipPreferenceId = 3;
        var isApprenticeSectionShareAllowed = (memberProfileWithPreferencesQueryResult.Preferences.Any(x => x.PreferenceId == apprenticeshipPreferenceId)) ? memberProfileWithPreferencesQueryResult.Preferences.FirstOrDefault(x => x.PreferenceId == apprenticeshipPreferenceId)!.Value : false;
        if (!(isPublicView && !isApprenticeSectionShareAllowed))
        {
            mockMediator.Setup(m => m.Send(It.IsAny<GetEmployerMemberSummaryQuery>(), It.IsAny<CancellationToken>()))!.ReturnsAsync(employerMemberSummaryQueryResult);
        }
        else
        {
            employerMemberSummaryQueryResult = null;
        }
        mockMediator.Setup(m => m.Send(It.IsAny<GetMemberProfileWithPreferencesQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(memberProfileWithPreferencesQueryResult);

        GetMemberProfileWithPreferencesModel response = new(memberProfileWithPreferencesQueryResult, employerMemberSummaryQueryResult);
        var sut = new MemberProfilesController(mockMediator.Object);
        var result = await sut.GetMemberProfileWithPreferences(memberId, requestedByMemberId, cancellationToken, isPublicView);

        result.As<OkObjectResult>().Value.Should().BeEquivalentTo(response);
    }
}
