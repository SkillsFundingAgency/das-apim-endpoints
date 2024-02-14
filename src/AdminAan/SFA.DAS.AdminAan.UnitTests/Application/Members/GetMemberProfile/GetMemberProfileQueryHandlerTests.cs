using System.Net;
using AutoFixture;
using FluentAssertions;
using FluentAssertions.Execution;
using Moq;
using RestEase;
using SFA.DAS.AdminAan.Application.Members.GetMemberProfile;
using SFA.DAS.AdminAan.Application.Regions.Queries.GetRegions;
using SFA.DAS.AdminAan.Domain;
using SFA.DAS.AdminAan.Domain.ApprenticeAccount;
using SFA.DAS.AdminAan.Domain.Courses;
using SFA.DAS.AdminAan.Infrastructure;

namespace SFA.DAS.AdminAan.UnitTests.Application.Members.GetMemberProfile;

public class GetMemberProfileQueryHandlerTests
{
    private Fixture _fixture = null!;
    private CancellationToken _cancellationToken;
    private GetMemberProfileQuery _query = null!;
    private readonly Guid _memberId = Guid.NewGuid();

    [SetUp]
    public void Init()
    {
        _fixture = new();
        _cancellationToken = _fixture.Create<CancellationToken>();
        _query = new(_memberId, _fixture.Create<Guid>());
    }


    [Test]
    public async Task GetMemberProfile_ReturnsMemberDetails()
    {
        var (sut, aanHubApiClientMock, commitmentsV2ApiClientMock, apprenticeAccountsApiClientMock, coursesApiClientMock) = SetupSut();
        var (getMemberResponse, _, _) = SetupGetMemberToReturnEmployerMember(aanHubApiClientMock, commitmentsV2ApiClientMock);
        SetupGetRegions(getMemberResponse.RegionId, aanHubApiClientMock);
        SetupProfilesAndPreferences(aanHubApiClientMock);
        SetupActivities(aanHubApiClientMock);

        GetMemberProfileQueryResult actual = await sut.Handle(_query, _cancellationToken);

        using (new AssertionScope())
        {
            aanHubApiClientMock.Verify(c => c.GetMember(getMemberResponse.MemberId, _cancellationToken), Times.Once);
            actual.FullName.Should().Be(getMemberResponse.FullName);
            actual.Email.Should().Be(getMemberResponse.Email);
            actual.FirstName.Should().Be(getMemberResponse.FirstName);
            actual.LastName.Should().Be(getMemberResponse.LastName);
            actual.OrganisationName.Should().Be(getMemberResponse.OrganisationName);
            actual.RegionId.Should().Be(getMemberResponse.RegionId);
            actual.UserType.Should().Be(getMemberResponse.UserType);
            actual.IsRegionalChair.Should().Be(getMemberResponse.IsRegionalChair.GetValueOrDefault());
        }
    }

    [Test]
    public async Task GetMemberProfile_ReturnsRegionName()
    {
        var (sut, aanHubApiClientMock, commitmentsV2ApiClientMock, apprenticeAccountsApiClientMock, coursesApiClientMock) = SetupSut();
        var (getMemberResponse, _, _) = SetupGetMemberToReturnEmployerMember(aanHubApiClientMock, commitmentsV2ApiClientMock);
        var expectedRegionName = SetupGetRegions(getMemberResponse.RegionId, aanHubApiClientMock);
        SetupProfilesAndPreferences(aanHubApiClientMock);
        SetupActivities(aanHubApiClientMock);

        GetMemberProfileQueryResult actual = await sut.Handle(_query, _cancellationToken);

        actual.RegionName.Should().Be(expectedRegionName);
        aanHubApiClientMock.Verify(c => c.GetRegions(_cancellationToken), Times.Once);
    }

    [Test]
    public async Task GetMemberProfile_DoesNotReturnRegionName()
    {
        var (sut, aanHubApiClientMock, commitmentsV2ApiClientMock, apprenticeAccountsApiClientMock, coursesApiClientMock) = SetupSut();
        var (getMemberResponse, _, _) = SetupGetMemberToReturnEmployerMember(aanHubApiClientMock, commitmentsV2ApiClientMock, false);
        var expectedRegionName = SetupGetRegions(getMemberResponse.RegionId, aanHubApiClientMock);
        SetupProfilesAndPreferences(aanHubApiClientMock);
        SetupActivities(aanHubApiClientMock);

        GetMemberProfileQueryResult actual = await sut.Handle(_query, _cancellationToken);

        actual.RegionName.Should().BeNull();
        aanHubApiClientMock.Verify(c => c.GetRegions(_cancellationToken), Times.Never);
    }

    [Test]
    public async Task GetMemberProfile_ReturnsProfileData()
    {
        var (sut, aanHubApiClientMock, commitmentsV2ApiClientMock, _, _) = SetupSut();
        var (getMemberResponse, _, _) = SetupGetMemberToReturnEmployerMember(aanHubApiClientMock, commitmentsV2ApiClientMock);
        SetupGetRegions(getMemberResponse.RegionId, aanHubApiClientMock);
        SetupActivities(aanHubApiClientMock);
        var expected = SetupProfilesAndPreferences(aanHubApiClientMock);

        GetMemberProfileQueryResult actual = await sut.Handle(_query, _cancellationToken);

        actual.Profiles.Should().BeEquivalentTo(expected.Profiles);
        aanHubApiClientMock.Verify(a => a.GetMemberProfileWithPreferences(_memberId, It.IsAny<Guid>(), _cancellationToken), Times.Once);
    }

    [Test]
    public async Task GetMemberProfile_ReturnsPreferencesData()
    {
        var (sut, aanHubApiClientMock, commitmentsV2ApiClientMock, _, _) = SetupSut();
        var (getMemberResponse, _, _) = SetupGetMemberToReturnEmployerMember(aanHubApiClientMock, commitmentsV2ApiClientMock);
        SetupGetRegions(getMemberResponse.RegionId, aanHubApiClientMock);
        SetupActivities(aanHubApiClientMock);
        var expected = SetupProfilesAndPreferences(aanHubApiClientMock);

        GetMemberProfileQueryResult actual = await sut.Handle(_query, _cancellationToken);

        actual.Preferences.Should().BeEquivalentTo(expected.Preferences);
        aanHubApiClientMock.Verify(a => a.GetMemberProfileWithPreferences(_memberId, It.IsAny<Guid>(), _cancellationToken), Times.Once);
    }

    [Test]
    public async Task GetMemberProfile_ReturnsApprenticeshipDetailsOfEmployer()
    {
        //Arrange
        var (sut, aanHubApiClientMock, commitmentsV2ApiClientMock, apprenticeAccountsApiClientMock, _) = SetupSut();
        var (getMemberResponse, accountSummaryResponse, apprenticeshipSummaryResponse) = SetupGetMemberToReturnEmployerMember(aanHubApiClientMock, commitmentsV2ApiClientMock);
        SetupGetRegions(getMemberResponse.RegionId, aanHubApiClientMock);
        SetupActivities(aanHubApiClientMock);
        var expected = SetupProfilesAndPreferences(aanHubApiClientMock);

        //Act
        GetMemberProfileQueryResult actual = await sut.Handle(_query, _cancellationToken);

        //Assert
        using (new AssertionScope())
        {
            commitmentsV2ApiClientMock.Verify(c => c.GetEmployerAccountSummary(getMemberResponse.Employer!.AccountId, _cancellationToken));
            commitmentsV2ApiClientMock.Verify(c => c.GetEmployerApprenticeshipsSummary(getMemberResponse.Employer!.AccountId, _cancellationToken));
            actual.Apprenticeship.Should().NotBeNull();
            actual.Apprenticeship!.Sectors.Should().BeEquivalentTo(apprenticeshipSummaryResponse.Sectors);
            actual.Apprenticeship!.ActiveApprenticesCount.Should().Be(accountSummaryResponse.ApprenticeshipStatusSummaryResponse.First().ActiveCount);
            actual.Apprenticeship!.Programme.Should().BeNull();
            actual.Apprenticeship!.Level.Should().BeNull();
            actual.Apprenticeship!.Sector.Should().BeNull();
            apprenticeAccountsApiClientMock.Verify(a => a.GetMyApprenticeship(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Never());
        }
    }

    [Test]
    public async Task GetMemberProfile_ReturnsApprenticeshipStandardDetailsOfApprentice()
    {
        //Arrange
        var (sut, aanHubApiClientMock, commitmentsV2ApiClientMock, apprenticeAccountsApiClientMock, coursesApiClientMock) = SetupSut();
        var (getMemberResponse, expectedStandard, _) = SetupGetMemberToReturnApprenticeMember(aanHubApiClientMock, apprenticeAccountsApiClientMock, coursesApiClientMock, true);
        SetupGetRegions(getMemberResponse.RegionId, aanHubApiClientMock);
        SetupActivities(aanHubApiClientMock);
        var expected = SetupProfilesAndPreferences(aanHubApiClientMock);

        //Act
        GetMemberProfileQueryResult actual = await sut.Handle(_query, _cancellationToken);

        //Assert
        using (new AssertionScope())
        {
            apprenticeAccountsApiClientMock.Verify(a => a.GetMyApprenticeship(It.IsAny<Guid>(), It.IsAny<CancellationToken>()));
            coursesApiClientMock.Verify(a => a.GetStandard(It.IsAny<string>(), _cancellationToken), Times.Once);
            actual.Apprenticeship.Should().NotBeNull();
            actual.Apprenticeship!.Programme.Should().Be(expectedStandard!.Title);
            actual.Apprenticeship!.Level.Should().Be(expectedStandard!.Level.ToString());
            actual.Apprenticeship!.Sector.Should().Be(expectedStandard!.Route);
            actual.Apprenticeship!.Sectors.Should().BeEmpty();
            actual.Apprenticeship!.ActiveApprenticesCount.Should().Be(0);
            coursesApiClientMock.Verify(a => a.GetFramework(It.IsAny<string>(), _cancellationToken), Times.Never);
            commitmentsV2ApiClientMock.Verify(c => c.GetEmployerAccountSummary(It.IsAny<long>(), It.IsAny<CancellationToken>()), Times.Never);
            commitmentsV2ApiClientMock.Verify(c => c.GetEmployerApprenticeshipsSummary(It.IsAny<long>(), It.IsAny<CancellationToken>()), Times.Never);
        }
    }

    [Test]
    public async Task GetMemberProfile_ReturnsApprenticeshipFrameworkDetailsOfApprentice()
    {
        //Arrange
        var (sut, aanHubApiClientMock, commitmentsV2ApiClientMock, apprenticeAccountsApiClientMock, coursesApiClientMock) = SetupSut();
        var (getMemberResponse, _, expectedFramework) = SetupGetMemberToReturnApprenticeMember(aanHubApiClientMock, apprenticeAccountsApiClientMock, coursesApiClientMock, false);
        SetupGetRegions(getMemberResponse.RegionId, aanHubApiClientMock);
        SetupActivities(aanHubApiClientMock);
        var expected = SetupProfilesAndPreferences(aanHubApiClientMock);

        //Act
        GetMemberProfileQueryResult actual = await sut.Handle(_query, _cancellationToken);

        //Assert
        using (new AssertionScope())
        {
            apprenticeAccountsApiClientMock.Verify(a => a.GetMyApprenticeship(It.IsAny<Guid>(), It.IsAny<CancellationToken>()));
            coursesApiClientMock.Verify(a => a.GetFramework(It.IsAny<string>(), _cancellationToken), Times.Once);
            actual.Apprenticeship.Should().NotBeNull();
            actual.Apprenticeship!.Programme.Should().Be(expectedFramework!.Title);
            actual.Apprenticeship!.Level.Should().Be(expectedFramework!.Level.ToString());
            actual.Apprenticeship!.Sector.Should().Be(expectedFramework!.FrameworkName);
            actual.Apprenticeship!.Sectors.Should().BeEmpty();
            actual.Apprenticeship!.ActiveApprenticesCount.Should().Be(0);
            coursesApiClientMock.Verify(a => a.GetStandard(It.IsAny<string>(), _cancellationToken), Times.Never);
            commitmentsV2ApiClientMock.Verify(c => c.GetEmployerAccountSummary(It.IsAny<long>(), It.IsAny<CancellationToken>()), Times.Never);
            commitmentsV2ApiClientMock.Verify(c => c.GetEmployerApprenticeshipsSummary(It.IsAny<long>(), It.IsAny<CancellationToken>()), Times.Never);
        }
    }

    [Test]
    public async Task GetMemberProfile_ShouldInvokeGetMemberActivities()
    {
        // Arrange
        var (sut, aanHubApiClientMock, commitmentsV2ApiClientMock, _, _) = SetupSut();
        var (getMemberResponse, _, _) = SetupGetMemberToReturnEmployerMember(aanHubApiClientMock, commitmentsV2ApiClientMock);
        SetupGetRegions(getMemberResponse.RegionId, aanHubApiClientMock);
        SetupProfilesAndPreferences(aanHubApiClientMock);
        var expected = SetupActivities(aanHubApiClientMock);

        // Act
        GetMemberProfileQueryResult actual = await sut.Handle(_query, _cancellationToken);

        // Assert
        aanHubApiClientMock.Verify(a => a.GetMemberActivities(_memberId, _cancellationToken), Times.Once);
    }

    [Test]
    public async Task GetMemberProfile_ReturnsExpectedActivitiesData()
    {
        // Arrange
        var (sut, aanHubApiClientMock, commitmentsV2ApiClientMock, _, _) = SetupSut();
        var (getMemberResponse, _, _) = SetupGetMemberToReturnEmployerMember(aanHubApiClientMock, commitmentsV2ApiClientMock);
        SetupGetRegions(getMemberResponse.RegionId, aanHubApiClientMock);
        SetupProfilesAndPreferences(aanHubApiClientMock);
        var expected = SetupActivities(aanHubApiClientMock);

        // Act
        GetMemberProfileQueryResult actual = await sut.Handle(_query, _cancellationToken);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(actual.Activities, Is.Not.Null);
            Assert.That(actual.Activities.LastSignedUpDate, Is.EqualTo(expected.LastSignedUpDate));
            Assert.That(actual.Activities.EventsAttended, Is.Not.Null);
            Assert.That(actual.Activities.EventsAttended.EventsDateRange, Is.EqualTo(expected.EventsAttended.EventsDateRange));
            Assert.That(actual.Activities.EventsAttended.Events, Is.EqualTo(expected.EventsAttended.Events));
            Assert.That(actual.Activities.EventsPlanned, Is.Not.Null);
            Assert.That(actual.Activities.EventsPlanned.EventsDateRange, Is.EqualTo(expected.EventsPlanned.EventsDateRange));
            Assert.That(actual.Activities.EventsPlanned.Events, Is.EqualTo(expected.EventsPlanned.Events));
        });
    }

    private static (GetMemberProfileQueryHandler, Mock<IAanHubRestApiClient>, Mock<ICommitmentsV2ApiClient>, Mock<IApprenticeAccountsApiClient>, Mock<ICoursesApiClient>) SetupSut()
    {
        Mock<IAanHubRestApiClient> annApiClientMock = new();
        Mock<ICommitmentsV2ApiClient> commitmentsV2ApiClientMock = new();
        Mock<IApprenticeAccountsApiClient> apprenticeAccountsApiClientMock = new();
        Mock<ICoursesApiClient> coursesApiClientMock = new();

        GetMemberProfileQueryHandler sut = new(annApiClientMock.Object, commitmentsV2ApiClientMock.Object, apprenticeAccountsApiClientMock.Object, coursesApiClientMock.Object);
        return (sut, annApiClientMock, commitmentsV2ApiClientMock, apprenticeAccountsApiClientMock, coursesApiClientMock);
    }

    private (GetMemberResponse, GetEmployerAccountSummaryResponse, GetEmployerApprenticeshipsSummaryResponse) SetupGetMemberToReturnEmployerMember(
        Mock<IAanHubRestApiClient> annApiClientMock,
        Mock<ICommitmentsV2ApiClient> commitmentsV2ApiClientMock,
        bool setRegionId = true)
    {
        GetMemberResponse expectedMemberDetails = _fixture
            .Build<GetMemberResponse>()
            .With(m => m.MemberId, _memberId)
            .With(m => m.RegionId, setRegionId ? _fixture.Create<int>() : null)
            .With(m => m.UserType, MemberUserType.Employer)
            .With(m => m.Employer, _fixture.Create<EmployerModel>())
            .Without(m => m.Apprentice)
            .Create();
        annApiClientMock.Setup(a => a.GetMember(_memberId, _cancellationToken)).ReturnsAsync(expectedMemberDetails);

        var accountSummaryResponse = _fixture.Create<GetEmployerAccountSummaryResponse>();
        commitmentsV2ApiClientMock.Setup(c => c.GetEmployerAccountSummary(expectedMemberDetails.Employer!.AccountId, _cancellationToken)).ReturnsAsync(accountSummaryResponse);
        var apprenticeshipSummaryResponse = _fixture.Create<GetEmployerApprenticeshipsSummaryResponse>();
        commitmentsV2ApiClientMock.Setup(c => c.GetEmployerApprenticeshipsSummary(expectedMemberDetails.Employer!.AccountId, _cancellationToken)).ReturnsAsync(apprenticeshipSummaryResponse);
        return (expectedMemberDetails, accountSummaryResponse, apprenticeshipSummaryResponse);
    }

    private (GetMemberResponse, GetStandardResponse?, GetFrameworkResponse?) SetupGetMemberToReturnApprenticeMember(
       Mock<IAanHubRestApiClient> annApiClientMock,
       Mock<IApprenticeAccountsApiClient> apprenticeAccountsApiClientMock,
       Mock<ICoursesApiClient> coursesApiClientMock,
       bool setStandard = true)
    {
        GetStandardResponse? standardResponse = null;
        GetFrameworkResponse? frameworkResponse = null;

        GetMemberResponse expectedMemberDetails = _fixture
            .Build<GetMemberResponse>()
            .With(m => m.MemberId, _memberId)
            .With(m => m.RegionId, _fixture.Create<int>())
            .With(m => m.UserType, MemberUserType.Apprentice)
            .With(m => m.Apprentice, _fixture.Create<ApprenticeModel>())
            .Without(m => m.Employer)
            .Create();
        annApiClientMock.Setup(a => a.GetMember(_memberId, _cancellationToken)).ReturnsAsync(expectedMemberDetails);

        var myApprenticeshipResponse = _fixture
            .Build<GetMyApprenticeshipResponse>()
            .With(a => a.StandardUId, setStandard ? _fixture.Create<string>() : null)
            .With(a => a.TrainingCode, _fixture.Create<string>())
            .Create();
        apprenticeAccountsApiClientMock.Setup(a => a.GetMyApprenticeship(expectedMemberDetails.Apprentice!.ApprenticeId, _cancellationToken)).ReturnsAsync(GetOkResponse(myApprenticeshipResponse));

        if (setStandard)
        {
            standardResponse = _fixture.Create<GetStandardResponse>();
            coursesApiClientMock.Setup(c => c.GetStandard(myApprenticeshipResponse.StandardUId!, _cancellationToken)).ReturnsAsync(GetOkResponse(standardResponse));
        }
        else
        {
            frameworkResponse = _fixture.Create<GetFrameworkResponse>();
            coursesApiClientMock.Setup(c => c.GetFramework(myApprenticeshipResponse.TrainingCode!, _cancellationToken)).ReturnsAsync(GetOkResponse(frameworkResponse));
        }

        return (expectedMemberDetails, standardResponse, frameworkResponse);
    }

    private static Response<T> GetOkResponse<T>(T result) => new(string.Empty, new(HttpStatusCode.OK), () => result);

    private string? SetupGetRegions(int? regionId, Mock<IAanHubRestApiClient> aanHubApiClientMock)
    {
        string? expectedRegionName = null;
        var regionsResponse = _fixture.Create<GetRegionsQueryResult>();
        var region = regionsResponse.Regions.FirstOrDefault(r => r.Id == regionId.GetValueOrDefault());
        if (region == null)
        {
            expectedRegionName = _fixture.Create<string>();
            regionsResponse.Regions.Add(new Region(regionId.GetValueOrDefault(), expectedRegionName, _fixture.Create<int>()));
        }
        else
        {
            expectedRegionName = region.Area;
        }
        aanHubApiClientMock.Setup(a => a.GetRegions(_cancellationToken)).ReturnsAsync(regionsResponse);
        return expectedRegionName;
    }

    private GetMemberProfilesAndPreferencesResponse SetupProfilesAndPreferences(Mock<IAanHubRestApiClient> aanHubApiClientMock)
    {
        var expectedProfilesAndPreferences = _fixture.Create<GetMemberProfilesAndPreferencesResponse>();
        aanHubApiClientMock.Setup(a => a.GetMemberProfileWithPreferences(_memberId, It.IsAny<Guid>(), _cancellationToken)).ReturnsAsync(expectedProfilesAndPreferences);
        return expectedProfilesAndPreferences;
    }

    private GetMemberActivitiesResponse SetupActivities(Mock<IAanHubRestApiClient> aanHubApiClientMock)
    {
        var expectedActivities = _fixture.Create<GetMemberActivitiesResponse>();
        aanHubApiClientMock.Setup(a => a.GetMemberActivities(_memberId, _cancellationToken)).ReturnsAsync(expectedActivities);
        return expectedActivities;
    }
}
