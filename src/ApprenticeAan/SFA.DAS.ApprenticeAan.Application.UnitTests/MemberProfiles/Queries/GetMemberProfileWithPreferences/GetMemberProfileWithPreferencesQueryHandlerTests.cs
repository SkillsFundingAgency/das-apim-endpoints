using System.Net;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using SFA.DAS.ApprenticeAan.Application.Infrastructure;
using SFA.DAS.ApprenticeAan.Application.InnerApi.MyApprenticeships;
using SFA.DAS.ApprenticeAan.Application.InnerApi.Standards.Requests;
using SFA.DAS.ApprenticeAan.Application.MemberProfiles.Queries.GetMemberProfileWithPreferences;
using SFA.DAS.ApprenticeAan.Application.Members.Queries.GetMember;
using SFA.DAS.ApprenticeAan.Application.MyApprenticeships.Queries.GetMyApprenticeship;
using SFA.DAS.ApprenticeAan.Application.Regions.Queries.GetRegions;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.ApprenticeAan.Application.UnitTests.MemberProfiles.Queries.GetMemberProfileWithPreferences;

public class GetMemberProfileWithPreferencesQueryHandlerTests
{
    [Test, MoqAutoData]
    public async Task Handle_ReturnAllProfiles(
        [Frozen] Mock<IAanHubRestApiClient> apiClient,
        [Frozen] Mock<IApprenticeAccountsApiClient<ApprenticeAccountsApiConfiguration>> apprenticeAccountsApiClientMock,
        [Frozen] Mock<ICoursesApiClient<CoursesApiConfiguration>> coursesApiClientMock,
        GetMemberProfileWithPreferencesQueryHandler handler,
        GetMemberProfileWithPreferencesQuery query,
        GetMemberProfileWithPreferencesQueryResult expected,
        GetMemberQueryResult memberResult,
        GetRegionsQueryResult regionsResult,
        MyApprenticeshipResponse response,
        GetMyApprenticeshipQuery request,
        CancellationToken cancellationToken)
    {

        memberResult.UserType = Common.MemberUserType.Apprentice.ToString();
        memberResult.ApprenticeId = request.ApprenticeId;
        regionsResult.Regions.Add(new Entities.Region { Id = (int)memberResult.RegionId!, Area = Guid.NewGuid().ToString(), Ordering = int.MinValue });

        apiClient.Setup(x => x.GetMember(query.MemberId, cancellationToken)).ReturnsAsync(memberResult);
        apiClient.Setup(x => x.GetRegions(cancellationToken)).ReturnsAsync(regionsResult);

        apiClient.Setup(x => x.GetMemberProfileWithPreferences(query.MemberId, query.MemberId, It.IsAny<bool>(), cancellationToken)).ReturnsAsync(expected);

        apprenticeAccountsApiClientMock.Setup(c => c.GetWithResponseCode<MyApprenticeshipResponse>(It.Is<GetMyApprenticeshipRequest>(r => r.Id == request.ApprenticeId)))!.ReturnsAsync(new ApiResponse<MyApprenticeshipResponse>(response, HttpStatusCode.OK, null));

        const string StandardUid = "ST0418_1.0";
        const string TrainingCode = null!;
        const string StandardRoute = "route of standard";
        const int Duration = 36;
        const string Title = "Title";
        const int Level = 3;
        response.StandardUId = StandardUid;
        response.TrainingCode = TrainingCode;

        var standardResponse = new GetStandardResponse
        {
            Title = Title,
            Level = Level,
            Route = StandardRoute,
            VersionDetail = new StandardVersionDetail { ProposedTypicalDuration = Duration }
        };

        coursesApiClientMock.Setup(c => c.Get<GetStandardResponse>(It.IsAny<GetStandardQueryRequest>()))
            .ReturnsAsync(standardResponse);

        coursesApiClientMock.Setup(c => c.Get<GetFrameworkResponse>(It.IsAny<GetFrameworkQueryRequest>()))
            .ReturnsAsync(new GetFrameworkResponse());

        var actual = await handler.Handle(query, cancellationToken);

        actual.Should().Be(expected);
    }
}