using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.Vacancies.Manage.Api.Models;
using SFA.DAS.Vacancies.Manage.InnerApi.Requests;
using WageType = SFA.DAS.Vacancies.Manage.Api.Models.WageType;

namespace SFA.DAS.Vacancies.Manage.Api.UnitTests.Models
{
    public class WhenMappingCreateVacancyRequestToMediatorCommand
    {
        [Test, AutoData]
        public void Then_The_Fields_Are_Correctly_Mapped_For_Employer(CreateVacancyRequest source)
        {
            source.Wage.WageType = WageType.FixedWage;
            var actual = (PostVacancyRequestData) source;
            
            actual.Should().BeEquivalentTo(source, options=> options
                .Excluding(c=>c.SubmitterContactDetails)
                .Excluding(c=>c.ContractingParties)
            );
            actual.AccountLegalEntityPublicHashedId.Should().Be(source.ContractingParties.AccountLegalEntityPublicHashedId);
            actual.User.Ukprn.Should().Be(source.ContractingParties.Ukprn);
            actual.User.Email.Should().Be(source.SubmitterContactDetails.Email);
            actual.User.Name.Should().Be(source.SubmitterContactDetails.Name);
            
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
    }
}