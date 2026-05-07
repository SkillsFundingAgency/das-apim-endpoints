using SFA.DAS.LearnerData.UnitTests.Application.Fm36.TestHelpers;

namespace SFA.DAS.LearnerData.UnitTests.Application.Fm36.WhenHandlingLearningDeliveries;

public class LearningDeliveryPeriodisedTextValues
{
    [TestCase(TestScenario.SimpleApprenticeship)]
    [TestCase(TestScenario.ApprenticeshipWithPriceChange)]
    public async Task ThenReturnsLearningDeliveryPeriodisedTextValuesForEachApprenticeship(TestScenario scenario)
    {
        // Arrange
        var testFixture = new GetFm36QueryTestFixture(scenario);

        // Act
        await testFixture.CallSubjectUnderTest();


        // Assert
        testFixture.Result.Should().NotBeNull();

        var apprenticeship = testFixture.UnpagedLearningsResponse.Single();
        var earningsApprenticeship = testFixture.EarningsResponse.Apprenticeships.First();

        var learningDelivery = testFixture.Result.Items.SingleOrDefault(learner => learner.ULN.ToString() == apprenticeship.Uln).LearningDeliveries.SingleOrDefault();
        learningDelivery.Should().NotBeNull();
        learningDelivery.LearningDeliveryPeriodisedTextValues.Should().NotBeNull();
        learningDelivery.LearningDeliveryPeriodisedTextValues.Should().Contain(x =>
            x.AttributeName == "FundLineType" && x.AllValuesAreSetTo(earningsApprenticeship.FundingLineType));
        learningDelivery.LearningDeliveryPeriodisedTextValues.Should().Contain(x =>
            x.AttributeName == "LearnDelContType" && x.AllValuesAreSetTo("Contract for services with the employer"));
    }
}
