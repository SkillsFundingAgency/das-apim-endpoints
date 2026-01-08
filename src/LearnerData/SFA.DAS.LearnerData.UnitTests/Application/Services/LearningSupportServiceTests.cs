using AutoFixture;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.LearnerData.Requests;
using SFA.DAS.LearnerData.Services;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.LearnerData;

namespace SFA.DAS.LearnerData.UnitTests.Application.Services;

[TestFixture]
public class LearningSupportServiceTests
{
    [Test]
    public void With_LearningSupport_Pre_And_Post_Break_Then_Learning_Is_Updated_With_Merged_LSF()
    {
        // Arrange
        var testData = new TestData(hasOnProgrammeBreaks: true);

        var lsf1 = new LearningSupportRequestDetails
        {
            StartDate = testData.FirstOnProgramme.StartDate,
            EndDate = testData.FirstOnProgramme.ActualEndDate!.Value
        };

        var lsf2 = new LearningSupportRequestDetails
        {
            StartDate = testData.LatestOnProgramme.StartDate,
            EndDate = testData.LatestOnProgramme.ExpectedEndDate
        };

        testData.FirstOnProgramme.LearningSupport.Add(lsf1);
        testData.LatestOnProgramme.LearningSupport.Add(lsf2);

        var sut = new LearningSupportService();

        // Act
        var actual = sut.GetCombinedLearningSupport(testData.OnProgrammes,
            testData.OnProgrammeEndDate,
            testData.OnProgrammeBreaksInLearning,
            testData.EnglishAndMathsCourses,
            testData.EnglishAndMathsRequestedLearningSupportByLearnAimRef);

        // Assert
        actual.Count.Should().Be(2);
        actual.First().Should().BeEquivalentTo(lsf1);
        actual.Skip(1).First().Should().BeEquivalentTo(lsf2);
    }

    [Test]
    public void With_LearningSupport_Overhanging_Start_Of_Break_Then_LSF_Is_Truncated()
    {
        // Arrange
        var testData = new TestData(hasOnProgrammeBreaks: true);

        var lsf1 = new LearningSupportRequestDetails
        {
            StartDate = testData.FirstOnProgramme.StartDate,
            EndDate = testData.FirstOnProgramme.ActualEndDate!.Value.AddMonths(1)
        };
        testData.FirstOnProgramme.LearningSupport.Add(lsf1);

        var sut = new LearningSupportService();

        // Act
        var actual = sut.GetCombinedLearningSupport(
            testData.OnProgrammes,
            testData.OnProgrammeEndDate,
            testData.OnProgrammeBreaksInLearning,
            testData.EnglishAndMathsCourses,
            testData.EnglishAndMathsRequestedLearningSupportByLearnAimRef);

        // Assert
        var expected = new LearningSupportUpdatedDetails
        {
            StartDate = testData.FirstOnProgramme.StartDate,
            EndDate = testData.FirstOnProgramme.ActualEndDate.Value
        };

        actual.Should().ContainSingle().Which.Should().BeEquivalentTo(expected);
    }

    [Test]
    public void With_LearningSupport_Overhanging_End_Of_Break_Then_LSF_Is_Truncated()
    {
        // Arrange
        var testData = new TestData(hasOnProgrammeBreaks: true);

        var lsf1 = new LearningSupportRequestDetails
        {
            StartDate = testData.LatestOnProgramme.StartDate.AddMonths(-1),
            EndDate = testData.LatestOnProgramme.ExpectedEndDate
        };
        testData.LatestOnProgramme.LearningSupport.Add(lsf1);

        var sut = new LearningSupportService();

        // Act
        var actual = sut.GetCombinedLearningSupport(
            testData.OnProgrammes,
            testData.OnProgrammeEndDate,
            testData.OnProgrammeBreaksInLearning,
            testData.EnglishAndMathsCourses,
            testData.EnglishAndMathsRequestedLearningSupportByLearnAimRef);

        // Assert
        var expected = new LearningSupportUpdatedDetails
        {
            StartDate = testData.LatestOnProgramme.StartDate,
            EndDate = testData.LatestOnProgramme.ExpectedEndDate
        };

        actual.Should().ContainSingle().Which.Should().BeEquivalentTo(expected);
    }

    [Test]
    public void With_LearningSupport_Containing_Break_Then_LSF_Is_Split()
    {
        // Arrange
        var testData = new TestData(hasOnProgrammeBreaks: true);

        var lsf1 = new LearningSupportRequestDetails
        {
            StartDate = testData.FirstOnProgramme.StartDate,
            EndDate = testData.LatestOnProgramme.ExpectedEndDate
        };
        testData.FirstOnProgramme.LearningSupport.Add(lsf1);

        var sut = new LearningSupportService();

        // Act
        var actual = sut.GetCombinedLearningSupport(
            testData.OnProgrammes,
            testData.OnProgrammeEndDate,
            testData.OnProgrammeBreaksInLearning,
            testData.EnglishAndMathsCourses,
            testData.EnglishAndMathsRequestedLearningSupportByLearnAimRef);

        // Assert
        var expected = new[]
        {
            new LearningSupportUpdatedDetails { StartDate = testData.FirstOnProgramme.StartDate, EndDate = testData.FirstOnProgramme.ActualEndDate.Value },
            new LearningSupportUpdatedDetails { StartDate = testData.LatestOnProgramme.StartDate, EndDate = testData.LatestOnProgramme.ExpectedEndDate }
        };

        actual.Should().BeEquivalentTo(expected, opts => opts.WithoutStrictOrdering());
    }

    [Test]
    public void With_EnglishAndMaths_LearningSupport_Overhanging_Start_Of_Break_Then_LSF_Is_Truncated()
    {
        // Arrange
        var testData = new TestData();
        var pauseDate = testData.FirstMathsAndEnglishDetails.StartDate.AddMonths(6);
        testData.FirstMathsAndEnglishDetails.PauseDate = pauseDate;

        var lsf1 = new LearningSupportRequestDetails
        {
            StartDate = testData.FirstMathsAndEnglishDetails.StartDate,
            EndDate = pauseDate
        };

        testData.AddEnglishAndMathsLearningSupport(testData.FirstMathsAndEnglishDetails.LearnAimRef, lsf1);

        var sut = new LearningSupportService();

        // Act
        var actual = sut.GetCombinedLearningSupport(
            testData.OnProgrammes,
            testData.OnProgrammeEndDate,
            testData.OnProgrammeBreaksInLearning,
            testData.EnglishAndMathsCourses,
            testData.EnglishAndMathsRequestedLearningSupportByLearnAimRef);

        // Assert
        var expected = new LearningSupportUpdatedDetails
        {
            StartDate = lsf1.StartDate,
            EndDate = pauseDate
        };

        actual.Should().ContainSingle().Which.Should().BeEquivalentTo(expected);
    }

    [Test]
    public void With_EnglishAndMaths_LearningSupport_With_Breaks_Then_LSF_Is_Split()
    {
        var testData = new TestData(hasEnglishAndMathsBreaks: true);
        var lsf1 = new LearningSupportRequestDetails
        {
            StartDate = testData.FirstMathsAndEnglishDetails.StartDate,
            EndDate = testData.LatestMathsAndEnglishDetails.PlannedEndDate
        };

        testData.AddEnglishAndMathsLearningSupport(testData.FirstMathsAndEnglishDetails.LearnAimRef, lsf1);

        var sut = new LearningSupportService();

        // Act
        var actual = sut.GetCombinedLearningSupport(
            testData.OnProgrammes,
            testData.OnProgrammeEndDate,
            testData.OnProgrammeBreaksInLearning,
            testData.EnglishAndMathsCourses,
            testData.EnglishAndMathsRequestedLearningSupportByLearnAimRef);
        
        // Assert
        var expected = new List<LearningSupportUpdatedDetails>
        {
            new LearningSupportUpdatedDetails{
                StartDate = lsf1.StartDate,
                EndDate = testData.EnglishAndMathsPauseDate!.Value
            },
            new LearningSupportUpdatedDetails{
                StartDate = testData.EnglishAndMathsResumeDate!.Value,
                EndDate = lsf1.EndDate
            }
        };

        actual.Should().BeEquivalentTo(expected);
    }

    private class TestData
    {
        internal OnProgrammeRequestDetails FirstOnProgramme { get; set; }
        internal OnProgrammeRequestDetails LatestOnProgramme { get; set; }
        internal List<OnProgrammeRequestDetails> OnProgrammes { get; set; } = new List<OnProgrammeRequestDetails>();
        internal DateTime OnProgrammeEndDate { get; set; }
        internal List<BreakInLearning> OnProgrammeBreaksInLearning { get; set; } = new List<BreakInLearning>();

        internal MathsAndEnglishDetails FirstMathsAndEnglishDetails { get; set; }
        internal MathsAndEnglishDetails LatestMathsAndEnglishDetails { get; set; }
        
        internal DateTime? EnglishAndMathsPauseDate { get; set; }
        internal DateTime? EnglishAndMathsResumeDate { get; set; }

        internal List<MathsAndEnglishDetails> EnglishAndMathsCourses { get; set; } = new List<MathsAndEnglishDetails>();
        private List<KeyValuePair<string, List<LearningSupportRequestDetails>>> _englishAndMathsRequestedLearningSupport = new List<KeyValuePair<string, List<LearningSupportRequestDetails>>>();
        internal IEnumerable<KeyValuePair<string, List<LearningSupportRequestDetails>>> EnglishAndMathsRequestedLearningSupportByLearnAimRef => _englishAndMathsRequestedLearningSupport;

        internal TestData(bool hasOnProgrammeBreaks = false, bool hasEnglishAndMathsBreaks = false)
        {
            var fixture = new Fixture();
            var startDate = fixture.Create<DateTime>();

            // On Programme setup
            if (hasOnProgrammeBreaks)
            {
                OnProgrammeSetupWithBreaks(startDate);
            }
            else
            {
                OnProgrammes.Add(new OnProgrammeRequestDetails
                {
                    StartDate = startDate,
                    ExpectedEndDate = startDate.AddYears(2),
                    LearningSupport = []
                });
            }

            FirstOnProgramme = OnProgrammes.OrderBy(x => x.StartDate).First();
            LatestOnProgramme = OnProgrammes.OrderBy(x => x.StartDate).Last();

            var latestOnProgramme = LatestOnProgramme;
            OnProgrammeEndDate = new[]
            {
                latestOnProgramme.ExpectedEndDate,
                latestOnProgramme.CompletionDate ?? DateTime.MaxValue,
                latestOnProgramme.WithdrawalDate ?? DateTime.MaxValue,
                latestOnProgramme.PauseDate ?? DateTime.MaxValue
            }.Min();

            // English and Maths setup
            var learnAimRef = fixture.Create<string>();

            if (hasEnglishAndMathsBreaks)
            {
                EnglishAndMathsSetupWithBreaks(startDate, learnAimRef);
            }
            else
            {
                EnglishAndMathsCourses.Add(new MathsAndEnglishDetails
                {
                    StartDate = startDate,
                    PlannedEndDate = startDate.AddYears(2),
                    LearnAimRef = learnAimRef,
                    BreaksInLearning = []
                });
            }

            FirstMathsAndEnglishDetails = EnglishAndMathsCourses.OrderBy(x => x.StartDate).First();
            LatestMathsAndEnglishDetails = EnglishAndMathsCourses.OrderBy(x => x.StartDate).Last();
        }

        internal void AddEnglishAndMathsLearningSupport(string learnAimRef, LearningSupportRequestDetails lsf)
        {
            _englishAndMathsRequestedLearningSupport.Add(new KeyValuePair<string, List<LearningSupportRequestDetails>>(learnAimRef, new List<LearningSupportRequestDetails> { lsf }));
        }

        private void EnglishAndMathsSetupWithBreaks(DateTime startDate, string learnAimRef)
        {
            EnglishAndMathsPauseDate = startDate.AddMonths(6);
            EnglishAndMathsResumeDate = EnglishAndMathsPauseDate.Value.AddMonths(6);

            EnglishAndMathsCourses.Add(new MathsAndEnglishDetails
            {
                StartDate = startDate,
                PlannedEndDate = startDate.AddYears(2),
                LearnAimRef = learnAimRef,
                BreaksInLearning = [new BreakInLearning
                {
                    StartDate = EnglishAndMathsPauseDate!.Value.AddDays(1),
                    EndDate = EnglishAndMathsResumeDate!.Value.AddDays(-1)
                }]
            });

        }

        private void OnProgrammeSetupWithBreaks(DateTime startDate)
        {
            var pauseDate = startDate.AddMonths(6);
            var resumeDate = pauseDate.AddMonths(6);

            OnProgrammes.Add(new OnProgrammeRequestDetails
            {
                StartDate = startDate,
                ExpectedEndDate = startDate.AddYears(2),
                ActualEndDate = pauseDate,
                LearningSupport = []
            });

            OnProgrammes.Add(new OnProgrammeRequestDetails
            {
                StartDate = resumeDate,
                ExpectedEndDate = resumeDate.AddYears(2),
                LearningSupport = []
            });

            OnProgrammeBreaksInLearning.Add(new BreakInLearning
            {
                StartDate = pauseDate.AddDays(1),
                EndDate = resumeDate.AddDays(-1)
            });
        }
    }
}

