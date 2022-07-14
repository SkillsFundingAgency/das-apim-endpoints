using System.Linq;
using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.Recruit.Api.Models;
using SFA.DAS.Recruit.Application.Queries.GetAccountLegalEntities;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;

namespace SFA.DAS.Recruit.Api.UnitTests.Models
{
    public class WhenMappingGetAccountLegalEntitiesResponseFromMediatorType
    {
        [Test, AutoData]
        public void Then_The_Fields_Are_Mapped(GetAccountLegalEntitiesQueryResult source)
        {
            var actual = (GetAccountLegalEntitiesResponse)source;

            actual.AccountLegalEntities.Should().BeEquivalentTo(source.AccountLegalEntities, options=>options.Excluding(c=>c.Agreements));
        }

        [Test, AutoData]
        public void Then_If_Has_Signed_Agreement_Flag_Set_To_True(GetAccountLegalEntitiesQueryResult source)
        {
            source.AccountLegalEntities.First().Agreements.First().Status = EmployerAgreementStatus.Signed;
            
            var actual = (GetAccountLegalEntitiesResponse)source;

            actual.AccountLegalEntities.First().HasLegalAgreement.Should().BeTrue();
        }
    }
}