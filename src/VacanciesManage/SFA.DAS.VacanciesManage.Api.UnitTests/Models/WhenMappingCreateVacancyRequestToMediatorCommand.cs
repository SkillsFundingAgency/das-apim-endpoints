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
        public void Then_If_Foundation_And_No_Skills_Or_Qualifications_Set_To_Empty_List(CreateVacancyRequest source)
        {
            // arrange
            source.Qualifications = null;
            source.Skills = null;
            
            // act
            var actual = (PostVacancyRequestData) source;
            
            // assert
            actual.Skills.Should().BeEmpty();
            actual.Qualifications.Should().BeEmpty();
        }
    }
}
