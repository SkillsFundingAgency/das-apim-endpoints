using AutoFixture;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.LearnerData.Application.UpdateLearner;
using SFA.DAS.LearnerData.Requests;
using SFA.DAS.LearnerData.Services;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.LearnerData;

namespace SFA.DAS.LearnerData.UnitTests.Application.Services
{
    [TestFixture]
    public class BreaksInLearningServiceTests
    {
        private readonly Fixture _fixture = new Fixture();

        [Test]
        public void When_OnProgrammeEpisodesHaveGap_Then_BreakIsCalculated()
        {
            var fixture = new Fixture();

            // Arrange: create two episodes with a gap
            var episodeKey = fixture.Create<Guid>();
            var command = CreateLearnerWithBreaksInLearning(false);

            var firstEpisode = command.UpdateLearnerRequest.Delivery.OnProgramme[0];
            var secondEpisode = command.UpdateLearnerRequest.Delivery.OnProgramme[1];

            var sut = new BreaksInLearningService();

            // Act: calculate breaks directly
            var result = sut.CalculateOnProgrammeBreaksInLearning(
                new List<OnProgrammeRequestDetails> { firstEpisode, secondEpisode });

            // Assert: expect one break spanning the gap
            var expectedBreak = new BreakInLearning
            {
                StartDate = firstEpisode.ActualEndDate!.Value.AddDays(1),
                EndDate = secondEpisode.StartDate.AddDays(-1)
            };

            result.Should().ContainSingle()
                .Which.Should().BeEquivalentTo(expectedBreak);
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
