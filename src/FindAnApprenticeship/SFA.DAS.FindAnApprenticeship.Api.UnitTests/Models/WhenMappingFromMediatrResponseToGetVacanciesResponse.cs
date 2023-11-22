using System.Linq;
using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FindAnApprenticeship.Application.Queries.SearchApprenticeships;
using SFA.DAS.FindAnApprenticeship.InnerApi.Responses;
using SFA.DAS.Vacancies.Api.Models;

namespace SFA.DAS.FindAnApprenticeship.Api.UnitTests.Models;

public class WhenMappingFromMediatrResponseToGetVacanciesResponse
{
    [Test, AutoData]
    public void Then_The_Fields_Are_Mapped(SearchApprenticeshipsResult source)
    {
        source.Vacancies.WageType = 3;
        source.Vacancies.WageUnit = 1;

        var actual = (GetVacanciesResponse)source;

        actual.Should().BeEquivalentTo(source.Vacancies, options => options
            .Excluding(c => c.CourseLevel)
            .Excluding(c => c.CourseTitle)
            .Excluding(c => c.Route)
            .Excluding(c => c.StandardLarsCode)
            .Excluding(c => c.LongDescription)
            .Excluding(c => c.Qualifications)
            .Excluding(c => c.FrameworkLarsCode)
            .Excluding(c => c.WageAmountLowerBound)
            .Excluding(c => c.WageAmountUpperBound)
            .Excluding(c => c.WageText)
            .Excluding(c => c.WageType)
            .Excluding(c => c.WageAmount)
            .Excluding(c => c.WageUnit)
            .Excluding(c => c.Id)
            .Excluding(c => c.AnonymousEmployerName)
            .Excluding(c => c.Category)
            .Excluding(c => c.CategoryCode)
            .Excluding(c => c.IsEmployerAnonymous)
            .Excluding(c => c.SubCategory)
            .Excluding(c => c.SubCategoryCode)
            .Excluding(c => c.VacancyLocationType)
            .Excluding(c => c.WorkingWeek)
            .Excluding(c => c.Score)
            .Excluding(c => c.IsPositiveAboutDisability)
        );
        actual.FullDescription.Should().Be(source.Vacancies.LongDescription);
        actual.Qualifications.Should().BeEquivalentTo(source.Vacancies.Qualifications.Select(c => (GetVacancyQualification)c).ToList());
        actual.Location.Lat.Should().Be(source.Vacancies.Location.Lat);
        actual.Location.Lon.Should().Be(source.Vacancies.Location.Lon);
        actual.Course.Level.Should().Be(source.Vacancies.CourseLevel);
        actual.Course.Title.Should().Be($"{source.Vacancies.CourseTitle} (level {source.Vacancies.CourseLevel})");
        actual.Course.Route.Should().Be(source.Vacancies.Route);
        actual.Course.LarsCode.Should().Be(source.Vacancies.StandardLarsCode);
        actual.Wage.WageAmount.Should().Be(source.Vacancies.WageAmount);
        actual.Wage.WageType.Should().Be((WageType)source.Vacancies.WageType);
        actual.Wage.WageUnit.Should().Be((WageUnit)source.Vacancies.WageUnit);
        actual.Wage.WageAdditionalInformation.Should().Be(source.Vacancies.WageText);
        actual.Wage.WageAmountLowerBound.Should().Be(source.Vacancies.WageAmountLowerBound);
        actual.Wage.WageAmountUpperBound.Should().Be(source.Vacancies.WageAmountUpperBound);
    }
}