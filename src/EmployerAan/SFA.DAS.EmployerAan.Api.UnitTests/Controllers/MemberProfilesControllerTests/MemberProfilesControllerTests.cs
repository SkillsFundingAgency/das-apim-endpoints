using System.Net;
using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SFA.DAS.EmployerAan.Api.Controllers;
using SFA.DAS.EmployerAan.Application.MemberProfiles.Commands.PutMemberProfile;
using SFA.DAS.EmployerAan.InnerApi.MemberProfiles;
using SFA.DAS.EmployerAan.Application.Employer.Queries.GetEmployerMemberSummary;
using SFA.DAS.EmployerAan.Application.InnerApi.MyApprenticeships;
using SFA.DAS.EmployerAan.Application.InnerApi.Standards.Requests;
using SFA.DAS.EmployerAan.Application.MemberProfiles.Queries.GetMemberProfileWithPreferences;
using SFA.DAS.EmployerAan.Application.MyApprenticeships.Queries.GetMyApprenticeship;
using SFA.DAS.EmployerAan.Common;
using SFA.DAS.EmployerAan.InnerApi.MyApprenticeships;
using SFA.DAS.EmployerAan.Models;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerAan.Api.UnitTests.Controllers.MemberProfilesControllerTests;
public class MemberProfilesControllerTests
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
        GetEmployerMemberSummaryQueryResult? employerMemberSummaryQueryResult,
        [Frozen] Mock<IMediator> mockMediator,
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
        GetMyApprenticeshipQuery myApprenticeshipQuery = new(appreticeId);
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
            employerMemberSummaryQueryResult = null;
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
            response = new(memberProfileWithPreferencesQueryResult, null, employerMemberSummaryQueryResult);
            mockMediator.Setup(m => m.Send(It.Is<GetEmployerMemberSummaryQuery>(x => x.EmployerAccountId == accountId), cancellationToken)).ReturnsAsync(employerMemberSummaryQueryResult!);
        }

        mockMediator.Setup(m => m.Send(It.Is<GetMyApprenticeshipQuery>(x => x.ApprenticeId == appreticeId), cancellationToken)).ReturnsAsync(myApprenticeship);
        mockMediator.Setup(m => m.Send(It.IsAny<GetMemberProfileWithPreferencesQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(memberProfileWithPreferencesQueryResult);

        Mock<IApprenticeAccountsApiClient<ApprenticeAccountsApiConfiguration>> apprenticeAccountsApiClientMock = new();

        GetMyApprenticeshipResponse myApprenticeshipResponse = new() { StandardUId = standardUid };

        apprenticeAccountsApiClientMock.Setup(c => c.GetWithResponseCode<GetMyApprenticeshipResponse>(It.Is<GetMyApprenticeshipRequest>(r => r.Id == myApprenticeshipQuery.ApprenticeId)))
            .ReturnsAsync(new ApiResponse<GetMyApprenticeshipResponse>(myApprenticeshipResponse, HttpStatusCode.OK, null));

        Mock<ICoursesApiClient<CoursesApiConfiguration>> coursesApiClientMock = new();
        coursesApiClientMock.Setup(c => c.Get<GetStandardResponse>(It.IsAny<GetStandardQueryRequest>())).ReturnsAsync(standardResponse);


        var sut = new MemberProfilesController(mockMediator.Object);

        var result = await sut.GetMemberProfileWithPreferences(memberId, requestedByMemberId, cancellationToken, isPublicView);

        result.As<OkObjectResult>().Value.Should().BeEquivalentTo(response);
    }
    
    [Test, MoqAutoData]
    public async Task PutMemberProfile_InvokesSendWithCommandProperties(
        [Frozen] Mock<IMediator> mediatorMock,
        Guid memberId,
        Guid requestedByMemberId,
        UpdateMemberProfileModel request,
        CancellationToken cancellationToken)
    {
        var sut = new MemberProfilesController(mediatorMock.Object);

        await sut.PutMemberProfile(memberId, requestedByMemberId, request, cancellationToken);

        mediatorMock.Verify(m => m.Send(
            It.Is<UpdateMemberProfilesCommand>(
                c => c.MemberId == memberId
                && c.RequestedByMemberId == requestedByMemberId), cancellationToken),
                     Times.Once());
    }

    [Test, MoqAutoData]
    public async Task PutMemberProfile_ReturnsNoContent(
    [Frozen] Mock<IMediator> mediatorMock,
    Guid memberId,
    Guid requestedByMemberId,
    UpdateMemberProfileModel request,
    CancellationToken cancellationToken)
    {
        var sut = new MemberProfilesController(mediatorMock.Object);

        var result = await sut.PutMemberProfile(memberId, requestedByMemberId, request, cancellationToken);

        result.Should().BeOfType<NoContentResult>();
    }
}
