using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.Vacancies.Manage.Api.Models;
using SFA.DAS.Vacancies.Manage.Application.EmployerAccounts.Queries.GetLegalEntitiesForEmployer;
using SFA.DAS.Vacancies.Manage.Application.Providers.Queries.GetProviderAccountLegalEntities;

namespace SFA.DAS.Vacancies.Manage.Api.UnitTests.Models
{
    public class WhenMappingFromMediatorResponseToGetAccountLegalEntitiesListResponse
    {
        [Test, AutoData]
        public void Then_The_Fields_Are_Correctly_Mapped_From_Provider_Source(GetProviderAccountLegalEntitiesQueryResponse source)
        {
            var actual = (GetAccountLegalEntitiesListResponse) source;

            actual.AccountLegalEntities.Should().BeEquivalentTo(source.ProviderAccountLegalEntities, options => options
                .Excluding(c=>c.AccountId)
                .Excluding(c=>c.AccountLegalEntityId)
                .Excluding(c=>c.AccountProviderId)
                .Excluding(c=>c.AccountHashedId)
            );
        }

        [Test, AutoData]
        public void Then_If_Null_Empty_List_Returned_From_Provider_Source(GetProviderAccountLegalEntitiesQueryResponse source)
        {
            source.ProviderAccountLegalEntities = null;
            
            var actual = (GetAccountLegalEntitiesListResponse) source;

            actual.AccountLegalEntities.Should().BeEmpty();
        }
        
        [Test, AutoData]
        public void Then_The_Fields_Are_Correctly_Mapped_From_Employer_Source(GetLegalEntitiesForEmployerResult source)
        {
            var actual = (GetAccountLegalEntitiesListResponse) source;

            actual.AccountLegalEntities.Should().BeEquivalentTo(source.LegalEntities, options=> options.Excluding(x=>x.Agreements));
        }
        
        [Test, AutoData]
        public void Then_If_Null_Empty_List_Returned_From_Employer_Source(GetLegalEntitiesForEmployerResult source)
        {
            source.LegalEntities = null;
            
            var actual = (GetAccountLegalEntitiesListResponse) source;

            actual.AccountLegalEntities.Should().BeEmpty();
        }
    }
}