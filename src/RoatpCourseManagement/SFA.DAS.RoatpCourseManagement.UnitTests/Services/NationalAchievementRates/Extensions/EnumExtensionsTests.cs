using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.RoatpCourseManagement.Services.Extensions;
using SFA.DAS.RoatpCourseManagement.Services.Models;

namespace SFA.DAS.RoatpCourseManagement.UnitTests.Services.NationalAchievementRates.Models
{
    [TestFixture]
    public class EnumExtensionsTests
    {
        [TestCase("16-18", Age.SixteenToEighteen)]
        [TestCase("19-23", Age.NineteenToTwentyThree)]
        [TestCase("24+", Age.TwentyFourPlus)]
        [TestCase("all age", Age.AllAges)]
        [TestCase("15", Age.Unknown)]
        public void ToAge_ReturnsEnum(string value, Age age)
        {
            var result = EnumExtensions.ToAge(value);

            result.Should().Be(age);
        }

        [TestCase("2", ApprenticeshipLevel.Two)]
        [TestCase("3", ApprenticeshipLevel.Three)]
        [TestCase("4+", ApprenticeshipLevel.FourPlus)]
        [TestCase("all levels", ApprenticeshipLevel.AllLevels)]
        [TestCase("1", ApprenticeshipLevel.Unknown)]
        public void ToAge_ReturnsEnum(string value, ApprenticeshipLevel apprenticeshipLevel)
        {
            var result = EnumExtensions.ToApprenticeshipLevel(value);

            result.Should().Be(apprenticeshipLevel);
        }
    }
}
