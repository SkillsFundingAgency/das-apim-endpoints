using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.Vacancies.Manage.Api.Models;
using SFA.DAS.Vacancies.Manage.InnerApi.Requests;

namespace SFA.DAS.Vacancies.Manage.Api.UnitTests.Models
{
    public class WhenMappingCreateTraineeshipVacancyRequestToMediatorCommand
    {
        [Test, AutoData]
        public void Then_The_Fields_Are_Correctly_Mapped_For_Providers(CreateTraineeshipVacancyRequest source)
        {
            var actual = (PostTraineeshipVacancyRequestData)source;

            actual.Should().BeEquivalentTo(source, options => options
                .Excluding(c => c.SubmitterContactDetails)
                .Excluding(c => c.ContractingParties)
                .Excluding(c => c.DurationAndWorkingHours)
            );
            actual.AccountLegalEntityPublicHashedId.Should().Be(source.ContractingParties.AccountLegalEntityPublicHashedId);
            actual.User.Ukprn.Should().Be(source.ContractingParties.Ukprn);
            actual.User.Email.Should().Be(source.SubmitterContactDetails.Email);
            actual.User.Name.Should().Be(source.SubmitterContactDetails.Name);
            actual.Wage.Should().BeEquivalentTo(source.DurationAndWorkingHours);
        }
    }
}