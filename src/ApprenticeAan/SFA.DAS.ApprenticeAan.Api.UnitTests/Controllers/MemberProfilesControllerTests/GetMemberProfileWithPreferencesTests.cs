using System.Net;
using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SFA.DAS.ApprenticeAan.Api.Controllers;
using SFA.DAS.ApprenticeAan.Application.Common;
using SFA.DAS.ApprenticeAan.Application.Employer.Queries.GetEmployerMemberSummary;
using SFA.DAS.ApprenticeAan.Application.InnerApi.MyApprenticeships;
using SFA.DAS.ApprenticeAan.Application.InnerApi.Standards.Requests;
using SFA.DAS.ApprenticeAan.Application.MemberProfiles.Queries.GetMemberProfileWithPreferences;
using SFA.DAS.ApprenticeAan.Application.Model;
using SFA.DAS.ApprenticeAan.Application.MyApprenticeships.Queries.GetMyApprenticeship;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
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
        MyApprenticeship? myApprenticeship,
        Apprenticeship? apprenticeship,
        [Frozen] Mock<IMediator> mediatorMock,
        Guid appreticeId,
        long accountId,
        Guid memberId,
        Guid requestedByMemberId,
        GetStandardResponse standardResponse,
        string standardUid,
        CancellationToken cancellationToken)
    {
        GetMemberProfileWithPreferencesQueryResult memberProfileWithPreferencesQueryResult = new GetMemberProfileWithPreferencesQueryResult();
        memberProfileWithPreferencesQueryResult.ApprenticeId = appreticeId;
        memberProfileWithPreferencesQueryResult.AccountId = accountId;
        memberProfileWithPreferencesQueryResult.UserType = userType;
        GetMyApprenticeshipQuery myApprenticeshipQuery = new() { ApprenticeId = appreticeId };
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
            response = new(memberProfileWithPreferencesQueryResult, null, null, isPublicView);
        }
        else if (memberProfileWithPreferencesQueryResult.UserType == MemberUserType.Apprentice && myApprenticeship != null)
        {
            response = new(memberProfileWithPreferencesQueryResult, myApprenticeship, null, isPublicView);
        }
        else
        {
            if (apprenticeship != null)
            {
                getEmployerMemberSummaryQueryResult!.ActiveCount = apprenticeship.ActiveApprenticesCount;
                getEmployerMemberSummaryQueryResult!.Sectors = apprenticeship.Sectors;
            }
            response = new(memberProfileWithPreferencesQueryResult, null, apprenticeship, isPublicView);
            mediatorMock.Setup(m => m.Send(It.Is<GetEmployerMemberSummaryQuery>(x => x.EmployerAccountId == accountId), cancellationToken)).ReturnsAsync(getEmployerMemberSummaryQueryResult!);
        }

        mediatorMock.Setup(m => m.Send(It.Is<GetMyApprenticeshipQuery>(x => x.ApprenticeId == appreticeId), cancellationToken)).ReturnsAsync(myApprenticeship);

        mediatorMock.Setup(m => m.Send(It.IsAny<GetMemberProfileWithPreferencesQuery>(), cancellationToken)).ReturnsAsync(memberProfileWithPreferencesQueryResult);

        Mock<IApprenticeAccountsApiClient<ApprenticeAccountsApiConfiguration>> apprenticeAccountsApiClientMock = new();

        MyApprenticeshipResponse myApprenticeshipResponse = new() { StandardUId = standardUid };

        apprenticeAccountsApiClientMock.Setup(c => c.GetWithResponseCode<MyApprenticeshipResponse>(It.Is<GetMyApprenticeshipRequest>(r => r.Id == myApprenticeshipQuery.ApprenticeId)))
            .ReturnsAsync(new ApiResponse<MyApprenticeshipResponse>(myApprenticeshipResponse, HttpStatusCode.OK, null));

        Mock<ICoursesApiClient<CoursesApiConfiguration>> coursesApiClientMock = new();
        coursesApiClientMock.Setup(c => c.Get<GetStandardResponse>(It.IsAny<GetStandardQueryRequest>())).ReturnsAsync(standardResponse);


        var sut = new MemberProfilesController(mediatorMock.Object);

        var result = await sut.GetMemberProfileWithPreferences(memberId, requestedByMemberId, cancellationToken, isPublicView);

        result.As<OkObjectResult>().Value.Should().BeEquivalentTo(response);
    }
}