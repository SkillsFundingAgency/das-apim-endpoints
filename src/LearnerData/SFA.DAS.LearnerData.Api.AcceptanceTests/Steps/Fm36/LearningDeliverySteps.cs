using ESFA.DC.ILR.FundingService.FM36.FundingOutput.Model.Output;
using FluentAssertions;
using SFA.DAS.LearnerData.Api.AcceptanceTests.Models;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace SFA.DAS.LearnerData.Api.AcceptanceTests.Steps.Fm36;

[Binding]
public  class LearningDeliverySteps(TestContext testContext, ScenarioContext scenarioContext)
{
    [Given(@"has the following sld onprogramme records")]
    public void GivenHasTheFollowingSldOnprogrammeRecords(Table table)
    {
        var learningDeliveries = table.CreateSet<LearningDeliveryModel>().ToList();

        var apprenticeship = scenarioContext.Get<ApprenticeshipModel>();
        apprenticeship.LearningDeliveries = learningDeliveries;
    }

    [Then(@"FundStart for the Learning Delivery is (.*)")]
    public void ThenFundStartForTheLearningDeliveryIsExpected_Fundstart(bool expectedFundStart)
    {
        var learner = GetFm36Learner();

        var learningDelivery = learner.LearningDeliveries.First();

        learningDelivery.LearningDeliveryValues.FundStart.Should().Be(expectedFundStart);
    }

    [Then(@"ThresholdDays for the Learning Delivery is (.*)")]
    public void ThenThresholdDaysForTheLearningDeliveryIs(int expectedThresholdDays)
    {
        var learner = GetFm36Learner();

        var learningDelivery = learner.LearningDeliveries.First();

        learningDelivery.LearningDeliveryValues.ThresholdDays.Should().Be(expectedThresholdDays);
    }

    [Then(@"there are (.*) learning deliveries")]
    public void ThenThereAreLearningDeliveries(int expectedLearningDeliveriesCount)
    {
        var learner = GetFm36Learner();

        learner.LearningDeliveries.Count.Should().Be(expectedLearningDeliveriesCount);
    }

    [Then(@"the aim sequence numbers on the learning deliveries are")]
    public void ThenTheAimSequenceNumbersOnTheLearningDeliveriesAre(Table table)
    {
        var learningDeliveries = table.CreateSet<LearningDeliveryModel>().ToList();

        var learner = GetFm36Learner();

        foreach (var learningDelivery in learningDeliveries)
        {
            learner.LearningDeliveries.Should().Contain(ld =>
                ld.AimSeqNumber == learningDelivery.AimSequenceNumber &&
                ld.LearningDeliveryValues.LearnAimRef == learningDelivery.LearnAimRef);
        }
    }

    private FM36Learner GetFm36Learner()
    {
        var learners = scenarioContext.Get<List<FM36Learner>>();
        return learners.First();
    }
}
