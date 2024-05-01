using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.Apprenticeships.Application.Apprenticeship;
using SFA.DAS.Apprenticeships.InnerApi;
using SFA.DAS.Apprenticeships.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.Apprenticeships;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Apprenticeships;
using SFA.DAS.SharedOuterApi.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.CollectionCalendar;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.CollectionCalendar;

namespace SFA.DAS.Apprenticeships.UnitTests.Application.Apprenticeship;

public class WhenGettingApprenticeshipStartDate
{
	private readonly Mock<ILogger<GetApprenticeshipStartDateQueryHandler>> _mocklogger;
    private readonly Mock<IApprenticeshipsApiClient<ApprenticeshipsApiConfiguration>> _mockApprenticeshipsApiClient;
    private readonly Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>> _mockCommitmentsV2ApiApiClient;
    private readonly Mock<ICollectionCalendarApiClient<CollectionCalendarApiConfiguration>> _mockCollectionCalendarApiClient;
    private readonly Fixture _fixture;

    GetApprenticeshipStartDateQueryHandler _sut = null!;

    ApprenticeshipStartDateResponse _expectedResponse = null!;
    DateTime _expectedEarliestStartDate;
    DateTime _expectedLatestStartDate;
    DateTime _expectedLastFridayOfSchool;
    DateTime _dateOfBirth;
    DateTime _effectiveFrom;
    DateTime _effectiveTo;
    private GetApprenticeshipStartDateResponse _expectedInnerApiResponse = null!;

    public WhenGettingApprenticeshipStartDate()
    {
		_mocklogger = new Mock<ILogger<GetApprenticeshipStartDateQueryHandler>>();
        _mockApprenticeshipsApiClient = new Mock<IApprenticeshipsApiClient<ApprenticeshipsApiConfiguration>>();
        _mockCommitmentsV2ApiApiClient = new Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>>();
        _mockCollectionCalendarApiClient = new Mock<ICollectionCalendarApiClient<CollectionCalendarApiConfiguration>>();
        _fixture = new Fixture();
    }

    [SetUp]
    public async Task Setup()
    {
        // Arrange
        _expectedResponse = _fixture.Create<ApprenticeshipStartDateResponse>();
        _expectedEarliestStartDate = _fixture.Create<DateTime>();
        _expectedLatestStartDate = _fixture.Create<DateTime>();
        _expectedLastFridayOfSchool = new DateTime(2021, 6, 25);
        _dateOfBirth = new DateTime(2005, 3, 3);
        _effectiveFrom = _fixture.Create<DateTime>();
        _effectiveTo = _effectiveFrom.Add(_fixture.Create<TimeSpan>());

        _expectedResponse.Standard = new StandardInfo
        {
            CourseCode = 456.ToString(),
            EffectiveFrom = _effectiveFrom,
            EffectiveTo = _effectiveTo,
            StandardVersion = new StandardVersionInfo
            {
                VersionEarliestStartDate = _effectiveFrom,
                VersionLatestStartDate = _effectiveTo,
                Version = "1.2"
            }
        };

        _expectedInnerApiResponse = new GetApprenticeshipStartDateResponse
        {
            AccountLegalEntityId = 1,
            ApprenticeshipKey = _expectedResponse.ApprenticeshipKey,
            ActualStartDate = _expectedResponse.ActualStartDate,
            PlannedEndDate = _expectedResponse.PlannedEndDate,
            UKPRN = 123,
            ApprenticeDateOfBirth = _dateOfBirth,
            CourseCode = 456.ToString(),
            CourseVersion = "1.2"
        };

        _mockApprenticeshipsApiClient.Setup(x => x.Get<GetApprenticeshipStartDateResponse>(It.IsAny<GetApprenticeshipStartDateRequest>()))
        .ReturnsAsync(_expectedInnerApiResponse);

        _mockCommitmentsV2ApiApiClient.Setup(x => x.Get<GetAccountLegalEntityResponse>(It.IsAny<GetAccountLegalEntityRequest>()))
            .ReturnsAsync(new GetAccountLegalEntityResponse
            {
                LegalEntityName = _expectedResponse.EmployerName!
            });

        _mockCommitmentsV2ApiApiClient.Setup(x => x.Get<GetProviderResponse>(It.IsAny<GetProviderRequest>()))
            .ReturnsAsync(new GetProviderResponse
            {
                Name = _expectedResponse.ProviderName!
            });

        _mockCommitmentsV2ApiApiClient.Setup(x => x.Get<GetTrainingProgrammeVersionsResponse>(It.IsAny<GetTrainingProgrammeVersionsRequest>()))
            .ReturnsAsync(new GetTrainingProgrammeVersionsResponse
            {
                TrainingProgrammeVersions = new List<TrainingProgramme>
                {
                    new()
                    {
                        CourseCode = _expectedResponse.Standard.CourseCode,
                        EffectiveFrom = _expectedResponse.Standard.EffectiveFrom,
                        EffectiveTo = _expectedResponse.Standard.EffectiveTo,
                        Version = _expectedResponse.Standard.StandardVersion.Version
                    }
                }
            });

        _mockCollectionCalendarApiClient
            .Setup(x => x.Get<GetAcademicYearsResponse>(It.Is<GetAcademicYearsRequest>(y =>
                y._dateTime == _expectedResponse.ActualStartDate.Value.ToString("yyyy-MM-dd"))))
            .ReturnsAsync(new GetAcademicYearsResponse { StartDate = _expectedEarliestStartDate });

        _mockCollectionCalendarApiClient
            .Setup(x => x.Get<GetAcademicYearsResponse>(It.Is<GetAcademicYearsRequest>(y =>
                y._dateTime == _expectedResponse.ActualStartDate.Value.AddYears(1).ToString("yyyy-MM-dd"))))
            .ReturnsAsync(new GetAcademicYearsResponse { EndDate = _expectedLatestStartDate });

        _sut = new GetApprenticeshipStartDateQueryHandler(_mocklogger.Object, _mockApprenticeshipsApiClient.Object, _mockCommitmentsV2ApiApiClient.Object, _mockCollectionCalendarApiClient.Object);
    }

    [Test]
	public async Task Then_Gets_ApprenticeshipStartDate_From_ApiClient()
	{
        // Act
        var result = await _sut.Handle(new GetApprenticeshipStartDateQuery( Guid.NewGuid()), CancellationToken.None);

        // Assert
		result.EmployerName.Should().Be(_expectedResponse.EmployerName);
        result.ApprenticeshipKey.Should().Be(_expectedResponse.ApprenticeshipKey);
        result.ActualStartDate.Should().Be(_expectedResponse.ActualStartDate);
        result.PlannedEndDate.Should().Be(_expectedResponse.PlannedEndDate);
        result.ProviderName.Should().Be(_expectedResponse.ProviderName);
        result.EarliestStartDate.Should().Be(_expectedEarliestStartDate);
        result.LatestStartDate.Should().Be(_expectedLatestStartDate);
        result.LastFridayOfSchool.Should().Be(_expectedLastFridayOfSchool);
    }

    [TestCase(true, true)]
    [TestCase(true, false)]
    [TestCase(false, true)]
    [TestCase(false, false)]
    public async Task Then_BuildsStandardInfoCorrectly(bool multipleVersions, bool openEndedDates)
    {
        //  Arrange
        var firstVersionDate = _fixture.Create<DateTime>();
        var secondVersionDate = firstVersionDate.Add(_fixture.Create<TimeSpan>());
        var thirdVersionDate = secondVersionDate.Add(_fixture.Create<TimeSpan>());
        var fourthVersionDate = thirdVersionDate.Add(_fixture.Create<TimeSpan>());

        _expectedResponse.Standard.StandardVersion.VersionEarliestStartDate = secondVersionDate;
        _expectedResponse.Standard.StandardVersion.VersionLatestStartDate = thirdVersionDate;

        var expectedTrainingProgrammeVersions = BuildTrainingProgrammes(multipleVersions, openEndedDates, firstVersionDate, secondVersionDate, thirdVersionDate, fourthVersionDate);

        _mockCommitmentsV2ApiApiClient.Setup(x => x.Get<GetTrainingProgrammeVersionsResponse>(It.IsAny<GetTrainingProgrammeVersionsRequest>()))
            .ReturnsAsync(new GetTrainingProgrammeVersionsResponse{ TrainingProgrammeVersions = expectedTrainingProgrammeVersions });

        //  Act
        var result = await _sut.Handle(new GetApprenticeshipStartDateQuery(Guid.NewGuid()), CancellationToken.None);

        //  Assert
        result.Standard.EffectiveFrom.Should().Be(openEndedDates ? null : _effectiveFrom);
        result.Standard.EffectiveTo.Should().Be(openEndedDates ? null : _effectiveTo);
        result.Standard.StandardVersion.VersionEarliestStartDate.Should().Be(openEndedDates ? null : secondVersionDate);
        result.Standard.StandardVersion.VersionLatestStartDate.Should().Be(openEndedDates ? null : thirdVersionDate);
    }

    private List<TrainingProgramme> BuildTrainingProgrammes(bool multipleVersions, bool openEndedDates, DateTime firstVersionDate, DateTime secondVersionDate, DateTime thirdVersionDate, DateTime fourthVersionDate)
    {
        var expectedTrainingProgrammeVersions = new List<TrainingProgramme>
        {
            new()
            {
                CourseCode = _expectedResponse.Standard.CourseCode,
                EffectiveFrom = _expectedResponse.Standard.EffectiveFrom,
                EffectiveTo = _expectedResponse.Standard.EffectiveTo,
                Version = _expectedResponse.Standard.StandardVersion.Version,
                VersionEarliestStartDate = _expectedResponse.Standard.StandardVersion.VersionEarliestStartDate,
                VersionLatestStartDate = _expectedResponse.Standard.StandardVersion.VersionLatestStartDate
            }
        };

        if (multipleVersions)
        {
            expectedTrainingProgrammeVersions.Add(new TrainingProgramme
            {
                EffectiveFrom = _effectiveFrom,
                EffectiveTo = _effectiveTo,
                CourseCode = _expectedResponse.Standard.CourseCode,
                VersionEarliestStartDate = firstVersionDate,
                VersionLatestStartDate = secondVersionDate,
                Version = "1.1"
            });
            expectedTrainingProgrammeVersions.Add(new TrainingProgramme
            {
                EffectiveFrom = _effectiveFrom,
                EffectiveTo = _effectiveTo,
                CourseCode = _expectedResponse.Standard.CourseCode,
                VersionEarliestStartDate = thirdVersionDate,
                VersionLatestStartDate = fourthVersionDate,
                Version = "1.3"
            });
        }

        if (openEndedDates)
        {
            expectedTrainingProgrammeVersions.Single(x => x.Version == "1.2").VersionEarliestStartDate = null;
            expectedTrainingProgrammeVersions.Single(x => x.Version == "1.2").VersionLatestStartDate = null;
            expectedTrainingProgrammeVersions.Single(x => x.Version == "1.2").EffectiveFrom = null;
            expectedTrainingProgrammeVersions.Single(x => x.Version == "1.2").EffectiveTo = null;
        }

        return expectedTrainingProgrammeVersions;
    }
}

