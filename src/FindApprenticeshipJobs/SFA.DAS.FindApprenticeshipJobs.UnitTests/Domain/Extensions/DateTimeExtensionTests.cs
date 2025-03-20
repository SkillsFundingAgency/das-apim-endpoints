using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FindApprenticeshipJobs.Domain.Extensions;

namespace SFA.DAS.FindApprenticeshipJobs.UnitTests.Domain.Extensions
{
    [TestFixture]
    public class DateTimeExtensionTests
    {
        [TestCase(-1, "Closed on {0:dddd d MMMM yyyy}")] // Past date
        [TestCase(0, "Closes today at 11:59pm")] // Today (internal)
        [TestCase(0, "Closes today", true)] // Today (external)
        [TestCase(1, "Closes tomorrow ({0:dddd d MMMM yyyy} at 11:59pm)")] // Tomorrow (internal)
        [TestCase(1, "Closes tomorrow ({0:dddd d MMMM yyyy})", true)] // Tomorrow (external)
        [TestCase(5, "Closes in 5 days ({0:dddd d MMMM yyyy} at 11:59pm)")] // Within 31 days (internal)
        [TestCase(5, "Closes in 5 days ({0:dddd d MMMM yyyy})", true)] // Within 31 days (external)
        [TestCase(31, "Closes in 31 days ({0:dddd d MMMM yyyy} at 11:59pm)")] // Exactly 31 days (internal)
        [TestCase(31, "Closes in 31 days ({0:dddd d MMMM yyyy})", true)] // Exactly 31 days (external)
        [TestCase(32, "Closes on {0:dddd d MMMM yyyy}")] // More than 31 days (internal)
        [TestCase(32, "Closes on {0:dddd d MMMM yyyy}", true)] // More than 31 days (external)
        public void GetClosingDate_ShouldReturnExpectedOutput(int daysFromToday, string expectedFormat, bool isExternalVacancy = false)
        {
            // Arrange
            var closingDate = DateTime.UtcNow.Date.AddDays(daysFromToday);
            var expectedResult = string.Format(expectedFormat, closingDate);

            // Act
            var result = DateTimeExtension.GetClosingDate(closingDate, isExternalVacancy);

            // Assert
            result.Should().Be(expectedResult);
        }
    }
}
