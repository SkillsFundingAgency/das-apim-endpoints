using AutoFixture;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.LearnerData.Application.UpdateLearner;
using SFA.DAS.LearnerData.Requests;
using SFA.DAS.LearnerData.Services;

namespace SFA.DAS.LearnerData.UnitTests.Application.Services
{
    [TestFixture]
    public class CostsServiceTests
    {
        private readonly Fixture _fixture = new Fixture();

        [Test]
        public void With_No_Change_In_Price_Then_Costs_Are_Merged()
        {
            // Arrange: helper builds two episodes with the same price
            var command = CreateLearnerWithBreaksInLearning(false);
            var onProgrammes = command.UpdateLearnerRequest.Delivery.OnProgramme;

            var sut = new CostsService();

            // Act
            var result = sut.GetCosts(onProgrammes);

            // Assert: only one merged cost
            result.Should().HaveCount(1);

            var expected = onProgrammes.First().Costs.First();
            result.First().Should().BeEquivalentTo(expected);
        }
        
        [Test]
        public void With_Change_In_Price_Then_Costs_Are_Split()
        {
            // Arrange: helper builds two episodes with different prices
            var command = CreateLearnerWithBreaksInLearning(true);
            var onProgrammes = command.UpdateLearnerRequest.Delivery.OnProgramme;

            var sut = new CostsService();

            // Act
            var result = sut.GetCosts(onProgrammes);

            // Assert: two separate costs
            result.Should().HaveCount(2);

            var expectedFirstCost = onProgrammes[0].Costs.First();
            var expectedSecondCost = onProgrammes[1].Costs.First();

            result[0].Should().BeEquivalentTo(expectedFirstCost, opts => opts.Excluding(c => c.FromDate));
            result[1].Should().BeEquivalentTo(expectedSecondCost, opts => opts.Excluding(c => c.FromDate));
        }

        private UpdateLearnerCommand CreateLearnerWithBreaksInLearning(bool withPriceChange)
        {
            var command = _fixture.Create<UpdateLearnerCommand>();
            command.UpdateLearnerRequest.Delivery.OnProgramme.Clear();

            var standardCode = _fixture.Create<int>();
            var agreementId = _fixture.Create<string>();
            var startDate = _fixture.Create<DateTime>();
            var pauseDate = startDate.AddMonths(6);
            var resumeDate = pauseDate.AddMonths(6);

            var initialCosts = new List<CostDetails>
            {
                new CostDetails
                {
                    FromDate = startDate,
                    TrainingPrice = _fixture.Create<int>(),
                    EpaoPrice = _fixture.Create<int>(),
                }
            };

            var resumeCosts = withPriceChange ?
                [
                    new CostDetails
                    {
                        FromDate = resumeDate,
                        TrainingPrice = initialCosts.First().TrainingPrice + 1000,
                        EpaoPrice = initialCosts.First().EpaoPrice + 1000,
                    }
                ]
                : initialCosts;

            command.UpdateLearnerRequest.Delivery.OnProgramme.Add(new OnProgrammeRequestDetails
            {
                StartDate = startDate,
                ExpectedEndDate = startDate.AddYears(2),
                ActualEndDate = pauseDate,
                StandardCode = standardCode,
                AgreementId = agreementId,
                PauseDate = null,
                Costs = initialCosts,
                LearningSupport = []
            });

            command.UpdateLearnerRequest.Delivery.OnProgramme.Add(new OnProgrammeRequestDetails
            {
                StartDate = resumeDate,
                ExpectedEndDate = resumeDate.AddYears(2),
                StandardCode = standardCode,
                AgreementId = agreementId,
                PauseDate = null,
                WithdrawalDate = null,
                CompletionDate = null,
                Costs = resumeCosts,
                LearningSupport = []
            });

            command.UpdateLearnerRequest.Delivery.EnglishAndMaths.Clear();

            return command;
        }
    }
}
