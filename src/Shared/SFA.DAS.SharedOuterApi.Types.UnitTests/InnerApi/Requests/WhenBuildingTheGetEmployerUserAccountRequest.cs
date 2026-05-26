using SFA.DAS.SharedOuterApi.Types.InnerApi.Requests;
using System.Web;

namespace SFA.DAS.SharedOuterApi.UnitTests.InnerApi.Requests;

public class WhenBuildingTheGetEmployerUserAccountRequest
{
    [Test, AutoData]
    public void Then_The_Request_Url_Is_Encoded_And_Returned(string id)
    {
        id = $"{id} ++$%^{id}";

        var actual = new GetEmployerUserAccountRequest(id);

        actual.GetUrl.Should().Be($"api/users/{HttpUtility.UrlEncode(id)}");
    }
}