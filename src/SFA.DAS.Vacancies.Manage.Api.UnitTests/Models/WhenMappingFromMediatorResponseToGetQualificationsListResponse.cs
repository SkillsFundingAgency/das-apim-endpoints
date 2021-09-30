using AutoFixture.NUnit3;
using NUnit.Framework;

namespace SFA.DAS.Vacancies.Manage.Api.UnitTests.Models
{
    public class WhenMappingFromMediatorResponseToGetQualificationsListResponse
    {
        [Test, AutoData]
        public void Then_The_Fields_Are_Correctly_Mapped(GetQualificationsQueryResponse source)
        {
            var actual = (GetQualificationsListResponse) source;

            actual.ProviderAccountLegalEntities.Should().BeEquivalentTo(source.ProviderAccountLegalEntities, options => options
                .Excluding(c=>c.AccountId)
                .Excluding(c=>c.AccountLegalEntityId)
                .Excluding(c=>c.AccountProviderId)
            );
        }

        [Test, AutoData]
        public void Then_If_Null_Empty_List_Returned(GetQualificationsQueryResponse source)
        {
            source.ProviderAccountLegalEntities = null;
            
            var actual = (GetQualificationsListResponse) source;

            actual.ProviderAccountLegalEntities.Should().BeEmpty();
        }
    }
}