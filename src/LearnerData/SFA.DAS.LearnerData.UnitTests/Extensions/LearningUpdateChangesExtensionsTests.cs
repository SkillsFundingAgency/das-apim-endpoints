using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.LearnerData;
using SFA.DAS.LearnerData.Extensions;

namespace SFA.DAS.LearnerData.UnitTests.Extensions
{
    [TestFixture]
    public class LearningUpdateChangesExtensionsTests
    {
        [TestCase(UpdateLearnerApiPutResponse.LearningUpdateChanges.ExpectedEndDate)]
        [TestCase(UpdateLearnerApiPutResponse.LearningUpdateChanges.CompletionDate)]
        [TestCase(UpdateLearnerApiPutResponse.LearningUpdateChanges.Withdrawal)]
        [TestCase(UpdateLearnerApiPutResponse.LearningUpdateChanges.ReverseWithdrawal)]
        [TestCase(UpdateLearnerApiPutResponse.LearningUpdateChanges.BreakInLearningStarted)]
        [TestCase(UpdateLearnerApiPutResponse.LearningUpdateChanges.BreakInLearningRemoved)]
        [TestCase(UpdateLearnerApiPutResponse.LearningUpdateChanges.BreaksInLearningUpdated)]
        [TestCase(UpdateLearnerApiPutResponse.LearningUpdateChanges.Prices)]
        [TestCase(UpdateLearnerApiPutResponse.LearningUpdateChanges.DateOfBirthChanged)]
        public void HasOnProgrammeUpdate_ReturnsTrue_ForOnProgrammeChanges(UpdateLearnerApiPutResponse.LearningUpdateChanges change)
        {
            var changes = new List<UpdateLearnerApiPutResponse.LearningUpdateChanges> { change };

            changes.HasOnProgrammeUpdate().Should().BeTrue();
        }

        [Test]
        public void HasOnProgrammeUpdate_ReturnsFalse_WhenNoRelevantChanges()
        {
            var changes = new List<UpdateLearnerApiPutResponse.LearningUpdateChanges>
            {
                UpdateLearnerApiPutResponse.LearningUpdateChanges.MathsAndEnglish
            };

            changes.HasOnProgrammeUpdate().Should().BeFalse();
        }

        [TestCase(UpdateLearnerApiPutResponse.LearningUpdateChanges.MathsAndEnglish)]
        [TestCase(UpdateLearnerApiPutResponse.LearningUpdateChanges.MathsAndEnglishWithdrawal)]
        public void HasEnglishAndMathsUpdate_ReturnsTrue_ForEnglishAndMathsChanges(UpdateLearnerApiPutResponse.LearningUpdateChanges change)
        {
            var changes = new List<UpdateLearnerApiPutResponse.LearningUpdateChanges> { change };

            changes.HasEnglishAndMathsUpdate().Should().BeTrue();
        }

        [Test]
        public void HasEnglishAndMathsUpdate_ReturnsFalse_WhenNoRelevantChanges()
        {
            var changes = new List<UpdateLearnerApiPutResponse.LearningUpdateChanges>
            {
                UpdateLearnerApiPutResponse.LearningUpdateChanges.ExpectedEndDate
            };

            changes.HasEnglishAndMathsUpdate().Should().BeFalse();
        }

        [Test]
        public void HasLearningSupportUpdate_ReturnsTrue_WhenLearningSupportChangePresent()
        {
            var changes = new List<UpdateLearnerApiPutResponse.LearningUpdateChanges>
            {
                UpdateLearnerApiPutResponse.LearningUpdateChanges.LearningSupport
            };

            changes.HasLearningSupportUpdate().Should().BeTrue();
        }

        [Test]
        public void HasLearningSupportUpdate_ReturnsFalse_WhenNoRelevantChanges()
        {
            var changes = new List<UpdateLearnerApiPutResponse.LearningUpdateChanges>
            {
                UpdateLearnerApiPutResponse.LearningUpdateChanges.ExpectedEndDate
            };

            changes.HasLearningSupportUpdate().Should().BeFalse();
        }

        [Test]
        public void HasPersonalDetailsOnly_ReturnsTrue_WhenOnlyPersonalDetailsPresent()
        {
            var changes = new List<UpdateLearnerApiPutResponse.LearningUpdateChanges>
            {
                UpdateLearnerApiPutResponse.LearningUpdateChanges.PersonalDetails
            };

            changes.HasPersonalDetailsOnly().Should().BeTrue();
        }

        [Test]
        public void HasPersonalDetailsOnly_ReturnsFalse_WhenNoChangesPresent()
        {
            var changes = new List<UpdateLearnerApiPutResponse.LearningUpdateChanges>();

            changes.HasPersonalDetailsOnly().Should().BeFalse();
        }

        [Test]
        public void HasPersonalDetailsOnly_ReturnsFalse_WhenPersonalDetailsPlusOtherChangePresent()
        {
            var changes = new List<UpdateLearnerApiPutResponse.LearningUpdateChanges>
            {
                UpdateLearnerApiPutResponse.LearningUpdateChanges.PersonalDetails,
                UpdateLearnerApiPutResponse.LearningUpdateChanges.ExpectedEndDate
            };

            changes.HasPersonalDetailsOnly().Should().BeFalse();
        }
    }
}