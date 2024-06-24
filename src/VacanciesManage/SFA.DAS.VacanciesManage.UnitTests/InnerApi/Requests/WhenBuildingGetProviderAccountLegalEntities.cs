using System.Collections.Generic;
using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Models.ProviderRelationships;

namespace SFA.DAS.VacanciesManage.UnitTests.InnerApi.Requests
{
    public class WhenBuildingGetProviderAccountLegalEntities
    {
        [Test, AutoData]
        public void Then_The_Request_Is_Correctly_Build(int ukprn)
        {
            var actual = new GetProviderAccountLegalEntitiesRequest(ukprn, new List<Operation> { Operation.Recruitment, Operation.RecruitmentRequiresReview });

            actual.GetUrl.Should().Be($"accountproviderlegalentities?ukprn={ukprn}&operations=1&operations=2");
        }
    }
}