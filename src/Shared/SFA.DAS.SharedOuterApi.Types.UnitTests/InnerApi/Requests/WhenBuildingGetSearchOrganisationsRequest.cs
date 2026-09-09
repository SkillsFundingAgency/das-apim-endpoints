using SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.ReferenceData;
using System.Web;

namespace SFA.DAS.SharedOuterApi.UnitTests.InnerApi.Requests;

public class WhenBuildingGetSearchOrganisationsRequest
{
    [Test, AutoData]
    public void Then_The_Request_Is_Correctly_Build(string searchTerm, int maximumResults)
    {
        var actual = new GetSearchOrganisationsRequest(searchTerm, maximumResults);

        var expected = $"api/organisations/?searchTerm={HttpUtility.UrlEncode(searchTerm)}&maximumResults={maximumResults}";

        actual.GetUrl.Should().Be(expected);
    }
}