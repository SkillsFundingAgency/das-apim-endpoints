using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.LearnerData.Requests;
using SFA.DAS.LearnerData.Services;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.LearnerData;

namespace SFA.DAS.LearnerData.UnitTests.Application.Services
{
    [TestFixture]
    public class LearningSupportServiceTests
    {
        [Test]
        public void With_LearningSupport_Pre_And_Post_Break_Then_Learning_Is_Updated_With_Merged_LSF()
        {
            // Arrange
            var command = BreaksInLearningTestHelper.CreateLearnerWithBreaksInLearning(false);
            var preBreak = command.UpdateLearnerRequest.Delivery.OnProgramme.First();
            var postBreak = command.UpdateLearnerRequest.Delivery.OnProgramme.Skip(1).First();

            var lsf1 = new LearningSupportRequestDetails
            {
                StartDate = preBreak.StartDate,
                EndDate = preBreak.ActualEndDate!.Value
            };

            var lsf2 = new LearningSupportRequestDetails
            {
                StartDate = postBreak.StartDate,
                EndDate = postBreak.ExpectedEndDate
            };

            preBreak.LearningSupport.Add(lsf1);
            postBreak.LearningSupport.Add(lsf2);

            var sut = new LearningSupportService();

            // Act
            var actual = sut.GetCombinedLearningSupport([preBreak, postBreak],
                postBreak.ExpectedEndDate,
                command.UpdateLearnerRequest.Delivery.EnglishAndMaths,
                []);

            // Assert
            actual.Count.Should().Be(2);
            actual.First().Should().BeEquivalentTo(lsf1);
            actual.Skip(1).First().Should().BeEquivalentTo(lsf2);
        }

        [Test]
        public void With_LearningSupport_Overhanging_Start_Of_Break_Then_LSF_Is_Truncated()
        {
            var command = BreaksInLearningTestHelper.CreateLearnerWithBreaksInLearning(false);
            var preBreak = command.UpdateLearnerRequest.Delivery.OnProgramme.First();
            var postBreak = command.UpdateLearnerRequest.Delivery.OnProgramme.Skip(1).First();

            var lsf1 = new LearningSupportRequestDetails
            {
                StartDate = preBreak.StartDate,
                EndDate = preBreak.ActualEndDate!.Value.AddMonths(1)
            };
            preBreak.LearningSupport.Add(lsf1);

            var breaks = new List<BreakInLearning>
            {
                new BreakInLearning { StartDate = preBreak.ActualEndDate.Value.AddDays(1), EndDate = postBreak.StartDate.AddDays(-1) }
            };

            var sut = new LearningSupportService();

            var actual = sut.GetCombinedLearningSupport([preBreak], postBreak.ExpectedEndDate, command.UpdateLearnerRequest.Delivery.EnglishAndMaths, breaks);

            var expected = new LearningSupportUpdatedDetails
            {
                StartDate = preBreak.StartDate,
                EndDate = preBreak.ActualEndDate.Value
            };

            actual.Should().ContainSingle().Which.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void With_LearningSupport_Overhanging_End_Of_Break_Then_LSF_Is_Truncated()
        {
            var command = BreaksInLearningTestHelper.CreateLearnerWithBreaksInLearning(false);
            var preBreak = command.UpdateLearnerRequest.Delivery.OnProgramme.First();
            var postBreak = command.UpdateLearnerRequest.Delivery.OnProgramme.Skip(1).First();

            var lsf1 = new LearningSupportRequestDetails
            {
                StartDate = postBreak.StartDate.AddMonths(-1),
                EndDate = postBreak.ExpectedEndDate
            };
            postBreak.LearningSupport.Add(lsf1);

            var breaks = new List<BreakInLearning>
            {
                new BreakInLearning { StartDate = preBreak.ActualEndDate!.Value.AddDays(1), EndDate = postBreak.StartDate.AddDays(-1) }
            };

            var sut = new LearningSupportService();

            var actual = sut.GetCombinedLearningSupport([postBreak], postBreak.ExpectedEndDate, command.UpdateLearnerRequest.Delivery.EnglishAndMaths, breaks);

            var expected = new LearningSupportUpdatedDetails
            {
                StartDate = postBreak.StartDate,
                EndDate = postBreak.ExpectedEndDate
            };

            actual.Should().ContainSingle().Which.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void With_LearningSupport_Containing_Break_Then_LSF_Is_Split()
        {
            var command = BreaksInLearningTestHelper.CreateLearnerWithBreaksInLearning(false);
            var preBreak = command.UpdateLearnerRequest.Delivery.OnProgramme.First();
            var postBreak = command.UpdateLearnerRequest.Delivery.OnProgramme.Skip(1).First();

            var lsf1 = new LearningSupportRequestDetails
            {
                StartDate = preBreak.StartDate,
                EndDate = postBreak.ExpectedEndDate
            };
            preBreak.LearningSupport.Add(lsf1);

            var breaks = new List<BreakInLearning>
            {
                new BreakInLearning { StartDate = preBreak.ActualEndDate!.Value.AddDays(1), EndDate = postBreak.StartDate.AddDays(-1) }
            };

            var sut = new LearningSupportService();

            var actual = sut.GetCombinedLearningSupport([preBreak, postBreak], postBreak.ExpectedEndDate, command.UpdateLearnerRequest.Delivery.EnglishAndMaths, breaks);

            var expected = new[]
            {
                new LearningSupportUpdatedDetails { StartDate = preBreak.StartDate, EndDate = preBreak.ActualEndDate.Value },
                new LearningSupportUpdatedDetails { StartDate = postBreak.StartDate, EndDate = postBreak.ExpectedEndDate }
            };

            actual.Should().BeEquivalentTo(expected, opts => opts.WithoutStrictOrdering());
        }

        [Test]
        public void With_EnglishAndMaths_LearningSupport_Overhanging_Start_Of_Break_Then_LSF_Is_Truncated()
        {
            var command = BreaksInLearningTestHelper.CreateLearnerWithEnglishAndMathsBreaksInLearning();
            var lsf1 = new LearningSupportRequestDetails
            {
                StartDate = command.UpdateLearnerRequest.Delivery.EnglishAndMaths.First().StartDate,
                EndDate = command.UpdateLearnerRequest.Delivery.EnglishAndMaths.First().EndDate
            };
            command.UpdateLearnerRequest.Delivery.EnglishAndMaths.First().LearningSupport.Add(lsf1);

            var sut = new LearningSupportService();

            //Currently BiL does not exist for E&M - all we have is a "PauseDate".
            //Since the BiL parameter in the below method is for the OnProg BiL, it probably needs to be removed altogether
            //to be replaced with two separate methods that can be unioned in the handler instead of in here.
            var actual = sut.GetCombinedLearningSupport([], DateTime.UtcNow, command.UpdateLearnerRequest.Delivery.EnglishAndMaths, []);

            var expected = new LearningSupportUpdatedDetails
            {
                StartDate = lsf1.StartDate,
                EndDate = command.UpdateLearnerRequest.Delivery.EnglishAndMaths.First().PauseDate!.Value
            };

            actual.Should().ContainSingle().Which.Should().BeEquivalentTo(expected);
        }

    }
}
