using SFA.DAS.LearnerData.UnitTests.Application.Fm36.TestHelpers;

namespace SFA.DAS.LearnerData.UnitTests.Application.Fm36.WhenHandlingLearningDeliveries;

public class LearningDelivery
{
    [Test]
    public async Task ThenALearningDeliveryIsCreatedForEachApprenticeship()
    {
        // Arrange
        var testFixture = new GetFm36QueryTestFixture(TestScenario.AllData);

        // Act
        await testFixture.CallSubjectUnderTest();

        // Assert
        testFixture.Result.Should().NotBeNull();

        testFixture.Result.Items.Count.Should().Be(testFixture.UnpagedLearningsResponse.Count);
        testFixture.Result.Items.SelectMany(learner => learner.LearningDeliveries).Count().Should().Be(testFixture.UnpagedLearningsResponse.Count);
    }


    [TestCase(TestScenario.SimpleApprenticeship)]
    [TestCase(TestScenario.ApprenticeshipWithPriceChange)]
    [TestCase(TestScenario.ApprenticeshipWithEnglish)]
    public async Task Then_AimSeqNumber_IsCorrect(TestScenario scenario)
    {
        // Arrange
        var testFixture = new GetFm36QueryTestFixture(scenario);

        // Act
        await testFixture.CallSubjectUnderTest();

        // Assert
        var learningDelivery = testFixture.GetLearningDelivery(scenario);

        if (scenario == TestScenario.ApprenticeshipWithEnglish)
        {
            learningDelivery.AimSeqNumber.Should().Be(2);
        }
        else
        {
            learningDelivery.AimSeqNumber.Should().Be(1);
        }
    }
}
