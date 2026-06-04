using SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.ReferenceData;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Responses.ReferenceData;
using System.Web;

namespace SFA.DAS.SharedOuterApi.UnitTests.InnerApi.Requests;

public class WhenBuildingGetLatestDetailsRequest
{
    [Test, AutoData]
    public void Then_The_Request_Is_Correctly_Build(string searchTerm, OrganisationType organisationType)
    {
        var actual = new GetLatestDetailsRequest(searchTerm, organisationType);

        var expected = $"api/organisations/get?identifier={HttpUtility.UrlEncode(searchTerm)}&organisationType={organisationType}";

        actual.GetUrl.Should().Be(expected);
    }
}