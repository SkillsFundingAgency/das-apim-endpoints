using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.Vacancies.Api.Models;
using SFA.DAS.Vacancies.Application.EmployerAccounts.Queries.GetLegalEntitiesForEmployer;

namespace SFA.DAS.Vacancies.Api.UnitTests.Models
{
    public class WhenMappingFromMediatorResponseToGetEmployerAccountLegalEntitiesListResponse
    {
        [Test, AutoData]
        public void Then_The_Fields_Are_Correctly_Mapped(GetLegalEntitiesForEmployerResult source)
        {
            var actual = (GetEmployerAccountLegalEntitiesListResponse) source;

            actual.EmployerAccountLegalEntities.Should().BeEquivalentTo(source.LegalEntities);
        }
    }
}