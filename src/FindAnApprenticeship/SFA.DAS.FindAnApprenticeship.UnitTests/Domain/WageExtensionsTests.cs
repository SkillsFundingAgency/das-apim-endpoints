using SFA.DAS.FindAnApprenticeship.Domain;
using SFA.DAS.FindAnApprenticeship.Domain.Models;
using SFA.DAS.FindAnApprenticeship.InnerApi.RecruitApi.Responses;

namespace SFA.DAS.FindAnApprenticeship.UnitTests.Domain;

public class WageExtensionsTests
{
    [TestCase(null, 1, 0, 0, null, "£Unknown a year")]
    [TestCase(null, 1, 0, 1, null, "£Unknown a year")]
    [TestCase(null, 1, 0, 2, null, "£Unknown a year")] 
    [TestCase(null, 1, 0, 3, null, "Competitive")]
    [TestCase(20000, 1, 0, 0, null, "£20,000 a year")]
    [TestCase(null, 1, 0, 0, 37.5, "£Unknown a year")]
    [TestCase(null, 1, 0, 1, 37.5, "£(.*) a year")]
    [TestCase(null, 1, 0, 2, 37.5, "£(.*) to (.*) a year")]
    [TestCase(null, 1, 0, 3, 37.5, "Competitive")]
    [TestCase(20000, 1, 0, 0, 37.5, "£20,000 a year")]
    
    [TestCase(null, 1, 1, 0, null, "£Unknown a year")]
    [TestCase(null, 1, 1, 1, null, "£Unknown a year")]
    [TestCase(null, 1, 1, 2, null, "£Unknown a year")]
    [TestCase(null, 1, 1, 3, null, "Competitive")]
    [TestCase(20000, 1, 1, 0, null, "£20,000 a year")]
    [TestCase(null, 1, 1, 0, 37.5, "£Unknown a year")]
    [TestCase(null, 1, 1, 1, 37.5, "£(.*) a year")]
    [TestCase(null, 1, 1, 2, 37.5, "£(.*) to (.*) a year")]
    [TestCase(null, 1, 1, 3, 37.5, "Competitive")]
    [TestCase(20000, 1, 1, 0, 37.5, "£20,000 a year")]
    
    [TestCase(null, 1, 2, 0, null, "£Unknown a year")]
    [TestCase(null, 1, 2, 1, null, "£Unknown a year")]
    [TestCase(null, 1, 2, 2, null, "£Unknown a year")]
    [TestCase(null, 1, 2, 3, null, "Competitive")]
    [TestCase(20000, 1, 2, 0, null, "£20,000 a year")]
    [TestCase(null, 1, 2, 0, 37.5, "£Unknown a year")]
    [TestCase(null, 1, 2, 1, 37.5, "£(.*) a year")]
    [TestCase(null, 1, 2, 2, 37.5, "£(.*) to (.*) a year")]
    [TestCase(null, 1, 2, 3, 37.5, "Competitive")]
    [TestCase(20000, 1, 2, 0, 37.5, "£20,000 a year")]
    public void ToDisplayText_Converts_To_Expected_Value(int? amount, int duration, int durationUnit, int wageType, double? weeklyHours, string expected)
    {
        // arrange
        var wage = new Wage
        {
            Duration = duration,
            DurationUnit = (DurationUnit)durationUnit,
            FixedWageYearlyAmount = amount,
            WageType = (WageType)wageType,
            WeeklyHours = (decimal?)weeklyHours,
        };

        // act
        var actual = wage.ToDisplayText(DateTime.Today);

        // assert
        actual.Should().MatchRegex(expected);
    }
}