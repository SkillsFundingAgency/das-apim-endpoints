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

    [Test, MoqAutoData]
    public void Then_Locations_Are_Not_Compared_On_Certain_Fields(
        [Greedy] VacancyComparerService sut)
    {
        // arrange
        var left = VacancyUtils.CreateVacancy();
        var right = VacancyUtils.CreateVacancy();

        right.EmployerLocations![0].Latitude = 1.0d;
        right.EmployerLocations[0].Longitude = 1.0d;
        right.EmployerLocations[0].Country = "Somewhere";

        // act
        var results = sut.Compare(left, right);

        // assert
        results.Fields.Single(f => f.FieldName == FieldIdResolver.ToFieldId(v => v.EmployerLocations)).AreEqual.Should().BeTrue();
    }
    
    [Test, MoqAutoData]
    public void Then_The_Order_Of_Addresses_Does_Not_Matter(
        Vacancy left,
        Vacancy right,
        [Greedy] VacancyComparerService sut)
    {
        // arrange
        right.EmployerLocations = Enumerable.Reverse(left.EmployerLocations!).ToList();
        
        // act
        var results = sut.Compare(left, right);

        // assert
        results.Fields.Single(f => f.FieldName == FieldIdResolver.ToFieldId(v => v.EmployerLocations)).AreEqual.Should().BeTrue();
    }
}