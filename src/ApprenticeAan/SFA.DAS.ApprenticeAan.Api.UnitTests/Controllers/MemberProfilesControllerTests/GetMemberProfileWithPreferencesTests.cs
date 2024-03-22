using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SFA.DAS.ApprenticeAan.Api.Controllers;
using SFA.DAS.ApprenticeAan.Application.Common;
using SFA.DAS.ApprenticeAan.Application.Employer.Queries.GetEmployerMemberSummary;
using SFA.DAS.ApprenticeAan.Application.Infrastructure;
using SFA.DAS.ApprenticeAan.Application.InnerApi.MyApprenticeships;
using SFA.DAS.ApprenticeAan.Application.InnerApi.Standards.Responses;
using SFA.DAS.ApprenticeAan.Application.MemberProfiles.Queries.GetMemberProfileWithPreferences;
using SFA.DAS.ApprenticeAan.Application.Models;
using SFA.DAS.ApprenticeAan.Application.MyApprenticeships.Queries.GetMyApprenticeship;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.ApprenticeAan.Api.UnitTests.Controllers.MemberProfiles;
public class GetMemberProfileWithPreferencesTests
{
    [Test]
    [MoqInlineAutoData(true, true, MemberUserType.Apprentice)]
    [MoqInlineAutoData(true, false, MemberUserType.Apprentice)]
    [MoqInlineAutoData(false, true, MemberUserType.Apprentice)]
    [MoqInlineAutoData(false, false, MemberUserType.Apprentice)]
    [MoqInlineAutoData(true, true, MemberUserType.Employer)]
    [MoqInlineAutoData(true, false, MemberUserType.Employer)]
    [MoqInlineAutoData(false, true, MemberUserType.Employer)]
    [MoqInlineAutoData(false, false, MemberUserType.Employer)]
    public async Task When_MediatorCommandSuccessful_Then_ReturnOk(
         bool isPublicView,
         bool isApprenticeshipSectionShow,
         MemberUserType userType,
         GetMyApprenticeshipQueryResult? myApprenticeship,
         Apprenticeship? apprenticeship,
         [Frozen] Mock<IMediator> mediatorMock,
         Guid apprenticeId,
         long accountId,
         Guid memberId,
         Guid requestedByMemberId,
         GetStandardResponse standardResponse,
         string standardUid,
         CancellationToken cancellationToken)
    {
        GetMemberProfileWithPreferencesQueryResult memberProfileWithPreferencesQueryResult = new GetMemberProfileWithPreferencesQueryResult();
        memberProfileWithPreferencesQueryResult.ApprenticeId = apprenticeId;
        memberProfileWithPreferencesQueryResult.AccountId = accountId;
        memberProfileWithPreferencesQueryResult.UserType = userType;
        GetMyApprenticeshipQuery myApprenticeshipQuery = new() { ApprenticeId = apprenticeId };
        GetEmployerMemberSummaryQuery employerMemberSummaryQuery = new(accountId);
        List<MemberPreference> memberPreferences = new()
         {
             new MemberPreference{ PreferenceId = 1, Value =  true },
             new MemberPreference{ PreferenceId = 2, Value =  true },
             new MemberPreference{ PreferenceId = 3, Value =  isApprenticeshipSectionShow },
             new MemberPreference{ PreferenceId = 4, Value =  false },
         };
        memberProfileWithPreferencesQueryResult.Preferences = memberPreferences;
        int apprenticeshipPreferenceId = 3;
        var isApprenticeSectionShareAllowed = (memberProfileWithPreferencesQueryResult.Preferences.Any(x => x.PreferenceId == apprenticeshipPreferenceId)) ? memberProfileWithPreferencesQueryResult.Preferences.FirstOrDefault(x => x.PreferenceId == apprenticeshipPreferenceId)!.Value : false;
        GetEmployerMemberSummaryQueryResult? getEmployerMemberSummaryQueryResult = new GetEmployerMemberSummaryQueryResult();
        if (isPublicView && !isApprenticeSectionShareAllowed)
        {
            myApprenticeship = null;
            apprenticeship = null;
            getEmployerMemberSummaryQueryResult = null;
        }
        GetMemberProfileWithPreferencesModel response = null!;

        if (isPublicView && !isApprenticeSectionShareAllowed)
        {
            response = new(memberProfileWithPreferencesQueryResult, null, null);
        }
        else if (memberProfileWithPreferencesQueryResult.UserType == MemberUserType.Apprentice && myApprenticeship != null)
        {
            response = new(memberProfileWithPreferencesQueryResult, myApprenticeship, null);
        }
        else
        {
            if (apprenticeship != null)
            {
                getEmployerMemberSummaryQueryResult!.ActiveCount = apprenticeship.ActiveApprenticesCount;
                getEmployerMemberSummaryQueryResult!.Sectors = apprenticeship.Sectors;
            }
            response = new(memberProfileWithPreferencesQueryResult, null, apprenticeship);
            mediatorMock.Setup(m => m.Send(It.Is<GetEmployerMemberSummaryQuery>(x => x.EmployerAccountId == accountId), cancellationToken)).ReturnsAsync(getEmployerMemberSummaryQueryResult!);
        }

        mediatorMock.Setup(m => m.Send(It.Is<GetMyApprenticeshipQuery>(x => x.ApprenticeId == apprenticeId), cancellationToken)).ReturnsAsync(myApprenticeship);

        mediatorMock.Setup(m => m.Send(It.IsAny<GetMemberProfileWithPreferencesQuery>(), cancellationToken)).ReturnsAsync(memberProfileWithPreferencesQueryResult);

        Mock<IApprenticeAccountsApiClient> apprenticeAccountsApiClientMock = new();

        GetMyApprenticeshipResponse myApprenticeshipResponse = new() { StandardUId = standardUid };

        var status = System.Net.HttpStatusCode.OK;

        var restApprenticeshipResponse = new RestEase.Response<GetMyApprenticeshipResponse>(
            "not used",
            new HttpResponseMessage(status),
            () => myApprenticeshipResponse);

        apprenticeAccountsApiClientMock.Setup(c => c.GetMyApprenticeship(myApprenticeshipQuery.ApprenticeId, It.IsAny<CancellationToken>()))
             .ReturnsAsync(restApprenticeshipResponse);

        Mock<ICoursesApiClient> coursesApiClientMock = new();

        var restStandardResponse = new RestEase.Response<GetStandardResponse>(
            "not used",
            new HttpResponseMessage(status),
            () => standardResponse);

        coursesApiClientMock.Setup(x => x.GetStandard(standardUid, It.IsAny<CancellationToken>()))
            .ReturnsAsync(restStandardResponse);

        var sut = new MemberProfilesController(mediatorMock.Object);

        var result = await sut.GetMemberProfileWithPreferences(memberId, requestedByMemberId, cancellationToken, isPublicView);

        result.As<OkObjectResult>().Value.Should().BeEquivalentTo(response);
    }
}