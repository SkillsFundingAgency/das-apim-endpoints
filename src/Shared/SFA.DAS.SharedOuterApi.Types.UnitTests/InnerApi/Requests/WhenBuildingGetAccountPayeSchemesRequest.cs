using SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.PayeSchemes;

namespace SFA.DAS.SharedOuterApi.UnitTests.InnerApi.Requests;

public class WhenBuildingGetAccountPayeSchemesRequest
{
    [Test, AutoData]
    public void Then_The_Url_Is_Correctly_Constructed(long accountId)
    {
        var actual = new GetAccountPayeSchemesRequest(accountId);

        actual.GetAllUrl.Should().Be($"api/accounts/{accountId}/payeschemes");
    }
}