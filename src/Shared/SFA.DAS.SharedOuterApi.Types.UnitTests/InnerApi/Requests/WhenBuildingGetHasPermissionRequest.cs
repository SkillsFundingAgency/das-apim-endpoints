using SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.ProviderRelationships;
using SFA.DAS.SharedOuterApi.Types.Models.ProviderRelationships;

namespace SFA.DAS.SharedOuterApi.UnitTests.InnerApi.Requests;
public class WhenBuildingGetHasPermissionRequest
{
    [Test, AutoData]
    public void Then_The_Request_Is_Correctly_Build(int ukprn, long accountLegalEntityId)
    {
        var actual = new GetHasPermissionRequest(ukprn, accountLegalEntityId, Operation.CreateCohort);

        actual.GetUrl.Should().Be($"permissions/has?ukprn={ukprn}&accountLegalEntityId={accountLegalEntityId}&operations={(int)Operation.CreateCohort}");
    }
}
