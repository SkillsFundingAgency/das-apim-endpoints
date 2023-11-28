using FluentAssertions;
using FluentAssertions.Execution;
using Moq;
using SFA.DAS.ApprenticeAan.Application.Common;
using SFA.DAS.ApprenticeAan.Application.Infrastructure;
using SFA.DAS.ApprenticeAan.Application.MemberProfiles.Queries.GetMemberProfileWithPreferences;
using SFA.DAS.ApprenticeAan.Application.Members.Queries.GetMember;
using SFA.DAS.ApprenticeAan.Application.Model;
using SFA.DAS.ApprenticeAan.Application.Regions.Queries.GetRegions;

namespace SFA.DAS.ApprenticeAan.Application.UnitTests.MemberProfiles.Queries.GetMemberProfileWithPreferences;

public class GetMemberProfileWithPreferencesQueryHandlerTests
{
    readonly Guid memberId = Guid.NewGuid();

    readonly string firstName = "Last";
    readonly string lastName = "First";
    readonly string organisationName = "organisation";
    readonly string fullName = "Full Name";
    readonly string email = "My_Email@domain.com";
    readonly int regionId = 1;
    readonly string regionName = "London";
    readonly MemberUserType userType = MemberUserType.Apprentice;
    readonly bool isRegionalChair = true;

    Mock<IAanHubRestApiClient> aanHubRestApiClientMock;

    GetMemberProfileWithPreferencesQueryHandler sut = null!;
    GetMemberProfileWithPreferencesQuery getMemberProfileWithPreferencesQuery = null!;

    GetMemberProfileWithPreferencesQueryResult expectedResult;
    GetMemberProfileWithPreferencesQueryResult actualResult = null!;

    GetMemberQueryResult memberResult = null!;
    GetRegionsQueryResult regionsResult = null!;

    CancellationToken cancellationToken = new CancellationToken();

    [SetUp]
    public void InitAsync()
    {
        getMemberProfileWithPreferencesQuery = new(memberId, memberId, true);

        aanHubRestApiClientMock = new();

        regionsResult = new() { Regions = new List<Region> { new Region { Id = regionId, Area = regionName, Ordering = 1 } } };
        memberResult = new()
        {
            MemberId = memberId,
            Email = email,
            FullName = fullName,
            FirstName = firstName,
            LastName = lastName,
            OrganisationName = organisationName,
            IsRegionalChair = isRegionalChair,
            UserType = userType.ToString(),
            RegionId = regionId,
            Apprentice = new ApprenticeModel(Guid.NewGuid()),
            Employer = new EmployerModel(0, Guid.NewGuid()),
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
            IsRegionalChair = isRegionalChair
        };

        expectedResult.Profiles = new[] {
                                            new MemberProfile { ProfileId=1,Value = Guid.NewGuid().ToString(), PreferenceId=1 },
                                            new MemberProfile { ProfileId=2,Value = Guid.NewGuid().ToString(), PreferenceId=2 }
                                        };

        expectedResult.Preferences = new[] {
                                                new MemberPreference { PreferenceId=1, Value = true    },
                                                new MemberPreference { PreferenceId=2, Value = false   }
                                            };
        aanHubRestApiClientMock.Setup(x => x.GetMember(memberId, cancellationToken)).ReturnsAsync(memberResult);
        aanHubRestApiClientMock.Setup(x => x.GetMemberProfileWithPreferences(getMemberProfileWithPreferencesQuery.MemberId, getMemberProfileWithPreferencesQuery.MemberId, It.IsAny<bool>(), cancellationToken)).ReturnsAsync(expectedResult);
        aanHubRestApiClientMock.Setup(x => x.GetRegions(cancellationToken)).ReturnsAsync(regionsResult);
    }

    private async Task InvokeHandler()
    {
        actualResult = await sut.Handle(getMemberProfileWithPreferencesQuery, cancellationToken);
    }

    [TestCase(MemberUserType.Apprentice)]
    [TestCase(MemberUserType.Employer)]
    public async Task Handle_InvokesGetMemberProfileWithPreferencesAPI(MemberUserType memberUserType)
    {
        //Arrange
        memberResult.UserType = memberUserType.ToString();
        aanHubRestApiClientMock.Setup(x => x.GetMember(memberId, cancellationToken)).ReturnsAsync(memberResult);

        //Act
        sut = new GetMemberProfileWithPreferencesQueryHandler(aanHubRestApiClientMock.Object);
        await InvokeHandler();

        //Assert
        using (new AssertionScope("Invokes GetMemberProfileWithPreferencesAPI & validates the Profile and PreferenceIds"))
        {
            aanHubRestApiClientMock.Verify(x => x.GetMemberProfileWithPreferences(memberId, memberId, true, cancellationToken), Times.Once());
            actualResult.Profiles.Should().BeEquivalentTo(expectedResult.Profiles);
            actualResult.Preferences.Should().BeEquivalentTo(expectedResult.Preferences);
        }
    }

    [TestCase(MemberUserType.Apprentice)]
    [TestCase(MemberUserType.Employer)]
    public async Task Handle_InvokesGetMemberAPI(MemberUserType memberUserType)
    {
        //Arrange
        memberResult.UserType = memberUserType.ToString();
        expectedResult.UserType = memberUserType;
        aanHubRestApiClientMock.Setup(x => x.GetMember(memberId, cancellationToken)).ReturnsAsync(memberResult);
        aanHubRestApiClientMock.Setup(x => x.GetMemberProfileWithPreferences(getMemberProfileWithPreferencesQuery.MemberId, getMemberProfileWithPreferencesQuery.MemberId, It.IsAny<bool>(), cancellationToken)).ReturnsAsync(expectedResult);

        //Act
        sut = new GetMemberProfileWithPreferencesQueryHandler(aanHubRestApiClientMock.Object);
        await InvokeHandler();

        //Assert
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

    [TestCase(MemberUserType.Apprentice)]
    [TestCase(MemberUserType.Employer)]
    public async Task Handle_InvokesGetRegionsAPI(MemberUserType memberUserType)
    {
        //Arrange
        memberResult.UserType = memberUserType.ToString();
        aanHubRestApiClientMock.Setup(x => x.GetMember(memberId, cancellationToken)).ReturnsAsync(memberResult);

        //Act
        sut = new GetMemberProfileWithPreferencesQueryHandler(aanHubRestApiClientMock.Object);
        await InvokeHandler();

        //Assert
        using (new AssertionScope("Invokes GetRegionsAPI & validates the RegionName property"))
        {
            aanHubRestApiClientMock.Verify(x => x.GetRegions(cancellationToken), Times.Once());
            actualResult.RegionName.Should().Be(expectedResult.RegionName);
        }
    }

    [TearDown]
    public void Dispose()
    {
        aanHubRestApiClientMock = null!;

        sut = null!;
        getMemberProfileWithPreferencesQuery = null!;

        expectedResult = null!;
        actualResult = null!;

        memberResult = null!;
        regionsResult = null!;
    }
}