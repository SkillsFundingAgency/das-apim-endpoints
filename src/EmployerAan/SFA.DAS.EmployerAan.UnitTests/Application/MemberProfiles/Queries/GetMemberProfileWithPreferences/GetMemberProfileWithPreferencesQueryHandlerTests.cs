using FluentAssertions;
using FluentAssertions.Execution;
using Moq;
using SFA.DAS.EmployerAan.Application.Employer.Queries.GetEmployerMemberSummary;
using SFA.DAS.EmployerAan.Application.MemberProfiles.Queries.GetMemberProfileWithPreferences;
using SFA.DAS.EmployerAan.Application.Members.Queries.GetMember;
using SFA.DAS.EmployerAan.Application.Regions.Queries.GetRegions;
using SFA.DAS.EmployerAan.Common;
using SFA.DAS.EmployerAan.Infrastructure;
using SFA.DAS.EmployerAan.Models;

namespace SFA.DAS.EmployerAan.UnitTests.Application.MemberProfiles.Queries.GetMemberProfileWithPreferences;

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

    readonly int accountId = 1;
    readonly int activeApprenticesCount = 10;
    readonly List<DateTime> startDates = new() { DateTime.Now, DateTime.Now.AddDays(-1) };
    readonly List<string> sectors = new List<string>() { Guid.NewGuid().ToString(), Guid.NewGuid().ToString() };

    Mock<IAanHubRestApiClient> aanHubRestApiClientMock;
    Mock<ICommitmentsV2ApiClient> commitmentsV2ApiClientMock;

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
        commitmentsV2ApiClientMock = new();

        regionsResult = new(new List<Region> { new Region(regionId, regionName, 1) });

        memberResult = new()
        {
            MemberId = memberId,
            Employer = new EmployerModel(accountId, Guid.NewGuid()),
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
            Apprenticeship = new Apprenticeship { ActiveApprenticesCount = activeApprenticesCount, Sectors = sectors }
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

        List<ApprenticeshipStatusSummaryResponse> apprenticeshipStatusSummaryResponse = new() { new ApprenticeshipStatusSummaryResponse { ActiveCount = activeApprenticesCount } };
        AccountsSummary accountsSummary = new AccountsSummary() { ApprenticeshipStatusSummaryResponse = apprenticeshipStatusSummaryResponse };

        ApprenticeshipsFilterValues expectedApprenticeshipsFilterValues = new()
        {
            StartDates = startDates,
            Sectors = sectors
        };

        commitmentsV2ApiClientMock.Setup(c => c.GetEmployerAccountSummary(memberResult.Employer.AccountId, cancellationToken)).ReturnsAsync(accountsSummary);
        commitmentsV2ApiClientMock.Setup(x => x.GetApprenticeshipsSummaryForEmployer(memberResult.Employer.AccountId, cancellationToken)).ReturnsAsync(expectedApprenticeshipsFilterValues);


        sut = new GetMemberProfileWithPreferencesQueryHandler(aanHubRestApiClientMock.Object, commitmentsV2ApiClientMock.Object);

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
    public void Handle_InvokesGetEmployerMemberSummaryQueryHandler()
    {
        using (new AssertionScope("Invokes GetMyApprenticeshipQueryHandler & validates the Apprenticeship properties"))
        {
            commitmentsV2ApiClientMock.Verify(c => c.GetEmployerAccountSummary(memberResult!.Employer!.AccountId, cancellationToken), Times.Once);
            commitmentsV2ApiClientMock.Verify(c => c.GetApprenticeshipsSummaryForEmployer(memberResult!.Employer!.AccountId, cancellationToken), Times.Once);

            actualResult.Apprenticeship.ActiveApprenticesCount.Should().Be(expectedResult.Apprenticeship.ActiveApprenticesCount);
            actualResult.Apprenticeship.Sectors.Should().BeEquivalentTo(expectedResult.Apprenticeship.Sectors);

        }
    }

    [TearDown]
    public void Dispose()
    {
        aanHubRestApiClientMock = null!;
        commitmentsV2ApiClientMock = null!;

        sut = null!;
        getMemberProfileWithPreferencesQuery = null!;

        expectedResult = null!;
        actualResult = null!;

        memberResult = null!;
        regionsResult = null!;
    }
}