using System.Net;
using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SFA.DAS.ApprenticeAan.Api.Controllers;
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
    static readonly List<MemberPreference> memberPreferences = new()
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
        MyApprenticeship? myApprenticeship,
        [Frozen] Mock<IMediator> mediatorMock,
        Guid appreticeId,
        Guid memberId,
        Guid requestedByMemberId,
        GetStandardResponse standardResponse,
        string standardUid,
        CancellationToken cancellationToken)
    {
        GetMemberProfileWithPreferencesQueryResult memberProfileWithPreferencesQueryResult = new GetMemberProfileWithPreferencesQueryResult();
        memberProfileWithPreferencesQueryResult.ApprenticeId = appreticeId;
        GetMyApprenticeshipQuery myApprenticeshipQuery = new() { ApprenticeId = appreticeId };
        List<MemberPreference> memberPreference = new List<MemberPreference>();
        memberPreference.AddRange(memberPreferences);
        if (isApprenticeshipSectionShow)
        {
            memberPreference.Add(new MemberPreference() { PreferenceId = 3, Value = true });
        }
        memberProfileWithPreferencesQueryResult.Preferences = memberPreference;
        int apprenticeshipPreferenceId = 3;
        var isApprenticeSectionShareAllowed = (memberProfileWithPreferencesQueryResult.Preferences.Any(x => x.PreferenceId == apprenticeshipPreferenceId)) ? memberProfileWithPreferencesQueryResult.Preferences.FirstOrDefault(x => x.PreferenceId == apprenticeshipPreferenceId)!.Value : false;
        if (isPublicView && !isApprenticeSectionShareAllowed)
        {
            myApprenticeship = null;
        }

        GetMemberProfileWithPreferencesModel response = new(memberProfileWithPreferencesQueryResult, myApprenticeship, isPublicView);

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