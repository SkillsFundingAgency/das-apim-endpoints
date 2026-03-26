using AutoFixture;
using Microsoft.Extensions.Logging;
using SFA.DAS.LearnerData.Services;
using SFA.DAS.Payments.EarningEvents.Messages.External;
using SFA.DAS.Payments.EarningEvents.Messages.External.Commands;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Earnings;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.LearnerData;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.LearnerData.UnitTests.Services;

[TestFixture]
internal class WhenBuildingCalculateGrowthAndSkillsPaymentsEvent
{
    private Fixture _fixture = new Fixture();
    private Mock<ILogger<CalculateGrowthAndSkillsPaymentsEventBuilder>> _mockLogger;
    private Mock<ICollectionCalendarApiClient<CollectionCalendarApiConfiguration>> _mockCollectionCalendarApiClient;

    [SetUp]
    public void Setup()
    {
        _mockLogger = new Mock<ILogger<CalculateGrowthAndSkillsPaymentsEventBuilder>>();
        _mockCollectionCalendarApiClient = new Mock<ICollectionCalendarApiClient<CollectionCalendarApiConfiguration>>();
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
        var earningsResponse = GetEarningsResponse();
        var builder = new CalculateGrowthAndSkillsPaymentsEventBuilder(_mockLogger.Object, _mockCollectionCalendarApiClient.Object);
        
        // Act
        var result = await builder.Build(ukprn, learningResponse, earningsResponse);
        
        // Assert
        result.UKPRN.Should().Be(ukprn);
        result.EarningsId.Should().Be(earningsResponse.EarningProfileVersion);
        result.Learner.LearnerKey.Should().Be(learningResponse.LearningKey);
        result.Learner.ULN.Should().Be(learningResponse.Uln);
        result.Learner.Reference.Should().Be(learningResponse.LearnerRef);
        result.Training.CourseType.Should().Be(Payments.EarningEvents.Messages.External.CourseType.ShortCourse);
        result.Training.LearningType.Should().Be(LearningType.ApprenticeshipUnit);
        result.Training.CourseCode.Should().Be(learningResponse.TrainingCode);
        result.Training.CourseReference.Should().Be(learningResponse.TrainingCode);
        result.Training.StartDate.Should().Be(learningResponse.StartDate);
        result.Training.PlannedEndDate.Should().Be(learningResponse.PlannedEndDate);

    }

    [TestCase("2000-03-01", "2020-03-01", 20, TestName = "CorrectAge - Exact birthday")]
    [TestCase("2000-03-02", "2020-03-01", 19, TestName = "CorrectAge - Before birthday")]
    [TestCase("2000-02-28", "2020-03-01", 20, TestName = "CorrectAge - After birthday")]
    [TestCase("2004-02-29", "2023-02-28", 18, TestName = "CorrectAge - Leap year - before birthday")]
    [TestCase("2004-02-29", "2023-03-01", 19, TestName = "CorrectAge - Leap year - after birthday")]
    [TestCase("2000-03-01", "2020-02-29", 19, TestName = "CorrectAge - Day before birthday (leap year)")]
    public async Task Then_AgeAtStartOfTrainingCorrectlySet(
        string dobString,
        string startDateString,
        int expectedAge)
    {
        // Arrange
        var ukprn = _fixture.Create<long>();

        var dob = DateTime.Parse(dobString);
        var startDate = DateTime.Parse(startDateString);

        var learningResponse = GetLearningPriceResponse();
        learningResponse.StartDate = startDate;
        learningResponse.DateOfBirth = dob;
        var earningsResponse = GetEarningsResponse();

        var builder = new CalculateGrowthAndSkillsPaymentsEventBuilder(
            _mockLogger.Object,
            _mockCollectionCalendarApiClient.Object);

        // Act
        var result = await builder.Build(ukprn, learningResponse, earningsResponse);

        // Assert
        result.Training.AgeAtStartOfTraining.Should().Be((byte)expectedAge);
    }

    [TestCase(null, null, null, TestName = "ActualEndDate - No Actual End Date")]
    [TestCase("2023-03-01", null, "2023-03-01", TestName = "ActualEndDate - is Withdrawal date")]
    public async Task Then_ActualEndDateCorrectlySet(string? withdrawalDateString, string? completionDateString, string? expectedEndDateString)
    {
        // Arrange
        var ukprn = _fixture.Create<long>();
        var learningResponse = GetLearningPriceResponse();

        DateTime? withdrawalDate = null;
        DateTime? completionDate = null;
        DateTime? expectedEndDate = null;

        if (withdrawalDateString != null)
            withdrawalDate = DateTime.Parse(withdrawalDateString);

        if (completionDateString != null)
            completionDate = DateTime.Parse(completionDateString);

        if (expectedEndDateString != null)
            expectedEndDate = DateTime.Parse(expectedEndDateString);

        learningResponse.WithdrawalDate = withdrawalDate;
        learningResponse.CompletionDate = completionDate;

        var earningsResponse = GetEarningsResponse();
        var builder = new CalculateGrowthAndSkillsPaymentsEventBuilder(_mockLogger.Object, _mockCollectionCalendarApiClient.Object);

        // Act
        var result = await builder.Build(ukprn, learningResponse, earningsResponse);

        // Assert
        result.Training.ActualEndDate.Should().Be(expectedEndDate);

    }


    private UpdateShortCourseLearningPutResponse GetLearningPriceResponse()
    {
        var response = _fixture.Create<UpdateShortCourseLearningPutResponse>();

        response.LearningType = "ApprenticeshipUnit";

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
