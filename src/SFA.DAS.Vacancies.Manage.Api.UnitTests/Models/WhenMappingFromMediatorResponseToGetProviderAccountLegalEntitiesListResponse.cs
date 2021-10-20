using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.Vacancies.Manage.Api.Models;
using SFA.DAS.Vacancies.Manage.Application.Providers.Queries.GetProviderAccountLegalEntities;

namespace SFA.DAS.Vacancies.Manage.Api.UnitTests.Models
{
    public class WhenMappingFromMediatorResponseToGetProviderAccountLegalEntitiesListResponse
    {
        [Test, AutoData]
        public void Then_The_Fields_Are_Correctly_Mapped(GetProviderAccountLegalEntitiesQueryResponse source)
        {
            var actual = (GetProviderAccountLegalEntitiesListResponse) source;

            actual.ProviderAccountLegalEntities.Should().BeEquivalentTo(source.ProviderAccountLegalEntities, options => options
                .Excluding(c=>c.AccountId)
                .Excluding(c=>c.AccountLegalEntityId)
                .Excluding(c=>c.AccountProviderId)
            );
        }

        [Test, AutoData]
        public void Then_If_Null_Empty_List_Returned(GetProviderAccountLegalEntitiesQueryResponse source)
        {
            source.ProviderAccountLegalEntities = null;
            
            var actual = (GetProviderAccountLegalEntitiesListResponse) source;

            actual.ProviderAccountLegalEntities.Should().BeEmpty();
        }
    }
}