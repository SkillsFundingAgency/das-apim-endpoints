using FluentAssertions;
using SFA.DAS.Earnings.Application.Extensions;

namespace SFA.DAS.Earnings.UnitTests.Application.Extensions
{
    [TestFixture]
    public class DateTimeExtensionsTests
    {
        [Test]
        public void GetNumberOfIncludedCensusDatesUntil_ShouldReturnCorrectCount()
        {
            // Arrange
            var start = new DateTime(2023, 1, 1);
            var end = new DateTime(2023, 12, 31);

            // Act
            var result = start.GetNumberOfIncludedCensusDatesUntil(end);

            // Assert
            result.Should().Be(12);
        }

        [Test]
        public void GetCensusDateForCollectionPeriod_ShouldReturnCorrectDate_ForFirstHalfOfYear()
        {
            // Arrange
            var academicYear = "2324";
            byte collectionPeriod = 5;

            // Act
            var result = academicYear.GetCensusDateForCollectionPeriod(collectionPeriod);

            // Assert
            result.Should().Be(new DateTime(2023, 12, 31));
        }

        [Test]
        public void GetCensusDateForCollectionPeriod_ShouldReturnCorrectDate_ForSecondHalfOfYear()
        {
            // Arrange
            var academicYear = "2324";
            byte collectionPeriod = 7;

            // Act
            var result = academicYear.GetCensusDateForCollectionPeriod(collectionPeriod);

            // Assert
            result.Should().Be(new DateTime(2024, 2, 29));// 2024 is a leap year
        }
    }
}
