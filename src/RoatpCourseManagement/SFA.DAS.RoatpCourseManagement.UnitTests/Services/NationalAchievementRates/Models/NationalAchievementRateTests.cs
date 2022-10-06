using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.RoatpCourseManagement.Services.Extensions;
using SFA.DAS.RoatpCourseManagement.Services.Models;
using SFA.DAS.RoatpCourseManagement.Services.Models.ImportTypes;

namespace SFA.DAS.RoatpCourseManagement.UnitTests.Services.NationalAchievementRates.Models
{
    [TestFixture]
    public class NationalAchievementRateTests
    {
        [Test, AutoData]
        public void ImplicitOperator_ConstructsObject(NationalAchievementRateOverallCsv source)
        {
            source.Age = "16-18";
            source.ApprenticeshipLevel = "all levels";
            source.OverallCohort = "1";
            source.OverallAchievementRate = "50";

            var destination = (NationalAchievementRateOverall)source;

            destination.Age.Should().Be(source.Age.ToAge());
            destination.ApprenticeshipLevel.Should().Be(source.ApprenticeshipLevel.ToApprenticeshipLevel());
            destination.OverallCohort.Should().Be(int.Parse(source.OverallCohort));
            destination.OverallAchievementRate.Should().Be(decimal.Parse(source.OverallAchievementRate));
        }
    }
}
