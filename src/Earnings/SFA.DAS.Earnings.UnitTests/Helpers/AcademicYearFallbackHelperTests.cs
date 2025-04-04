using FluentAssertions;
using SFA.DAS.Earnings.Helpers;

namespace SFA.DAS.Earnings.UnitTests.Helpers
{
    [TestFixture]
    public class AcademicYearFallbackHelperTests
    {
        [Test]
        [TestCase(2526, "2526", "2025-08-01", "2026-07-31")]
        [TestCase(3031, "3031", "2030-08-01", "2031-07-31")]
        [TestCase(1920, "1920", "2019-08-01", "2020-07-31")]
        public void GetFallbackAcademicYearResponse_Should_Return_Correct_AcademicYear_Dates(
            int inputAcademicYear,
            string expectedAcademicYear,
            string expectedStartDate,
            string expectedEndDate)
        {
            var result = AcademicYearFallbackHelper.GetFallbackAcademicYearResponse(inputAcademicYear);

            result.Should().NotBeNull();
            result.AcademicYear.Should().Be(expectedAcademicYear);
            result.StartDate.Should().Be(DateTime.Parse(expectedStartDate));
            result.EndDate.Should().Be(DateTime.Parse(expectedEndDate));
            result.HardCloseDate.Should().BeNull(); // Ensure HardCloseDate is always null
        }

        [Test]
        public void GetFallbackAcademicYearResponse_Should_Throw_Exception_For_Invalid_Input()
        {
            var invalidInput = 2; //Not a valid a/y
            Action act = () => AcademicYearFallbackHelper.GetFallbackAcademicYearResponse(invalidInput);
            act.Should().Throw<ArgumentOutOfRangeException>();
        }
    }
}