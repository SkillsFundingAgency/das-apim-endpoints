using ESFA.DC.ILR.FundingService.FM36.FundingOutput.Model.Output;
using FluentAssertions;
using SFA.DAS.Earnings.Api.AcceptanceTests.Extensions;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace SFA.DAS.Earnings.Api.AcceptanceTests.Steps
{
    [Binding]
    public class PeriodisedValuesSteps(TestContext testContext, ScenarioContext scenarioContext)
    {
        [Then(@"the Price Episode Periodised Values are as follows:")]
        public void ThenThePriceEpisodePeriodisedValuesAreAsFollows(Table table)
        {
            var expected = table.CreateSet<Models.PriceEpisodePeriodisedValuesModel>().ToList();

            var learners = scenarioContext.Get<List<FM36Learner>>();
            var learner = learners.First();

            foreach (var expectation in expected)
            {
                var priceEpisode = learner.PriceEpisodes[expectation.Episode];

                var actual =
                    priceEpisode.PriceEpisodePeriodisedValues.Single(x => x.AttributeName == expectation.Attribute);

                actual.GetPeriodValue(expectation.Period).Should().Be(expectation.Value);
            }
        }
    }
}
