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
        [Test]
        public void When_OnProgrammeEpisodesHaveGap_Then_BreakIsCalculated()
        {
            // Arrange
            var command = BreaksInLearningTestHelper.CreateLearnerWithBreaksInLearning(false);

            var firstEpisode = command.UpdateLearnerRequest.Delivery.OnProgramme[0];
            var secondEpisode = command.UpdateLearnerRequest.Delivery.OnProgramme[1];

            var breaksInLearningService = new BreaksInLearningService();

            // Act
            var result = breaksInLearningService.CalculateOnProgrammeBreaksInLearning([firstEpisode, secondEpisode]);

            // Assert
            var expectedBreak = new BreakInLearning
            {
                StartDate = firstEpisode.ActualEndDate!.Value.AddDays(1),
                EndDate = secondEpisode.StartDate.AddDays(-1)
            };

            result.Should().ContainSingle()
                .Which.Should().BeEquivalentTo(expectedBreak);
        }
    }
}
