using ESFA.DC.ILR.FundingService.FM36.FundingOutput.Model.Output;
using FluentAssertions;
using TechTalk.SpecFlow;

namespace SFA.DAS.LearnerData.Api.AcceptanceTests.Steps.Fm36
{
    [Binding]
    public  class LearningDeliverySteps(TestContext testContext, ScenarioContext scenarioContext)
    {
        [Then(@"FundStart for the Learning Delivery is (.*)")]
        public void ThenFundStartForTheLearningDeliveryIsExpected_Fundstart(bool expectedFundStart)
        {
            var learners = scenarioContext.Get<List<FM36Learner>>();
            var learner = learners.First();

            var learningDelivery = learner.LearningDeliveries.First();

            learningDelivery.LearningDeliveryValues.FundStart.Should().Be(expectedFundStart);
        }

        [Then(@"ThresholdDays for the Learning Delivery is (.*)")]
        public void ThenThresholdDaysForTheLearningDeliveryIs(int expectedThresholdDays)
        {
            var learners = scenarioContext.Get<List<FM36Learner>>();
            var learner = learners.First();

            var learningDelivery = learner.LearningDeliveries.First();

            learningDelivery.LearningDeliveryValues.ThresholdDays.Should().Be(expectedThresholdDays);
        }
    }
}
