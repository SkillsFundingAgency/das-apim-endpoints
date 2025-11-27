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
    public class LearningSupportServiceTests
    {
        private readonly Fixture _fixture = new Fixture();

        [Test]
        public void With_LearningSupport_Pre_And_Post_Break_Then_Learning_Is_Updated_With_Merged_LSF()
        {
            // Arrange
            var command = CreateLearnerWithBreaksInLearning(false);
            var preBreak = command.UpdateLearnerRequest.Delivery.OnProgramme.First();
            var postBreak = command.UpdateLearnerRequest.Delivery.OnProgramme.Skip(1).First();

            var lsf1 = new LearningSupportRequestDetails
            {
                StartDate = preBreak.StartDate,
                EndDate = preBreak.ActualEndDate.Value
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
                command.UpdateLearnerRequest.Delivery.EnglishAndMaths,
                []);

            // Assert
            actual.Count().Should().Be(2);
            actual.First().Should().BeEquivalentTo(lsf1);
            actual.Skip(1).First().Should().BeEquivalentTo(lsf2);
        }

        [Test]
        public void With_LearningSupport_Overhanging_Start_Of_Break_Then_LSF_Is_Truncated()
        {
            var command = CreateLearnerWithBreaksInLearning(false);
            var preBreak = command.UpdateLearnerRequest.Delivery.OnProgramme.First();
            var postBreak = command.UpdateLearnerRequest.Delivery.OnProgramme.Skip(1).First();

            var lsf1 = new LearningSupportRequestDetails
            {
                StartDate = preBreak.StartDate,
                EndDate = preBreak.ActualEndDate.Value.AddMonths(1)
            };
            preBreak.LearningSupport.Add(lsf1);

            var breaks = new List<BreakInLearning>
            {
                new BreakInLearning { StartDate = preBreak.ActualEndDate.Value.AddDays(1), EndDate = postBreak.StartDate.AddDays(-1) }
            };

            var sut = new LearningSupportService();

            var actual = sut.GetCombinedLearningSupport([preBreak], command.UpdateLearnerRequest.Delivery.EnglishAndMaths, breaks);

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
            var command = CreateLearnerWithBreaksInLearning(false);
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
                new BreakInLearning { StartDate = preBreak.ActualEndDate.Value.AddDays(1), EndDate = postBreak.StartDate.AddDays(-1) }
            };

            var sut = new LearningSupportService();

            var actual = sut.GetCombinedLearningSupport([postBreak], command.UpdateLearnerRequest.Delivery.EnglishAndMaths, breaks);

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
            var command = CreateLearnerWithBreaksInLearning(false);
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
                new BreakInLearning { StartDate = preBreak.ActualEndDate.Value.AddDays(1), EndDate = postBreak.StartDate.AddDays(-1) }
            };

            var sut = new LearningSupportService();

            var actual = sut.GetCombinedLearningSupport([preBreak, postBreak], command.UpdateLearnerRequest.Delivery.EnglishAndMaths, breaks);

            var expected = new[]
            {
                new LearningSupportUpdatedDetails { StartDate = preBreak.StartDate, EndDate = preBreak.ActualEndDate.Value },
                new LearningSupportUpdatedDetails { StartDate = postBreak.StartDate, EndDate = postBreak.ExpectedEndDate }
            };

            actual.Should().BeEquivalentTo(expected, opts => opts.WithoutStrictOrdering());
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
