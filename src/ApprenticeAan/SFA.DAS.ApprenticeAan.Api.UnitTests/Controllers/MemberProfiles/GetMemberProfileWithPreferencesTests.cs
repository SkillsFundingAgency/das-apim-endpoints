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
    [Test]
    [MoqInlineAutoData(false)]
    [MoqInlineAutoData(true)]
    public async Task When_MediatorCommandSuccessful_Then_ReturnOk(
        bool isPublicView,
        GetMemberProfileWithPreferencesQueryResult memberProfileWithPreferencesQueryResult,
        MyApprenticeship myApprenticeship,
        [Frozen] Mock<IMediator> mediatorMock,
        Guid appreticeId,
        Guid memberId,
        Guid requestedByMemberId,
        GetStandardResponse standardResponse,
        string standardUid,
        CancellationToken cancellationToken)
    {
        memberProfileWithPreferencesQueryResult.ApprenticeId = appreticeId;
        GetMyApprenticeshipQuery myApprenticeshipQuery = new() { ApprenticeId = appreticeId };

        GetMemberProfileWithPreferencesModel response = new(memberProfileWithPreferencesQueryResult, myApprenticeship);

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