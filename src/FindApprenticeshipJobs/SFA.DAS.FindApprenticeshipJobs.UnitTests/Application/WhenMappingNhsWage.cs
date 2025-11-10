using SFA.DAS.FindApprenticeshipJobs.Domain.Models;
using SFA.DAS.FindApprenticeshipJobs.Services;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FindApprenticeshipJobs.UnitTests.Application
{
    [TestFixture]
    public class WhenMappingNhsWage
    {
        [Test]
        [MoqInlineAutoData("£26530.00 to £29114.00", "21890.23")]
        public void GetWage_ShouldReturnCorrectWage_ForRangeInput(string wageText, string expected)
        {

            // Act
            var result = LiveVacancyMapper.GetNhsJobsWage(wageText);

            // Assert
            result.WageType.Should().Be(WageType.FixedWage);
            result.WageText.Should().Be(wageText);
            result.ApprenticeMinimumWage.Should().Be(26530.00M);
            result.Under18NationalMinimumWage.Should().Be(26530.00M);
            result.Between18AndUnder21NationalMinimumWage.Should().Be(Convert.ToDecimal(expected));
            result.Between21AndUnder25NationalMinimumWage.Should().Be(29114.00M);
            result.Over25NationalMinimumWage.Should().Be(29114.00M);
        }

        [Test]
        [MoqInlineAutoData("£30000")]
        public void GetWage_ShouldReturnCorrectWage_ForFixedWageInput(string wageText)
        {
            // Act
            var result = LiveVacancyMapper.GetNhsJobsWage(wageText);

            // Assert
            result.WageType.Should().Be(WageType.FixedWage);
            result.WageText.Should().Be(wageText);
            result.ApprenticeMinimumWage.Should().Be(30000M);
            result.Under18NationalMinimumWage.Should().Be(30000M);
            result.Between18AndUnder21NationalMinimumWage.Should().Be(30000M);
            result.Between21AndUnder25NationalMinimumWage.Should().Be(30000M);
            result.Over25NationalMinimumWage.Should().Be(30000M);
        }

        [Test]
        [MoqInlineAutoData("Competitive")]
        [MoqInlineAutoData("Depends on experience")]
        public void GetWage_ShouldReturnCompetitiveSalary_ForInvalidInput(string wageText)
        {

            // Act
            var result = LiveVacancyMapper.GetNhsJobsWage(wageText);

            // Assert
            result.WageType.Should().Be(WageType.CompetitiveSalary);
            result.WageText.Should().Be(wageText);
        }
    }
}
