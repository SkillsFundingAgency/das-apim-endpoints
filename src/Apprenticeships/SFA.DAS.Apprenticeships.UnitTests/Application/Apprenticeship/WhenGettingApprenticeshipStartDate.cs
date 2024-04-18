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

    public WhenGettingApprenticeshipStartDate()
    {
		_mocklogger = new Mock<ILogger<GetApprenticeshipStartDateQueryHandler>>();
        _mockApprenticeshipsApiClient = new Mock<IApprenticeshipsApiClient<ApprenticeshipsApiConfiguration>>();
        _mockCommitmentsV2ApiApiClient = new Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>>();
        _mockCollectionCalendarApiClient = new Mock<ICollectionCalendarApiClient<CollectionCalendarApiConfiguration>>();
        _fixture = new Fixture();
    }

    [Test]
	public async Task Then_Gets_ApprenticeshipStartDate_From_ApiClient()
	{
        //  Arrange
        var expectedResponse = _fixture.Create<ApprenticeshipStartDateResponse>();
        var expectedEarliestStartDate = _fixture.Create<DateTime>();
        var expectedLatestStartDate = _fixture.Create<DateTime>();
        var expectedLastFridayOfSchool = new DateTime(2021, 6, 25);
        var dateOfBirth = new DateTime(2005, 3, 3);
        var effectiveFrom = _fixture.Create<DateTime>();
        var effectiveTo = _fixture.Create<DateTime>();

        expectedResponse.Standard = new StandardInfo
        {
            CourseCode = 456.ToString(),
            EffectiveFrom = effectiveFrom,
            EffectiveTo = effectiveTo,
            StandardVersion = new StandardVersionInfo
            {
                EffectiveFrom = effectiveFrom, EffectiveTo = effectiveTo, Version = "1.1" 
            }
        };

        _mockApprenticeshipsApiClient.Setup(x => x.Get<GetApprenticeshipStartDateResponse>(It.IsAny<GetApprenticeshipStartDateRequest>()))
        .ReturnsAsync(new GetApprenticeshipStartDateResponse
			{
				AccountLegalEntityId = 1,
				ApprenticeshipKey = expectedResponse.ApprenticeshipKey,
				ActualStartDate = expectedResponse.ActualStartDate,
				PlannedEndDate = expectedResponse.PlannedEndDate,
				UKPRN = 123,
				ApprenticeDateOfBirth = dateOfBirth,
				CourseCode = 456.ToString(),
                CourseVersion = "1.1"
			});

		_mockCommitmentsV2ApiApiClient.Setup(x => x.Get<GetAccountLegalEntityResponse>(It.IsAny<GetAccountLegalEntityRequest>()))
			.ReturnsAsync(new GetAccountLegalEntityResponse
			{
				LegalEntityName = expectedResponse.EmployerName!
			});

        _mockCommitmentsV2ApiApiClient.Setup(x => x.Get<GetProviderResponse>(It.IsAny<GetProviderRequest>()))
            .ReturnsAsync(new GetProviderResponse
            {
                Name = expectedResponse.ProviderName!
            });

        _mockCommitmentsV2ApiApiClient.Setup(x => x.Get<GetTrainingProgrammeVersionsResponse>(It.IsAny<GetTrainingProgrammeVersionsRequest>()))
            .ReturnsAsync(new GetTrainingProgrammeVersionsResponse
            {
                TrainingProgrammeVersions = new List<TrainingProgramme>
                {
					new()
                    {
                        CourseCode = expectedResponse.Standard.CourseCode,
                        EffectiveFrom = expectedResponse.Standard.EffectiveFrom,
						EffectiveTo = expectedResponse.Standard.EffectiveTo,
						Version = expectedResponse.Standard.StandardVersion.Version
                    }
                }
            });

        _mockCollectionCalendarApiClient
            .Setup(x => x.Get<GetAcademicYearsResponse>(It.Is<GetAcademicYearsRequest>(y =>
                y._dateTime == expectedResponse.ActualStartDate.Value.ToString("yyyy-MM-dd"))))
            .ReturnsAsync(new GetAcademicYearsResponse { StartDate = expectedEarliestStartDate });

        _mockCollectionCalendarApiClient
            .Setup(x => x.Get<GetAcademicYearsResponse>(It.Is<GetAcademicYearsRequest>(y =>
                y._dateTime == expectedResponse.ActualStartDate.Value.AddYears(1).ToString("yyyy-MM-dd"))))
            .ReturnsAsync(new GetAcademicYearsResponse { EndDate = expectedLatestStartDate });

        var handler = new GetApprenticeshipStartDateQueryHandler(_mocklogger.Object, _mockApprenticeshipsApiClient.Object, _mockCommitmentsV2ApiApiClient.Object, _mockCollectionCalendarApiClient.Object);

		//  Act
		var result = await handler.Handle(new GetApprenticeshipStartDateQuery( Guid.NewGuid()), CancellationToken.None);

		//  Assert
		result.EmployerName.Should().Be(expectedResponse.EmployerName);
        result.ApprenticeshipKey.Should().Be(expectedResponse.ApprenticeshipKey);
        result.ActualStartDate.Should().Be(expectedResponse.ActualStartDate);
        result.PlannedEndDate.Should().Be(expectedResponse.PlannedEndDate);
        result.ProviderName.Should().Be(expectedResponse.ProviderName);
        result.EarliestStartDate.Should().Be(expectedEarliestStartDate);
        result.LatestStartDate.Should().Be(expectedLatestStartDate);
        result.LastFridayOfSchool.Should().Be(expectedLastFridayOfSchool);
    }

    [TestCase(true, true)]
    [TestCase(true, false)]
    [TestCase(false, true)]
    [TestCase(false, false)]
    public async Task Then_BuildsStandardInfoCorrectly(bool multipleVersions, bool openEndedDates)
    {
        //  Arrange
        var expectedResponse = _fixture.Create<ApprenticeshipStartDateResponse>();
        var expectedEarliestStartDate = _fixture.Create<DateTime>();
        var expectedLatestStartDate = _fixture.Create<DateTime>();
        var dateOfBirth = new DateTime(2005, 3, 3);
        var effectiveFrom = _fixture.Create<DateTime>();
        var effectiveTo = effectiveFrom.Add(_fixture.Create<TimeSpan>());

        var expectedStandardEffectiveFrom = (DateTime?)effectiveFrom;
        var expectedStandardEffectiveTo = (DateTime?)effectiveTo;

        expectedResponse.Standard = new StandardInfo
        {
            CourseCode = 456.ToString(),
            EffectiveFrom = effectiveFrom,
            EffectiveTo = effectiveTo,
            StandardVersion = new StandardVersionInfo
            {
                EffectiveFrom = effectiveFrom,
                EffectiveTo = effectiveTo,
                Version = "1.1"
            }
        };

        _mockApprenticeshipsApiClient.Setup(x => x.Get<GetApprenticeshipStartDateResponse>(It.IsAny<GetApprenticeshipStartDateRequest>()))
        .ReturnsAsync(new GetApprenticeshipStartDateResponse
        {
            AccountLegalEntityId = 1,
            ApprenticeshipKey = expectedResponse.ApprenticeshipKey,
            ActualStartDate = expectedResponse.ActualStartDate,
            PlannedEndDate = expectedResponse.PlannedEndDate,
            UKPRN = 123,
            ApprenticeDateOfBirth = dateOfBirth,
            CourseCode = 456.ToString(),
            CourseVersion = "1.1"
        });

        _mockCommitmentsV2ApiApiClient.Setup(x => x.Get<GetAccountLegalEntityResponse>(It.IsAny<GetAccountLegalEntityRequest>()))
            .ReturnsAsync(new GetAccountLegalEntityResponse
            {
                LegalEntityName = expectedResponse.EmployerName!
            });

        _mockCommitmentsV2ApiApiClient.Setup(x => x.Get<GetProviderResponse>(It.IsAny<GetProviderRequest>()))
            .ReturnsAsync(new GetProviderResponse
            {
                Name = expectedResponse.ProviderName!
            });

        var expectedTrainingProgrammeVersions = new List<TrainingProgramme>
        {
            new()
            {
                CourseCode = expectedResponse.Standard.CourseCode,
                EffectiveFrom = expectedResponse.Standard.EffectiveFrom,
                EffectiveTo = expectedResponse.Standard.EffectiveTo,
                Version = expectedResponse.Standard.StandardVersion.Version
            }
        };

        if (multipleVersions)
        {
            var expectedSecondEffectiveTo = effectiveTo.Add(_fixture.Create<TimeSpan>());
            expectedStandardEffectiveTo = expectedSecondEffectiveTo.Add(_fixture.Create<TimeSpan>());
            expectedTrainingProgrammeVersions.Add(new TrainingProgramme
            {
                CourseCode = expectedResponse.Standard.CourseCode,
                EffectiveFrom = effectiveTo,
                EffectiveTo = expectedSecondEffectiveTo,
            });
            expectedTrainingProgrammeVersions.Add(new TrainingProgramme
            {
                CourseCode = expectedResponse.Standard.CourseCode,
                EffectiveFrom = expectedSecondEffectiveTo,
                EffectiveTo = expectedStandardEffectiveTo,
            });
        }

        if (openEndedDates)
        {
            expectedStandardEffectiveFrom = null;
            expectedStandardEffectiveTo = null;
            expectedTrainingProgrammeVersions.First().EffectiveFrom = null;
            expectedTrainingProgrammeVersions.Last().EffectiveTo = null;
        }

        _mockCommitmentsV2ApiApiClient.Setup(x => x.Get<GetTrainingProgrammeVersionsResponse>(It.IsAny<GetTrainingProgrammeVersionsRequest>()))
            .ReturnsAsync(new GetTrainingProgrammeVersionsResponse{ TrainingProgrammeVersions = expectedTrainingProgrammeVersions });

        _mockCollectionCalendarApiClient
            .Setup(x => x.Get<GetAcademicYearsResponse>(It.Is<GetAcademicYearsRequest>(y =>
                y._dateTime == expectedResponse.ActualStartDate.Value.ToString("yyyy-MM-dd"))))
            .ReturnsAsync(new GetAcademicYearsResponse { StartDate = expectedEarliestStartDate });

        _mockCollectionCalendarApiClient
            .Setup(x => x.Get<GetAcademicYearsResponse>(It.Is<GetAcademicYearsRequest>(y =>
                y._dateTime == expectedResponse.ActualStartDate.Value.AddYears(1).ToString("yyyy-MM-dd"))))
            .ReturnsAsync(new GetAcademicYearsResponse { EndDate = expectedLatestStartDate });

        var handler = new GetApprenticeshipStartDateQueryHandler(_mocklogger.Object, _mockApprenticeshipsApiClient.Object, _mockCommitmentsV2ApiApiClient.Object, _mockCollectionCalendarApiClient.Object);

        //  Act
        var result = await handler.Handle(new GetApprenticeshipStartDateQuery(Guid.NewGuid()), CancellationToken.None);

        //  Assert
        result.Standard.EffectiveTo.Should().Be(expectedStandardEffectiveTo);
        result.Standard.EffectiveFrom.Should().Be(expectedStandardEffectiveFrom);
    }
}

