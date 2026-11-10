using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.Apim.Shared.Extensions;
using SFA.DAS.Vacancies.Api.Models;
using SFA.DAS.Vacancies.Application.Vacancies.Queries.GetVacancy;
using System;
using System.Linq;

namespace SFA.DAS.Vacancies.Api.UnitTests.Models
{
    public class WhenMappingFromMediatorResponseToGetVacancyResponse
    {
        [Test, AutoData]

        public void Then_The_Fields_Are_Mapped(GetVacancyQueryResult source, int ukprn)
        {
            source.Vacancy.Ukprn = ukprn.ToString();
            source.Vacancy.WageType = 3;
            source.Vacancy.WageUnit = 1;
            source.Vacancy.VacancyReference = $"VAC{source.Vacancy.VacancyReference}";
            
            var actual = (GetVacancyResponse)source;
            
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
                .Excluding(c => c.ApprenticeshipType)
                .Excluding(item => item.Ukprn)
                .Excluding(item => item.VacancyReference)
                .Excluding(item => item.VacancySource)
                .Excluding(item => item.ApplicationUrl)
                .Excluding(item => item.EmploymentLocationInformation)
                .Excluding(item => item.EmployerContactEmail)
                .Excluding(item => item.EmployerContactName)
                .Excluding(item => item.EmployerContactPhone)
                .Excluding(item => item.ProviderContactEmail)
                .Excluding(item => item.ProviderContactName)
                .Excluding(item => item.ProviderContactPhone)
            );
            actual.FullDescription.Should().Be(source.Vacancy.LongDescription);
            actual.Qualifications.Should().BeEquivalentTo(source.Vacancy.Qualifications.Select(c=>(GetVacancyQualification)c).ToList());
            actual.Location.Lat.Should().Be(source.Vacancy.Location.Lat);
            actual.Location.Lon.Should().Be(source.Vacancy.Location.Lon);
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
            actual.IsNationalVacancy.Should().Be(source.Vacancy.VacancyLocationType.Equals("National", StringComparison.CurrentCultureIgnoreCase));
            actual.IsNationalVacancyDetails.Should().Be(source.Vacancy.VacancyLocationType.Equals("National", StringComparison.CurrentCultureIgnoreCase) ? source.Vacancy.EmploymentLocationInformation : string.Empty);
        }
        
        [Test, AutoData]
        public void Then_The_Employer_Contact_Details_Are_Mapped_Correctly(GetVacancyQueryResult source, int ukprn)
        {
            // arrange
            source.Vacancy.Ukprn = ukprn.ToString();
            source.Vacancy.VacancyReference = $"VAC{source.Vacancy.VacancyReference}";

            source.Vacancy.ProviderContactEmail = null;
            source.Vacancy.ProviderContactName = null;
            source.Vacancy.ProviderContactPhone = null;
            
            // act
            var actual = (GetVacancyResponse)source;
            
            // assert
            actual.EmployerContactEmail.Should().Be(source.Vacancy.EmployerContactEmail);
            actual.EmployerContactName.Should().Be(source.Vacancy.EmployerContactName);
            actual.EmployerContactPhone.Should().Be(source.Vacancy.EmployerContactPhone);
        }
        
        [Test, AutoData]
        public void Then_The_Provider_Contact_Details_Are_Mapped_Correctly(GetVacancyQueryResult source, int ukprn)
        {
            // arrange
            source.Vacancy.Ukprn = ukprn.ToString();
            source.Vacancy.VacancyReference = $"VAC{source.Vacancy.VacancyReference}";

            source.Vacancy.EmployerContactEmail = null;
            source.Vacancy.EmployerContactName = null;
            source.Vacancy.EmployerContactPhone = null;
            
            // act
            var actual = (GetVacancyResponse)source;
            
            // assert
            actual.EmployerContactEmail.Should().Be(source.Vacancy.ProviderContactEmail);
            actual.EmployerContactName.Should().Be(source.Vacancy.ProviderContactName);
            actual.EmployerContactPhone.Should().Be(source.Vacancy.ProviderContactPhone);
        }

        [Test, AutoData]
        public void Then_If_Anonymous_Then_Anon_Values_Mapped(GetVacancyQueryResult source, int ukprn)
        {
            source.Vacancy.Ukprn = ukprn.ToString();
            source.Vacancy.IsEmployerAnonymous = true;
            source.Vacancy.VacancyLocationType = "nATIonal";
            
            var actual = (GetVacancyResponse)source;
            
            actual.Should().BeEquivalentTo(source.Vacancy,options => options
                .ExcludingMissingMembers()
                .Excluding(item => item.EmployerName)
                .Excluding(item => item.CourseTitle)
                .Excluding(item => item.CourseLevel)
                .Excluding(item => item.Location)
                .Excluding(item => item.Ukprn)
                .Excluding(item => item.ClosingDate)
                .Excluding(item => item.VacancyReference)
                .Excluding(item => item.EmploymentLocationInformation)
            );
            actual.EmployerName.Should().Be(source.Vacancy.AnonymousEmployerName);
            actual.VacancyReference.Should().Be(source.Vacancy.VacancyReference.TrimVacancyReference());
            actual.Location.Should().BeNull();
            actual.IsNationalVacancy.Should().Be(source.Vacancy.VacancyLocationType.Equals("National", StringComparison.CurrentCultureIgnoreCase));
            actual.IsNationalVacancyDetails.Should().Be(source.Vacancy.VacancyLocationType.Equals("National", StringComparison.CurrentCultureIgnoreCase) ? source.Vacancy.EmploymentLocationInformation : string.Empty);
        }

        [Test, AutoData]
        public void Then_If_Null_Then_Null_Returned(GetVacancyQueryResult source)
        {
            source.Vacancy = null;
            
            var actual = (GetVacancyResponse)source;

            actual.Should().BeNull();
        }
    }
}