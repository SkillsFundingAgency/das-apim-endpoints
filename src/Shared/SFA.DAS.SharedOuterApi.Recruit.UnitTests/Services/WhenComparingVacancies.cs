using SFA.DAS.SharedOuterApi.Recruit.Services;
using SFA.DAS.SharedOuterApi.Types.Domain.Recruit;

namespace SFA.DAS.SharedOuterApi.Recruit.UnitTests.Services;

public class WhenComparingVacancies
{
    private static IEnumerable<object[]> TestCases => new List<object[]>
    {
        new object[] {VacancyUtils.CreateVacancy(), VacancyUtils.CreateEmptyVacancy(), false},
        new object[] {VacancyUtils.CreateVacancy(), VacancyUtils.CreateChangedVacancy(), false},
        new object[] {VacancyUtils.CreateVacancy(), VacancyUtils.CreateVacancy(), true}
    };
    
    [TestCaseSource(nameof(TestCases))]
    public void ShouldCompare(Vacancy a, Vacancy b, bool expectedAreEqual)
    {
        var sut = new VacancyComparerService();

        var result = sut.Compare(a, b);

        result.Fields.Single(f => f.FieldName == FieldIdResolver.ToFieldId(v => v.VacancyReference)).AreEqual.Should().Be(expectedAreEqual);
        result.Fields.Single(f => f.FieldName == FieldIdResolver.ToFieldId(v => v.AccountId)).AreEqual.Should().Be(expectedAreEqual);
        result.Fields.Single(f => f.FieldName == FieldIdResolver.ToFieldId(v => v.ApplicationInstructions)).AreEqual.Should().Be(expectedAreEqual);
        result.Fields.Single(f => f.FieldName == FieldIdResolver.ToFieldId(v => v.ApplicationMethod)).AreEqual.Should().Be(expectedAreEqual);
        result.Fields.Single(f => f.FieldName == FieldIdResolver.ToFieldId(v => v.ApplicationUrl)).AreEqual.Should().Be(expectedAreEqual);
        result.Fields.Single(f => f.FieldName == FieldIdResolver.ToFieldId(v => v.ClosingDate)).AreEqual.Should().Be(expectedAreEqual);
        result.Fields.Single(f => f.FieldName == FieldIdResolver.ToFieldId(v => v.Description)).AreEqual.Should().Be(expectedAreEqual);
        result.Fields.Single(f => f.FieldName == FieldIdResolver.ToFieldId(v => v.DisabilityConfident)).AreEqual.Should().Be(expectedAreEqual);
        result.Fields.Single(f => f.FieldName == FieldIdResolver.ToFieldId(v => v.Contact.Email)).AreEqual.Should().Be(expectedAreEqual);
        result.Fields.Single(f => f.FieldName == FieldIdResolver.ToFieldId(v => v.Contact.Name)).AreEqual.Should().Be(expectedAreEqual);
        result.Fields.Single(f => f.FieldName == FieldIdResolver.ToFieldId(v => v.Contact.Phone)).AreEqual.Should().Be(expectedAreEqual);
        result.Fields.Single(f => f.FieldName == FieldIdResolver.ToFieldId(v => v.EmployerDescription)).AreEqual.Should().Be(expectedAreEqual);
        result.Fields.Single(f => f.FieldName == FieldIdResolver.ToFieldId(v => v.EmployerLocations)).AreEqual.Should().Be(expectedAreEqual);
        result.Fields.Single(f => f.FieldName == FieldIdResolver.ToFieldId(v => v.EmployerLocationInformation)).AreEqual.Should().Be(expectedAreEqual);
        result.Fields.Single(f => f.FieldName == FieldIdResolver.ToFieldId(v => v.EmployerName)).AreEqual.Should().Be(expectedAreEqual);
        result.Fields.Single(f => f.FieldName == FieldIdResolver.ToFieldId(v => v.EmployerWebsiteUrl)).AreEqual.Should().Be(expectedAreEqual);
        result.Fields.Single(f => f.FieldName == FieldIdResolver.ToFieldId(v => v.NumberOfPositions)).AreEqual.Should().Be(expectedAreEqual);
        result.Fields.Single(f => f.FieldName == FieldIdResolver.ToFieldId(v => v.OutcomeDescription)).AreEqual.Should().Be(expectedAreEqual);
        result.Fields.Single(f => f.FieldName == FieldIdResolver.ToFieldId(v => v.ProgrammeId)).AreEqual.Should().Be(expectedAreEqual);
        result.Fields.Single(f => f.FieldName == FieldIdResolver.ToFieldId(v => v.ShortDescription)).AreEqual.Should().Be(expectedAreEqual);
        result.Fields.Single(f => f.FieldName == FieldIdResolver.ToFieldId(v => v.StartDate)).AreEqual.Should().Be(expectedAreEqual);
        result.Fields.Single(f => f.FieldName == FieldIdResolver.ToFieldId(v => v.ThingsToConsider)).AreEqual.Should().Be(expectedAreEqual);
        result.Fields.Single(f => f.FieldName == FieldIdResolver.ToFieldId(v => v.Title)).AreEqual.Should().Be(expectedAreEqual);
        result.Fields.Single(f => f.FieldName == FieldIdResolver.ToFieldId(v => v.TrainingDescription)).AreEqual.Should().Be(expectedAreEqual);
        result.Fields.Single(f => f.FieldName == FieldIdResolver.ToFieldId(v => v.TrainingProvider.Ukprn)).AreEqual.Should().Be(expectedAreEqual);
        result.Fields.Single(f => f.FieldName == FieldIdResolver.ToFieldId(v => v.Wage.WeeklyHours)).AreEqual.Should().Be(expectedAreEqual);
        result.Fields.Single(f => f.FieldName == FieldIdResolver.ToFieldId(v => v.Wage.WorkingWeekDescription)).AreEqual.Should().Be(expectedAreEqual);
        result.Fields.Single(f => f.FieldName == FieldIdResolver.ToFieldId(v => v.Wage.WageAdditionalInformation)).AreEqual.Should().Be(expectedAreEqual);
        result.Fields.Single(f => f.FieldName == FieldIdResolver.ToFieldId(v => v.Wage.WageType)).AreEqual.Should().Be(expectedAreEqual);
        result.Fields.Single(f => f.FieldName == FieldIdResolver.ToFieldId(v => v.Wage.FixedWageYearlyAmount)).AreEqual.Should().Be(expectedAreEqual);
        result.Fields.Single(f => f.FieldName == FieldIdResolver.ToFieldId(v => v.Wage.Duration)).AreEqual.Should().Be(expectedAreEqual);
        result.Fields.Single(f => f.FieldName == FieldIdResolver.ToFieldId(v => v.AdditionalQuestion1)).AreEqual.Should().Be(expectedAreEqual);
        result.Fields.Single(f => f.FieldName == FieldIdResolver.ToFieldId(v => v.AdditionalQuestion2)).AreEqual.Should().Be(expectedAreEqual);
    }
}