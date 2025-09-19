using SFA.DAS.Recruit.Domain.Vacancy;
using SFA.DAS.Recruit.Extensions;

namespace SFA.DAS.Recruit.UnitTests.Extensions;
[TestFixture]
internal class WageExtensionsTests
{
    [Test]
    public void GetDuration_ReturnsEmptyString_WhenWageIsNull()
    {
        Wage? wage = null;
        var result = wage.GetDuration();
        result.Should().BeNullOrEmpty();
    }

    [Test]
    [MoqInlineAutoData(1, DurationUnit.Year, "1 year")]
    [MoqInlineAutoData(2, DurationUnit.Year, "2 years")]
    [MoqInlineAutoData(1, DurationUnit.Week, "1 week")]
    [MoqInlineAutoData(3, DurationUnit.Week, "3 weeks")]
    public void GetDuration_ReturnsCorrectYearOrWeekDuration(int duration, DurationUnit unit, string expected)
    {
        var wage = new Wage { Duration = duration, DurationUnit = unit };
        var result = wage.GetDuration();
        result.Should().Be(expected);
    }

    [Test]
    [MoqInlineAutoData(1, "1 month")]
    [MoqInlineAutoData(12, "1 year")]
    [MoqInlineAutoData(13, "1 year 1 month")]
    [MoqInlineAutoData(24, "2 years")]
    [MoqInlineAutoData(25, "2 years 1 month")]
    [MoqInlineAutoData(0, "")]
    public void GetDuration_ReturnsCorrectMonthDuration(int months, string expected)
    {
        var wage = new Wage { Duration = months, DurationUnit = DurationUnit.Month };
        var result = wage.GetDuration();
        result.Should().Be(expected);
    }

    [Test]
    public void GetDuration_ReturnsEmptyString_WhenDurationUnitIsUnknown()
    {
        var wage = new Wage { Duration = 5, DurationUnit = null };
        var result = wage.GetDuration();
        result.Should().Be(string.Empty);
    }

    [Test]
    public void GetDuration_ReturnsEmptyString_WhenDurationIsNull()
    {
        var wage = new Wage { Duration = null, DurationUnit = DurationUnit.Year };
        var result = wage.GetDuration();
        result.Should().Be(" years");
    }
}
