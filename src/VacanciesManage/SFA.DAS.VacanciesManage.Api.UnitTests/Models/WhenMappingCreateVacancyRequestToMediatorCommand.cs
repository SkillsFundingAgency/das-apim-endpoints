using AutoFixture.NUnit3;
using FluentAssertions;
using FluentAssertions.Equivalency;
using NUnit.Framework;
using SFA.DAS.SharedOuterApi.Domain;
using SFA.DAS.VacanciesManage.Api.Models;
using SFA.DAS.VacanciesManage.InnerApi.Requests;
using System;
using System.Collections.Generic;
using WageType = SFA.DAS.VacanciesManage.Api.Models.WageType;

namespace SFA.DAS.VacanciesManage.Api.UnitTests.Models
{
    public class WhenMappingCreateVacancyRequestToMediatorCommand
    {
        private EquivalencyAssertionOptions<CreateVacancyRequest> WithCorrectiveMapping(EquivalencyAssertionOptions<CreateVacancyRequest> options)
        {
            return options
                .Excluding(c => c.SubmitterContactDetails)
                .Excluding(c => c.ContractingParties)
                .Excluding(c => c.Address)
                .Excluding(c => c.MultipleAddresses)
                .Excluding(c => c.RecruitingNationally)
                .Excluding(c => c.ApprenticeshipType)
                .WithMapping("RecruitingNationallyDetails", "EmployerLocationInformation");
        }

        [Test, AutoData]
        public void Then_The_Fields_Are_Correctly_Mapped_For_Employer(CreateVacancyRequest source)
        {
            // arrange
            source.RecruitingNationally = false;
            source.MultipleAddresses = null;
            source.Wage.WageType = WageType.FixedWage;
            
            // act
            var actual = (PostVacancyRequestData) source;

            // assert
            actual.Should().BeEquivalentTo(source, WithCorrectiveMapping);
            actual.AccountLegalEntityPublicHashedId.Should().Be(source.ContractingParties.AccountLegalEntityPublicHashedId);
            actual.User.Ukprn.Should().Be(source.ContractingParties.Ukprn);
            actual.User.Email.Should().Be(source.SubmitterContactDetails.Email);
            actual.User.Name.Should().Be(source.SubmitterContactDetails.Name);
            actual.Addresses.Should().BeEquivalentTo([source.Address]);
            actual.Address.Should().BeNull();
        }

        [Test]
        [InlineAutoData(WageType.NationalMinimumWage)]
        [InlineAutoData(WageType.NationalMinimumWageForApprentices)]
        public void Then_If_The_Wage_Is_Not_Fixed_Then_Value_Set_To_Null(WageType wageType, CreateVacancyRequest source)
        {
            source.Wage.WageType = wageType;
            
            var actual = (PostVacancyRequestData) source;

            actual.Wage.FixedWageYearlyAmount.Should().BeNull();
        }
        
        [Test, AutoData]
        public void Then_The_Fields_Are_Correctly_Mapped_For_Single_Address(CreateVacancyRequest source)
        {
            // arrange
            source.RecruitingNationally = false;
            source.RecruitingNationallyDetails = null;
            source.MultipleAddresses = null;
            
            // act
            var actual = (PostVacancyRequestData) source;
            
            // assert
            actual.Should().BeEquivalentTo(source, WithCorrectiveMapping);
            actual.Address.Should().BeNull();
            actual.Addresses.Should().BeEquivalentTo([source.Address]);
            actual.EmployerLocationOption.Should().Be(AvailableWhere.OneLocation);
            actual.EmployerLocationInformation.Should().BeNull();
        }
        
        [Test, AutoData]
        public void Then_The_Fields_Are_Correctly_Mapped_For_Multiple_Addresses(CreateVacancyRequest source)
        {
            // arrange
            source.Address = null;
            source.RecruitingNationally = false;
            source.RecruitingNationallyDetails = null;
            
            // act
            var actual = (PostVacancyRequestData) source;
            
            // assert
            actual.Should().BeEquivalentTo(source, WithCorrectiveMapping);

            actual.Address.Should().BeNull();
            actual.Addresses.Should().BeEquivalentTo(source.MultipleAddresses);
            actual.EmployerLocationOption.Should().Be(AvailableWhere.MultipleLocations);
            actual.EmployerLocationInformation.Should().BeNull();
        }
        
        [Test, AutoData]
        public void Then_The_Fields_Are_Correctly_Mapped_For_Recruiting_Nationally(CreateVacancyRequest source)
        {
            // arrange
            source.Address = null;
            source.MultipleAddresses = null;
            
            // act
            var actual = (PostVacancyRequestData) source;
            
            // assert
            actual.Should().BeEquivalentTo(source, WithCorrectiveMapping);

            actual.Address.Should().BeNull();
            actual.Addresses.Should().BeNull();
            actual.EmployerLocationOption.Should().Be(AvailableWhere.AcrossEngland);
        }

        [Test, AutoData]
        public void Then_Skills_And_Qualifications_Can_Be_Empty_For_Foundation_Apprenticeships(CreateVacancyRequest source)
        {
            // arrange
            source.ApprenticeshipType = ApprenticeshipTypes.Foundation;
            source.Skills = new List<string>();
            source.Qualifications = new List<CreateVacancyQualification>();

            // act
            var actual = (PostVacancyRequestData)source;

            // assert
            actual.Should().BeEquivalentTo(source, WithCorrectiveMapping);
            actual.Skills.Should().BeEmpty();
            actual.Qualifications.Should().BeEmpty();
        }

        [Test, AutoData]
        public void Then_Throws_When_Skills_Are_Not_Provided_For_Non_Foundation_Apprenticeships(CreateVacancyRequest source)
        {
            // arrange
            source.ApprenticeshipType = ApprenticeshipTypes.Standard; // Non-Foundation type
            source.Skills = new List<string>();
            source.Qualifications = new List<CreateVacancyQualification>
            {
                new CreateVacancyQualification { QualificationType = "GCSE", Subject = "Maths", Grade = "A", Weighting = Api.Models.QualificationWeighting.Essential }
            };

            // act
            Action act = () => _ = (PostVacancyRequestData)source;

            // assert
            act.Should().Throw<ArgumentException>()
                .WithMessage("Skills are required for this Type of apprenticeship.");
        }

        [Test, AutoData]
        public void Then_Throws_When_Qualifications_Are_Null_For_Non_Foundation_Apprenticeships(CreateVacancyRequest source)
        {
            // arrange
            source.ApprenticeshipType = ApprenticeshipTypes.Standard; // Non-Foundation type
            source.Skills = new List<string> { "Teamwork" };
            source.Qualifications = null;

            // act
            Action act = () => _ = (PostVacancyRequestData)source;

            // assert
            act.Should().Throw<ArgumentException>()
                .WithMessage("Qualifications are required for this Type of apprenticeship.");
        }

        [Test, AutoData]
        public void Then_Skills_And_Qualifications_Are_Mapped_For_Non_Foundation_Apprenticeships(CreateVacancyRequest source)
        {
            // arrange
            source.ApprenticeshipType = ApprenticeshipTypes.Standard; // Non-Foundation type
            source.Skills = new List<string> { "Teamwork", "Communication" };
            source.Qualifications = new List<CreateVacancyQualification>
            {
                new CreateVacancyQualification { QualificationType = "GCSE", Subject = "Maths", Grade = "A", Weighting = Api.Models.QualificationWeighting.Essential }
            };

            // act
            var actual = (PostVacancyRequestData)source;

            // assert
            actual.Should().BeEquivalentTo(source, WithCorrectiveMapping);
            actual.Skills.Should().BeEquivalentTo(source.Skills);
            actual.Qualifications.Should().BeEquivalentTo(source.Qualifications, options => options
                .ComparingByMembers<PostCreateVacancyQualificationData>());
        }
    }
}
