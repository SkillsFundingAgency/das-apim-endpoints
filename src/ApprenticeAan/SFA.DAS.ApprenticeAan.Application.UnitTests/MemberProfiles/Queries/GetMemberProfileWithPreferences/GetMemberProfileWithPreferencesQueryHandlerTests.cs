using System.Net;
using FluentAssertions;
using FluentAssertions.Execution;
using Moq;
using SFA.DAS.ApprenticeAan.Application.Common;
using SFA.DAS.ApprenticeAan.Application.Entities;
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

namespace SFA.DAS.ApprenticeAan.Application.UnitTests.MemberProfiles.Queries.GetMemberProfileWithPreferences;

public class GetMemberProfileWithPreferencesQueryHandlerTests
{
    readonly Guid memberId = Guid.NewGuid();
    readonly Guid apprenticeId = Guid.NewGuid();

    readonly string firstName = "Last";
    readonly string lastName = "First";
    readonly string organisationName = "organisation";
    readonly string fullName = "Full Name";
    readonly string email = "My_Email@domain.com";
    readonly int regionId = 1;
    readonly string regionName = "London";
    readonly MemberUserType userType = MemberUserType.Apprentice;
    readonly bool isRegionalChair = true;

    readonly string standardUid = "ST0418_1.0";
    readonly string standardRoute = "route of standard";
    readonly string title = "Title";
    readonly int level = 3;

    Mock<IAanHubRestApiClient> aanHubRestApiClientMock;
    Mock<IApprenticeAccountsApiClient<ApprenticeAccountsApiConfiguration>> apprenticeAccountsApiClientMock;
    Mock<ICoursesApiClient<CoursesApiConfiguration>> coursesApiClientMock;

    GetMyApprenticeshipQuery myApprenticeshipQuery = null!;
    MyApprenticeshipResponse myApprenticeshipResponse = null!;
    GetStandardResponse standardResponse = null!;

    GetMemberProfileWithPreferencesQueryHandler sut = null!;
    GetMemberProfileWithPreferencesQuery getMemberProfileWithPreferencesQuery = null!;

    GetMemberProfileWithPreferencesQueryResult expectedResult;
    GetMemberProfileWithPreferencesQueryResult actualResult = null!;

    GetMemberQueryResult memberResult = null!;
    GetRegionsQueryResult regionsResult = null!;

    CancellationToken cancellationToken = new CancellationToken();

    [SetUp]
    public async Task InitAsync()
    {
        getMemberProfileWithPreferencesQuery = new(memberId, memberId, true);

        aanHubRestApiClientMock = new();
        apprenticeAccountsApiClientMock = new();
        coursesApiClientMock = new();

        regionsResult = new() { Regions = new List<Region> { new Region { Id = regionId, Area = regionName, Ordering = 1 } } };
        memberResult = new()
        {
            MemberId = memberId,
            ApprenticeId = apprenticeId,
            Email = email,
            FullName = fullName,
            FirstName = firstName,
            LastName = lastName,
            OrganisationName = organisationName,
            IsRegionalChair = isRegionalChair,
            UserType = userType.ToString(),
            RegionId = regionId
        };

        expectedResult = new()
        {
            FullName = fullName,
            FirstName = firstName,
            LastName = lastName,
            OrganisationName = organisationName,
            Email = email,
            RegionId = regionId,
            RegionName = regionName,
            UserType = userType,
            IsRegionalChair = isRegionalChair,
            Apprenticeship = new Apprenticeship { Level = level.ToString(), Programmes = title, Sector = standardRoute }
        };

        expectedResult.Profiles = new[] {
                                            new MemberProfile { ProfileId=1,Value = Guid.NewGuid().ToString(), PreferenceId=1 },
                                            new MemberProfile { ProfileId=2,Value = Guid.NewGuid().ToString(), PreferenceId=2 }
                                        };

        expectedResult.Preferences = new[] {
                                                new MemberPreference { PreferenceId=1, Value = true    },
                                                new MemberPreference { PreferenceId=2, Value = false   }
                                            };

        aanHubRestApiClientMock.Setup(x => x.GetMemberProfileWithPreferences(getMemberProfileWithPreferencesQuery.MemberId, getMemberProfileWithPreferencesQuery.MemberId, It.IsAny<bool>(), cancellationToken)).ReturnsAsync(expectedResult);
        aanHubRestApiClientMock.Setup(x => x.GetMember(memberId, cancellationToken)).ReturnsAsync(memberResult);
        aanHubRestApiClientMock.Setup(x => x.GetRegions(cancellationToken)).ReturnsAsync(regionsResult);

        myApprenticeshipQuery = new GetMyApprenticeshipQuery { ApprenticeId = apprenticeId };
        myApprenticeshipResponse = new MyApprenticeshipResponse { StandardUId = standardUid };
        standardResponse = new GetStandardResponse { Title = title, Level = level, Route = standardRoute };

        coursesApiClientMock.Setup(c => c.Get<GetStandardResponse>(It.IsAny<GetStandardQueryRequest>())).ReturnsAsync(standardResponse);
        apprenticeAccountsApiClientMock.Setup(c => c.GetWithResponseCode<MyApprenticeshipResponse>(It.Is<GetMyApprenticeshipRequest>(r => r.Id == myApprenticeshipQuery.ApprenticeId)))
            .ReturnsAsync(new ApiResponse<MyApprenticeshipResponse>(myApprenticeshipResponse, HttpStatusCode.OK, null));

        sut = new GetMemberProfileWithPreferencesQueryHandler(aanHubRestApiClientMock.Object, coursesApiClientMock.Object, apprenticeAccountsApiClientMock.Object);

        await InvokeHandler();
    }

    private async Task InvokeHandler()
    {
        actualResult = await sut.Handle(getMemberProfileWithPreferencesQuery, cancellationToken);
    }

    [Test]
    public void Handle_InvokesGetMemberProfileWithPreferencesAPI()
    {
        using (new AssertionScope("Invokes GetMemberProfileWithPreferencesAPI & validates the Profile and Preferences"))
        {
            aanHubRestApiClientMock.Verify(x => x.GetMemberProfileWithPreferences(memberId, memberId, true, cancellationToken), Times.Once());
            actualResult.Profiles.Should().BeEquivalentTo(expectedResult.Profiles);
            actualResult.Preferences.Should().BeEquivalentTo(expectedResult.Preferences);
        }
    }

    [Test]
    public void Handle_InvokesGetMemberAPI()
    {
        using (new AssertionScope("Invokes GetMemberAPI & validates the Member properties"))
        {
            aanHubRestApiClientMock.Verify(x => x.GetMember(memberId, cancellationToken), Times.Once());
            actualResult.FullName.Should().Be(expectedResult.FullName);
            actualResult.FirstName.Should().Be(expectedResult.FirstName);
            actualResult.LastName.Should().Be(expectedResult.LastName);
            actualResult.OrganisationName.Should().Be(expectedResult.OrganisationName);
            actualResult.Email.Should().Be(expectedResult.Email);
            actualResult.IsRegionalChair.Should().Be(expectedResult.IsRegionalChair);
            actualResult.UserType.Should().Be(expectedResult.UserType);
            actualResult.RegionId.Should().Be(expectedResult.RegionId);
        }
    }

    [Test]
    public void Handle_InvokesGetRegionsAPI()
    {
        using (new AssertionScope("Invokes GetRegionsAPI & validates the RegionName property"))
        {
            aanHubRestApiClientMock.Verify(x => x.GetRegions(cancellationToken), Times.Once());
            actualResult.RegionName.Should().Be(expectedResult.RegionName);
        }
    }

    [Test]
    public void Handle_InvokesGetMyApprenticeshipQueryHandler()
    {
        using (new AssertionScope("Invokes GetMyApprenticeshipQueryHandler & validates the Apprenticeship properties"))
        {
            coursesApiClientMock.Verify(c => c.Get<GetStandardResponse>(It.IsAny<GetStandardQueryRequest>()), Times.Once);
            apprenticeAccountsApiClientMock.Verify(c => c.GetWithResponseCode<MyApprenticeshipResponse>(It.Is<GetMyApprenticeshipRequest>(r => r.Id == apprenticeId)), Times.Once);

            actualResult.Apprenticeship.Level.Should().Be(expectedResult.Apprenticeship.Level);
            actualResult.Apprenticeship.Sector.Should().Be(expectedResult.Apprenticeship.Sector);
            actualResult.Apprenticeship.Programmes.Should().Be(expectedResult.Apprenticeship.Programmes);
        }
    }

    [TearDown]
    public void Dispose()
    {
        aanHubRestApiClientMock = null!;
        apprenticeAccountsApiClientMock = null!;
        coursesApiClientMock = null!; ;

        myApprenticeshipQuery = null!;
        myApprenticeshipResponse = null!;
        standardResponse = null!;

        sut = null!;
        getMemberProfileWithPreferencesQuery = null!;

        expectedResult = null!;
        actualResult = null!;

        memberResult = null!;
        regionsResult = null!;
    }
}