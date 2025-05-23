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

            scenarioContext.Set(expected);

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

        [Then(@"all other (.*) values are (.*)")]
        public void ThenAllOtherPriceEpisodeFirstEmpPayValuesAre(string attributeName, int value)
        {
            var expected = scenarioContext.Get<List<Models.PriceEpisodePeriodisedValuesModel>>();
            var learners = scenarioContext.Get<List<FM36Learner>>();
            var learner = learners.First();

            var episodeOrdinal = 0;
            foreach (var priceEpisode in learner.PriceEpisodes)
            {
                foreach (var periodisedValue in priceEpisode.PriceEpisodePeriodisedValues.Where(x => x.AttributeName == attributeName))
                {
                    for (var i = 1; i < 12; i++)
                    {
                        if (!expected.Any(x => x.Period == i && x.Episode == episodeOrdinal))
                        {
                            periodisedValue.GetPeriodValue(i).Should().Be(0);
                        }
                    }
                }

                episodeOrdinal++;
            }
        }
    }
}
