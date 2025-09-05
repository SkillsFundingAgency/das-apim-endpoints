using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.Vacancies.Api.Models;
using SFA.DAS.Vacancies.Application.Vacancies.Queries.GetVacancy;
using System;
using System.Linq;

namespace SFA.DAS.Vacancies.Api.UnitTests.Models
{
    public class WhenMappingFromMediatorResponseToGetVacancyResponseV2
    {
        [Test, AutoData]
        public void Then_The_Fields_Are_Mapped(GetVacancyQueryResult source, int ukprn)
        {
            source.Vacancy.Ukprn = ukprn.ToString();
            source.Vacancy.WageType = 3;
            source.Vacancy.WageUnit = 1;
            source.Vacancy.VacancyReference = $"VAC{source.Vacancy.VacancyReference}";
            
            var actual = (GetVacancyResponseV2)source;
            
            actual.Should().BeEquivalentTo(source.Vacancy, options => options
                .Excluding(c=>c.CourseLevel)
                .Excluding(c=>c.CourseTitle)
                .Excluding(c=>c.Route)
                .Excluding(c=>c.StandardLarsCode)
                .Excluding(c=>c.LongDescription)
                .Excluding(c=>c.Qualifications)
                .Excluding(c=>c.FrameworkLarsCode)
                .Excluding(c=>c.WageAmountLowerBound)
                .Excluding(c=>c.WageAmountUpperBound)
                .Excluding(c=>c.WageText)
                .Excluding(c=>c.WageType)
                .Excluding(c=>c.WageAmount)
                .Excluding(c=>c.WageUnit)
                .Excluding(c=>c.Id)
                .Excluding(c=>c.AnonymousEmployerName)
                .Excluding(c=>c.Category)
                .Excluding(c=>c.CategoryCode)
                .Excluding(c=>c.IsEmployerAnonymous)
                .Excluding(c=>c.SubCategory)
                .Excluding(c=>c.SubCategoryCode)
                .Excluding(c=>c.VacancyLocationType)
                .Excluding(c=>c.WorkingWeek)
                .Excluding(c=>c.IsPositiveAboutDisability)
                .Excluding(c => c.ClosingDate)
                .Excluding(item => item.Ukprn)
                .Excluding(item => item.VacancyReference)
                .Excluding(item => item.VacancySource)
                .Excluding(item => item.Location)
                .Excluding(c => c.ApprenticeshipType)
                .Excluding(item => item.EmploymentLocationInformation)
                .Excluding(item => item.Address)
                .Excluding(item => item.OtherAddresses)
            );
            actual.FullDescription.Should().Be(source.Vacancy.LongDescription);
            actual.Qualifications.Should().BeEquivalentTo(source.Vacancy.Qualifications.Select(c=>(GetVacancyQualification)c).ToList());
            actual.Course.Level.Should().Be(source.Vacancy.CourseLevel);
            actual.Course.Title.Should().Be($"{source.Vacancy.CourseTitle} (level {source.Vacancy.CourseLevel})");
            actual.Course.Route.Should().Be(source.Vacancy.Route);
            actual.Course.LarsCode.Should().Be(source.Vacancy.StandardLarsCode);
            actual.Wage.WageAmount.Should().Be(source.Vacancy.WageAmount);
            actual.Wage.WageType.Should().Be((WageType)source.Vacancy.WageType);
            actual.Wage.WageUnit.Should().Be((WageUnit)source.Vacancy.WageUnit);
            actual.Wage.WageAdditionalInformation.Should().Be(source.Vacancy.WageText);
            actual.Ukprn.Should().Be(ukprn);
            actual.VacancyReference.Should().Be(source.Vacancy.VacancyReference.TrimVacancyReference());
            actual.ClosingDate.Should().Be(source.Vacancy.ClosingDate.AddDays(1).Subtract(TimeSpan.FromSeconds(1)));
            actual.ApplicationUrl.Should().Be(source.Vacancy.ApplicationUrl);
            actual.IsNationalVacancy.Should().Be(source.Vacancy.VacancyLocationType.Equals("National", StringComparison.CurrentCultureIgnoreCase));
            actual.IsNationalVacancyDetails.Should().Be(source.Vacancy.VacancyLocationType.Equals("National", StringComparison.CurrentCultureIgnoreCase) ? source.Vacancy.EmploymentLocationInformation : string.Empty);
        }

        [Test, AutoData]
        public void Then_If_Anonymous_Then_Anon_Values_Mapped(GetVacancyQueryResult source, int ukprn)
        {
            source.Vacancy.Ukprn = ukprn.ToString();
            source.Vacancy.IsEmployerAnonymous = true;
            source.Vacancy.VacancyLocationType = "nATIonal";
            
            var actual = (GetVacancyResponseV2)source;
            
            actual.Should().BeEquivalentTo(source.Vacancy,options => options
                .ExcludingMissingMembers()
                .Excluding(item => item.EmployerName)
                .Excluding(item => item.CourseTitle)
                .Excluding(item => item.CourseLevel)
                .Excluding(item => item.Location)
                .Excluding(item => item.Ukprn)
                .Excluding(item => item.ClosingDate)
                .Excluding(item => item.VacancyReference)
            );
            actual.EmployerName.Should().Be(source.Vacancy.AnonymousEmployerName);
            actual.VacancyReference.Should().Be(source.Vacancy.VacancyReference.TrimVacancyReference());
        }


        [Test, AutoData]
        public void Then_If_Null_Then_Null_Returned(GetVacancyQueryResult source)
        {
            source.Vacancy = null;
            
            var actual = (GetVacancyResponseV2)source;

            actual.Should().BeNull();
        }
    }
}