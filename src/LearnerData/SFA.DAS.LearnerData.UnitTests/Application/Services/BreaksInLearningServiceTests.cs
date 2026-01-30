using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.LearnerData.Services;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.LearnerData;

namespace SFA.DAS.LearnerData.UnitTests.Application.Services;

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
            StartDate = firstEpisode.PauseDate!.Value.AddDays(1),
            EndDate = secondEpisode.StartDate.AddDays(-1),
            PriorPeriodExpectedEndDate = firstEpisode.ExpectedEndDate
        };

        result.Should().ContainSingle()
            .Which.Should().BeEquivalentTo(expectedBreak);
    }

    [Test]
    public void When_EnglishAndMathsCoursesHaveGap_Then_BreakIsCalculated()
    {
        // Arrange
        var command = BreaksInLearningTestHelper.CreateLearnerWithEnglishAndMathsBreaksInLearning(true);

        var breaksInLearningService = new BreaksInLearningService();
        var groupedCourses = command.UpdateLearnerRequest.Delivery.EnglishAndMaths
            .GroupBy(c => c.LearnAimRef)
            .First()
            .ToList(); // Not strictly necessary to do this for the test, but keeps it similar to real usage
        var firstEpisode = groupedCourses.OrderBy(x=>x.StartDate).First();
        var secondEpisode = groupedCourses.OrderBy(x=>x.StartDate).Last();

        // Act
        var result = breaksInLearningService.CalculateEnglishAndMathsBreaksInLearning(groupedCourses);

        // Assert
        var expectedBreak = new BreakInLearning
        {
            StartDate = firstEpisode.PauseDate!.Value.AddDays(1),
            EndDate = secondEpisode.StartDate.AddDays(-1),
            PriorPeriodExpectedEndDate = firstEpisode.EndDate
        };

        result.Should().ContainSingle()
            .Which.Should().BeEquivalentTo(expectedBreak);
    }
}
