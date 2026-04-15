using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.Vacancies.Api.Models;
using System;
using System.Linq;
using SFA.DAS.Vacancies.InnerApi.Responses;

namespace SFA.DAS.Vacancies.Api.UnitTests.Models
{
    public class WhenMappingFromMediatorResponseToGetVacancyResponseV2
    {
        [Test, AutoData]
        public void Then_The_Fields_Are_Mapped(GetVacancyApiResponse source, int ukprn)
        {
            source.Ukprn = ukprn.ToString();
            source.WageType = 3;
            source.WageUnit = 1;
            source.VacancyReference = $"VAC{source.VacancyReference}";
            
            var actual = (GetVacancyResponseV2)source;
            
            actual.Should().BeEquivalentTo(source, options => options
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
            actual.FullDescription.Should().Be(source.LongDescription);
            actual.Qualifications.Should().BeEquivalentTo(source.Qualifications.Select(c=>(GetVacancyQualification)c).ToList());
            actual.Course.Level.Should().Be(source.CourseLevel);
            actual.Course.Title.Should().Be($"{source.CourseTitle} (level {source.CourseLevel})");
            actual.Course.Route.Should().Be(source.Route);
            actual.Course.LarsCode.Should().Be(source.StandardLarsCode);
            actual.Wage.WageAmount.Should().Be(source.WageAmount);
            actual.Wage.WageType.Should().Be((WageType)source.WageType);
            actual.Wage.WageUnit.Should().Be((WageUnit)source.WageUnit);
            actual.Wage.WageAdditionalInformation.Should().Be(source.WageText);
            actual.Ukprn.Should().Be(ukprn);
            actual.VacancyReference.Should().Be(source.VacancyReference.TrimVacancyReference());
            actual.ClosingDate.Should().Be(source.ClosingDate.AddDays(1).Subtract(TimeSpan.FromSeconds(1)));
            actual.ApplicationUrl.Should().Be(source.ApplicationUrl);
            actual.IsNationalVacancy.Should().Be(source.VacancyLocationType.Equals("National", StringComparison.CurrentCultureIgnoreCase));
            actual.IsNationalVacancyDetails.Should().Be(source.VacancyLocationType.Equals("National", StringComparison.CurrentCultureIgnoreCase) ? source.EmploymentLocationInformation : string.Empty);
            actual.AdditionalTrainingDescription.Should().Be(source.AdditionalTrainingDescription);
            actual.CompanyBenefitsInformation.Should().Be(source.CompanyBenefitsInformation);
            actual.ThingsToConsider.Should().Be(source.ThingsToConsider);
            actual.Skills.Should().BeEquivalentTo(source.Skills);
        }

        [Test, AutoData]
        public void Then_If_Anonymous_Then_Anon_Values_Mapped(GetVacancyApiResponse source, int ukprn)
        {
            source.Ukprn = ukprn.ToString();
            source.IsEmployerAnonymous = true;
            source.VacancyLocationType = "nATIonal";
            
            var actual = (GetVacancyResponseV2)source;
            
            actual.Should().BeEquivalentTo(source,options => options
                .ExcludingMissingMembers()
                .Excluding(item => item.EmployerName)
                .Excluding(item => item.CourseTitle)
                .Excluding(item => item.CourseLevel)
                .Excluding(item => item.Location)
                .Excluding(item => item.Ukprn)
                .Excluding(item => item.ClosingDate)
                .Excluding(item => item.VacancyReference)
            );
            actual.EmployerName.Should().Be(source.AnonymousEmployerName);
            actual.VacancyReference.Should().Be(source.VacancyReference.TrimVacancyReference());
        }


        [Test, AutoData]
        public void Then_Addresses_Are_Combined_From_Primary_And_Other_Addresses(GetVacancyApiResponse source)
        {
            source.Ukprn = "12345";
            source.WageType = 3;
            source.WageUnit = 1;
            source.VacancyReference = $"VAC{source.VacancyReference}";

            var actual = (GetVacancyResponseV2)source;

            actual.Addresses.Should().HaveCount(source.OtherAddresses.Count + 1);
            actual.Addresses[0].AddressLine1.Should().Be(source.Address.AddressLine1);
            actual.Addresses[0].Postcode.Should().Be(source.Address.Postcode);
            foreach (var otherAddress in source.OtherAddresses)
            {
                actual.Addresses.Should().Contain(a => a.AddressLine1 == otherAddress.AddressLine1 && a.Postcode == otherAddress.Postcode);
            }
        }

        [Test, AutoData]
        public void Then_Addresses_Contains_Only_Primary_When_OtherAddresses_Is_Null(GetVacancyApiResponse source)
        {
            source.Ukprn = "12345";
            source.WageType = 3;
            source.WageUnit = 1;
            source.VacancyReference = $"VAC{source.VacancyReference}";
            source.OtherAddresses = null;

            var actual = (GetVacancyResponseV2)source;

            actual.Addresses.Should().HaveCount(1);
            actual.Addresses[0].AddressLine1.Should().Be(source.Address.AddressLine1);
            actual.Addresses[0].Postcode.Should().Be(source.Address.Postcode);
        }

        [Test, AutoData]
        public void Then_Addresses_Is_Empty_When_Primary_Address_Is_Null(GetVacancyApiResponse source)
        {
            source.Ukprn = "12345";
            source.WageType = 3;
            source.WageUnit = 1;
            source.VacancyReference = $"VAC{source.VacancyReference}";
            source.Address = null;
            source.OtherAddresses = null;

            var actual = (GetVacancyResponseV2)source;

            actual.Addresses.Should().BeEmpty();
        }

        [Test, AutoData]
        public void Then_If_Null_Then_Null_Returned(GetVacancyApiResponse source)
        {
            source = null;
            
            var actual = (GetVacancyResponseV2)source;

            actual.Should().BeNull();
        }
    }
}