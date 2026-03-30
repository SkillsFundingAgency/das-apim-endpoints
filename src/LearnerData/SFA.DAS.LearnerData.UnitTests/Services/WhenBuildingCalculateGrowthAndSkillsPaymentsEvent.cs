using AutoFixture;
using Microsoft.Extensions.Logging;
using SFA.DAS.LearnerData.Services;
using SFA.DAS.Payments.EarningEvents.Messages.External;
using SFA.DAS.Payments.EarningEvents.Messages.External.Commands;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.CollectionCalendar;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.CollectionCalendar;
using SFA.DAS.LearnerData.Responses.EarningsInner;
using SFA.DAS.LearnerData.Responses.Learning;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.LearnerData.UnitTests.Services;

[TestFixture]
internal class WhenBuildingCalculateGrowthAndSkillsPaymentsEvent
{
    private Fixture _fixture = new Fixture();
    private Mock<ILogger<CalculateGrowthAndSkillsPaymentsEventBuilder>> _mockLogger;
    private Mock<ICollectionCalendarApiClient<CollectionCalendarApiConfiguration>> _mockCollectionCalendarApiClient;
    private GetAcademicYearsResponse _academicYear2425;
    private GetAcademicYearsResponse _academicYear2526;

    public WhenBuildingCalculateGrowthAndSkillsPaymentsEvent()
    {
        _academicYear2425 = new GetAcademicYearsResponse
        {
            AcademicYear = "2425",
            StartDate = new DateTime(2024, 8, 1),
            EndDate = new DateTime(2025, 7, 31)
        };
        _academicYear2526 = new GetAcademicYearsResponse
        {
            AcademicYear = "2526",
            StartDate = new DateTime(2025, 8, 1),
            EndDate = new DateTime(2026, 7, 31)
        };
    }

    [SetUp]
    public void Setup()
    {
        _mockLogger = new Mock<ILogger<CalculateGrowthAndSkillsPaymentsEventBuilder>>();
        _mockCollectionCalendarApiClient = new Mock<ICollectionCalendarApiClient<CollectionCalendarApiConfiguration>>();
        _mockCollectionCalendarApiClient.Setup(x => x.Get<GetAcademicYearsResponse>(It.Is<GetAcademicYearByYearRequest>(y => y.GetUrl == $"academicyears/2425")))
            .ReturnsAsync(_academicYear2425);
        _mockCollectionCalendarApiClient.Setup(x => x.Get<GetAcademicYearsResponse>(It.Is<GetAcademicYearByYearRequest>(y => y.GetUrl == $"academicyears/2526")))
            .ReturnsAsync(_academicYear2526);
    }

    [Test]
    public async Task When_NoInstalmentsReturned_DoesNotThrow()
    {
        // Arrange
        var ukprn = _fixture.Create<long>();
        var learningResponse = GetLearningPriceResponse();

        var earningsResponse = new ShortCourseEarningsResponse
        {
            EarningProfileVersion = Guid.NewGuid(),
            Instalments = new List<ShortCourseInstalment>()
        };

        var builder = new CalculateGrowthAndSkillsPaymentsEventBuilder(_mockLogger.Object, _mockCollectionCalendarApiClient.Object);

        // Act
        var result = await builder.Build(ukprn, learningResponse, earningsResponse);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<CalculateGrowthAndSkillsPayments>();
    }

    [Test]
    public async Task Then_BasicInformationCorrectlySet()
    {
        // Arrange
        var ukprn = _fixture.Create<long>();
        var learningResponse = GetLearningPriceResponse();
        var episode = learningResponse.Episodes.Single();
        var earningsResponse = GetEarningsResponse();
        var builder = new CalculateGrowthAndSkillsPaymentsEventBuilder(_mockLogger.Object, _mockCollectionCalendarApiClient.Object);
        
        // Act
        var result = await builder.Build(ukprn, learningResponse, earningsResponse);

        // Assert
        result.EarningsId.Should().Be(earningsResponse.EarningProfileVersion);
        result.UKPRN.Should().Be(ukprn);
        result.Learner.LearnerKey.Should().Be(learningResponse.LearningKey);
        result.Learner.ULN.Should().Be(long.Parse(learningResponse.Learner.Uln));
        result.Learner.Reference.Should().Be(episode.LearnerRef);
        result.Training.CourseType.Should().Be(Payments.EarningEvents.Messages.External.CourseType.ShortCourse);
        result.Training.LearningType.Should().Be(LearningType.ApprenticeshipUnit);
        result.Training.CourseCode.Should().Be(episode.CourseCode);
        result.Training.CourseReference.Should().Be(episode.CourseCode);
        result.Training.AgeAtStartOfTraining.Should().Be((byte)episode.AgeAtStart);
        result.Training.StartDate.Should().Be(episode.StartDate);
        result.Training.PlannedEndDate.Should().Be(episode.PlannedEndDate);

    }

    [TestCase(null, null, null, TestName = "ActualEndDate - No Actual End Date")]
    [TestCase("2023-03-01", null, "2023-03-01", TestName = "ActualEndDate - is Withdrawal date")]
    [TestCase(null, "2023-03-01", "2023-03-01", TestName = "ActualEndDate - is Completion date")]
    public async Task Then_ActualEndDateCorrectlySet(string? withdrawalDateString, string? completionDateString, string? expectedEndDateString)
    {
        // Arrange
        var ukprn = _fixture.Create<long>();
        var learningResponse = GetLearningPriceResponse();
        var episode = learningResponse.Episodes.Single();

        DateTime? withdrawalDate = null;
        DateTime? completionDate = null;
        DateTime? expectedEndDate = null;

        if (withdrawalDateString != null)
            withdrawalDate = DateTime.Parse(withdrawalDateString);

        if (completionDateString != null)
            completionDate = DateTime.Parse(completionDateString);

        if (expectedEndDateString != null)
            expectedEndDate = DateTime.Parse(expectedEndDateString);

        episode.WithdrawalDate = withdrawalDate;
        learningResponse.CompletionDate = completionDate;

        var earningsResponse = GetEarningsResponse();
        var builder = new CalculateGrowthAndSkillsPaymentsEventBuilder(_mockLogger.Object, _mockCollectionCalendarApiClient.Object);

        // Act
        var result = await builder.Build(ukprn, learningResponse, earningsResponse);

        // Assert
        result.Training.ActualEndDate.Should().Be(expectedEndDate);

    }

    [TestCase(null, null, TrainingStatus.Continuing, TestName = "TrainingStatus - Continuing")]
    [TestCase("2023-03-01", null, TrainingStatus.Withdrawn, TestName = "TrainingStatus - Withdrawn")]
    [TestCase(null, "2023-03-01", TrainingStatus.Completed, TestName = "TrainingStatus - Completed")]
    public async Task Then_TrainingStatusCorrectlySet(string? withdrawalDateString, string? completionDateString, TrainingStatus expectedStatus)
    {
        // Arrange
        var ukprn = _fixture.Create<long>();
        var learningResponse = GetLearningPriceResponse();
        var episode = learningResponse.Episodes.Single();

        DateTime? withdrawalDate = null;
        DateTime? completionDate = null;

        if (withdrawalDateString != null)
            withdrawalDate = DateTime.Parse(withdrawalDateString);

        if (completionDateString != null)
            completionDate = DateTime.Parse(completionDateString);

        episode.WithdrawalDate = withdrawalDate;
        learningResponse.CompletionDate = completionDate;

        var earningsResponse = GetEarningsResponse();
        var builder = new CalculateGrowthAndSkillsPaymentsEventBuilder(_mockLogger.Object, _mockCollectionCalendarApiClient.Object);

        // Act
        var result = await builder.Build(ukprn, learningResponse, earningsResponse);

        // Assert
        result.Training.TrainingStatus.Should().Be(expectedStatus);

    }

    [Test]
    public async Task When_EarningsOverSingleYear_ThenCorrectlySet()
    {
        // Arrange
        var ukprn = _fixture.Create<long>();
        var learningResponse = GetLearningPriceResponse();
        var episode = learningResponse.Episodes.Single();

        episode.Price = 1000m;
        var earningsResponse = new ShortCourseEarningsResponse
        {
            EarningProfileVersion = Guid.NewGuid(),
            Instalments = new List<ShortCourseInstalment>
            {
                new ShortCourseInstalment
                {
                    CollectionYear = 2526,
                    CollectionPeriod = 1,
                    Amount = 700m,
                    Type = "ThirtyPercentLearningComplete",
                    IsPayable = true
                },
                new ShortCourseInstalment
                {
                    CollectionYear = 2526,
                    CollectionPeriod = 2,
                    Amount = 300m,
                    Type = "LearningComplete",
                    IsPayable = true
                }
            }
        };
        var builder = new CalculateGrowthAndSkillsPaymentsEventBuilder(_mockLogger.Object, _mockCollectionCalendarApiClient.Object);
        
        // Act
        var result = await builder.Build(ukprn, learningResponse, earningsResponse);
        
        // Assert
        result.Earnings.Should().HaveCount(1);
        var earning = result.Earnings.First();
        earning.AcademicYear.Should().Be(2526);
        earning.PricePeriods.Should().HaveCount(1);

        var pricePeriod = earning.PricePeriods.First();
        pricePeriod.Price.Should().Be(1000m);
        pricePeriod.StartDate.Should().Be(episode.StartDate);
        pricePeriod.EndDate.Should().Be(episode.PlannedEndDate);
        pricePeriod.Periods.Should().HaveCount(2);

        pricePeriod.Periods.Should().ContainSingle(x =>
            x.DeliveryPeriod == 1 &&
            x.EarningType == EarningType.Milestone1 &&
            x.Amount == 700m &&
            x.Employer.AccountId == episode.EmployerAccountId &&
            x.Employer.FundingAccountId == episode.EmployerAccountId);

        pricePeriod.Periods.Should().ContainSingle(x => 
            x.DeliveryPeriod == 2 && 
            x.EarningType == EarningType.Completion && 
            x.Amount == 300m &&
            x.Employer.AccountId == episode.EmployerAccountId &&
            x.Employer.FundingAccountId == episode.EmployerAccountId);

    }

    [Test]
    public async Task When_EarningsOverSingleYear_And_OnlyOnePaymentPayable_ThenCorrectlySet()
    {
        // Arrange
        var ukprn = _fixture.Create<long>();
        var learningResponse = GetLearningPriceResponse();
        var episode = learningResponse.Episodes.Single();

        episode.Price = 1000m;

        var earningsResponse = new ShortCourseEarningsResponse
        {
            EarningProfileVersion = Guid.NewGuid(),
            Instalments = new List<ShortCourseInstalment>
            {
                new ShortCourseInstalment
                {
                    CollectionYear = 2526,
                    CollectionPeriod = 1,
                    Amount = 700m,
                    Type = "ThirtyPercentLearningComplete",
                    IsPayable = true
                },
                new ShortCourseInstalment
                {
                    CollectionYear = 2526,
                    CollectionPeriod = 2,
                    Amount = 300m,
                    Type = "LearningComplete",
                    IsPayable = false
                }
            }
        };
        var builder = new CalculateGrowthAndSkillsPaymentsEventBuilder(_mockLogger.Object, _mockCollectionCalendarApiClient.Object);

        // Act
        var result = await builder.Build(ukprn, learningResponse, earningsResponse);

        // Assert
        result.Earnings.Should().HaveCount(1);
        var earning = result.Earnings.First();
        earning.AcademicYear.Should().Be(2526);
        earning.PricePeriods.Should().HaveCount(1);

        var pricePeriod = earning.PricePeriods.First();
        pricePeriod.Price.Should().Be(1000m);
        pricePeriod.StartDate.Should().Be(episode.StartDate);
        pricePeriod.EndDate.Should().Be(episode.PlannedEndDate);
        pricePeriod.Periods.Should().HaveCount(1);

        pricePeriod.Periods.Should().ContainSingle(x =>
            x.DeliveryPeriod == 1 &&
            x.EarningType == EarningType.Milestone1 &&
            x.Amount == 700m &&
            x.Employer.AccountId == episode.EmployerAccountId &&
            x.Employer.FundingAccountId == episode.EmployerAccountId);
    }

    [Test]
    public async Task When_EarningsOverMultipleYears_ThenCorrectlySet()
    {
        // Arrange
        var ukprn = _fixture.Create<long>();
        var learningResponse = GetLearningPriceResponse();
        var episode = learningResponse.Episodes.Single();

        episode.Price = 1000m;
        episode.StartDate = new DateTime(2025, 7, 1);
        episode.PlannedEndDate = new DateTime(2025, 9, 20);

        var earningsResponse = new ShortCourseEarningsResponse
        {
            EarningProfileVersion = Guid.NewGuid(),
            Instalments = new List<ShortCourseInstalment>
            {
                new ShortCourseInstalment
                {
                    CollectionYear = 2425,
                    CollectionPeriod = 12,
                    Amount = 700m,
                    Type = "ThirtyPercentLearningComplete",
                    IsPayable = true
                },
                new ShortCourseInstalment
                {
                    CollectionYear = 2526,
                    CollectionPeriod = 2,
                    Amount = 300m,
                    Type = "LearningComplete",
                    IsPayable = true
                }
            }
        };
        var builder = new CalculateGrowthAndSkillsPaymentsEventBuilder(_mockLogger.Object, _mockCollectionCalendarApiClient.Object);

        // Act
        var result = await builder.Build(ukprn, learningResponse, earningsResponse);

        // Assert
        result.Earnings.Should().HaveCount(2);
        var earningFirstYear = result.Earnings.Where(x=>x.AcademicYear == 2425).Single();
        var earningSecondYear = result.Earnings.Where(x => x.AcademicYear == 2526).Single();

        earningFirstYear.PricePeriods.Should().HaveCount(1);

        var pricePeriodFirstYear = earningFirstYear.PricePeriods.First();
        pricePeriodFirstYear.Price.Should().Be(1000m);
        pricePeriodFirstYear.StartDate.Should().Be(episode.StartDate);
        pricePeriodFirstYear.EndDate.Should().Be(_academicYear2425.EndDate);
        pricePeriodFirstYear.Periods.Should().HaveCount(1);

        pricePeriodFirstYear.Periods.Should().ContainSingle(x =>
            x.DeliveryPeriod == 12 &&
            x.EarningType == EarningType.Milestone1 &&
            x.Amount == 700m &&
            x.Employer.AccountId == episode.EmployerAccountId &&
            x.Employer.FundingAccountId == episode.EmployerAccountId);


        earningSecondYear.PricePeriods.Should().HaveCount(1);

        var pricePeriodSecondYear = earningSecondYear.PricePeriods.First();
        pricePeriodSecondYear.Price.Should().Be(1000m);
        pricePeriodSecondYear.StartDate.Should().Be(_academicYear2526.StartDate);
        pricePeriodSecondYear.EndDate.Should().Be(episode.PlannedEndDate);
        pricePeriodSecondYear.Periods.Should().HaveCount(1);

        pricePeriodSecondYear.Periods.Should().ContainSingle(x =>
            x.DeliveryPeriod == 2 &&
            x.EarningType == EarningType.Completion &&
            x.Amount == 300m &&
            x.Employer.AccountId == episode.EmployerAccountId &&
            x.Employer.FundingAccountId == episode.EmployerAccountId);

    }


    private UpdateShortCourseLearningPutResponse GetLearningPriceResponse()
    {
        var response = _fixture.Create<UpdateShortCourseLearningPutResponse>();

        response.Learner.Uln = "1234567890";
        response.Episodes = new[]
        {
            _fixture.Build<UpdateShortCourseResultEpisode>()
                .With(x => x.LearningType, "ApprenticeshipUnit")
                .With(x => x.CourseCode, "SC123")
                .With(x => x.LearnerRef, "LR123")
                .With(x => x.AgeAtStart, 20)
                .With(x => x.StartDate, new DateTime(2025, 7, 1))
                .With(x => x.PlannedEndDate, new DateTime(2025, 9, 20))
                .Create()
        };

        return response;
    }

    private ShortCourseEarningsResponse GetEarningsResponse()
    {
        var response = new ShortCourseEarningsResponse
        {
            EarningProfileVersion = Guid.NewGuid(),
            Instalments = new List<ShortCourseInstalment>()
        };

        response.Instalments.Add(new ShortCourseInstalment
        {
            CollectionYear = 2526,
            CollectionPeriod = 1,
            Amount = 700m,
            Type = "ThirtyPercentLearningComplete",
            IsPayable = true
        });

        response.Instalments.Add(new ShortCourseInstalment
        {
            CollectionYear = 2526,
            CollectionPeriod = 2,
            Amount = 300m,
            Type = "LearningComplete",
            IsPayable = true
        });

        return response;
    }

}