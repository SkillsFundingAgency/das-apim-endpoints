using SFA.DAS.LearnerData.Responses.EarningsInner;
using SFA.DAS.LearnerData.UnitTests.Application.Fm36.TestHelpers;
using static SFA.DAS.LearnerData.Application.Fm36.Common.EarningsFM36Constants;
using PeriodTextValues = ESFA.DC.ILR.FundingService.FM36.FundingOutput.Model.Output.LearningDeliveryPeriodisedTextValues;

namespace SFA.DAS.LearnerData.UnitTests.Application.Fm36.WhenHandlingLearningDeliveries;

public class LearningDeliveryPeriodisedTextValues
{
    [TestCase(TestScenario.SimpleApprenticeship, "2024-08-01", "2025-07-31", new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 })]
    [TestCase(TestScenario.SimpleApprenticeship, "2024-08-01", "2025-01-31", new byte[] { 1, 2, 3, 4, 5, 6 })]
    [TestCase(TestScenario.ApprenticeshipWithPriceChange, "2024-08-01", "2025-07-31", new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 })]
    [TestCase(TestScenario.ApprenticeshipWithEnglish, "2024-08-01", "2025-07-31", new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 })]
    [TestCase(TestScenario.ApprenticeshipWithEnglish, "2024-12-01", "2025-01-31", new byte[] { 5, 6 })]
    public async Task ThenReturnsLearningDeliveryPeriodisedTextValuesForEachApprenticeship(
        TestScenario scenario, string startDate, string endDate, byte[] expectedPopulatedPeriods)
    {
        // Arrange
        var testFixture = new GetFm36QueryTestFixture(scenario, (context) => SetDateRange(scenario, context, startDate, endDate));

        // Act
        await testFixture.CallSubjectUnderTest(collectionYear:2425);


        // Assert
        testFixture.Result.Should().NotBeNull();

        var apprenticeship = testFixture.UnpagedLearningsResponse.Single();
        var earningsApprenticeship = testFixture.EarningsResponse.Apprenticeships.First();

        var learningDelivery = testFixture.GetLearningDelivery(scenario);
        learningDelivery.Should().NotBeNull();
        learningDelivery.LearningDeliveryPeriodisedTextValues.Should().NotBeNull();

        var fundingLineType = learningDelivery.LearningDeliveryPeriodisedTextValues.Where(x=> x.AttributeName == "FundLineType").SingleOrDefault();
        fundingLineType.Should().NotBeNull();
        AssertTextValues(fundingLineType, expectedPopulatedPeriods, earningsApprenticeship.FundingLineType);

        var learnDelContType = learningDelivery.LearningDeliveryPeriodisedTextValues.Where(x => x.AttributeName == "LearnDelContType").SingleOrDefault();
        learnDelContType.Should().NotBeNull();
        AssertTextValues(learnDelContType, expectedPopulatedPeriods, "Contract for services with the employer");

    }

    private void SetDateRange(TestScenario scenario, LearnerData.TestHelpers.Fm36TestContext context, string startDateString, string endDateString)
    {
        var startDate = DateTime.Parse(startDateString);
        var endDate = DateTime.Parse(endDateString);
        
        var learner = context.TestLearners.Single().UpdateLearnerRequest;

        if (scenario == TestScenario.ApprenticeshipWithEnglish)
        {
            var english = learner.Delivery.EnglishAndMaths.Single();
            english.StartDate = startDate;
            english.EndDate = endDate;
        }
        else
        {
            var onProgramme = learner.Delivery.OnProgramme.Single();
            onProgramme.StartDate = startDate;
            onProgramme.ExpectedEndDate = endDate;
        }
    }

    private void AssertTextValues(PeriodTextValues textValues, byte[] populatedPeriods, string expectedText)
    {


        textValues.Period1.Should().Be(populatedPeriods.Any(x => x == 1) ? expectedText : "None");
        textValues.Period2.Should().Be(populatedPeriods.Any(x => x == 2) ? expectedText : "None");
        textValues.Period3.Should().Be(populatedPeriods.Any(x => x == 3) ? expectedText : "None");
        textValues.Period4.Should().Be(populatedPeriods.Any(x => x == 4) ? expectedText : "None");
        textValues.Period5.Should().Be(populatedPeriods.Any(x => x == 5) ? expectedText : "None");
        textValues.Period6.Should().Be(populatedPeriods.Any(x => x == 6) ? expectedText : "None");
        textValues.Period7.Should().Be(populatedPeriods.Any(x => x == 7) ? expectedText : "None");
        textValues.Period8.Should().Be(populatedPeriods.Any(x => x == 8) ? expectedText : "None");
        textValues.Period9.Should().Be(populatedPeriods.Any(x => x == 9) ? expectedText : "None");
        textValues.Period10.Should().Be(populatedPeriods.Any(x => x == 10) ? expectedText : "None");
        textValues.Period11.Should().Be(populatedPeriods.Any(x => x == 11) ? expectedText : "None");
        textValues.Period12.Should().Be(populatedPeriods.Any(x => x == 12) ? expectedText : "None");


    }
}
