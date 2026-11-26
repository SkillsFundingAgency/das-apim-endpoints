using SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.ProviderRelationships;
using SFA.DAS.SharedOuterApi.Types.Models.ProviderRelationships;
using System.Collections.Generic;

namespace SFA.DAS.SharedOuterApi.UnitTests.InnerApi.Requests;

public class WhenBuildingGetProviderAccountLegalEntitiesRequest
{
    [Test, AutoData]
    public void Then_The_Request_Is_Correctly_Build(int ukprn)
    {
        var actual = new GetProviderAccountLegalEntitiesRequest(ukprn, new List<Operation> { Operation.Recruitment, Operation.RecruitmentRequiresReview });

        actual.GetUrl.Should().Be($"accountproviderlegalentities?ukprn={ukprn}&accounthashedid=&operations=1&operations=2");
    }
}