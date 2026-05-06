using System.Web;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.Location;

namespace SFA.DAS.SharedOuterApi.UnitTests.InnerApi.Requests
{
    public class WhenBuildingTheLocationQueryRequest
    {
        [Test, AutoData]
        public void Then_The_Request_Is_Correctly_Built(string query)
        {
            var actual = new GetLocationsQueryRequest(query);

            actual.GetUrl.Should().Be($"api/search?query={query}");   
        }

        [Test, InlineAutoData("test/*6]'est&#%'*'/@")]
        public void Then_The_Request_Is_Correctly_Encoded_For_Special_Characters(string query)
        {
            var actual = new GetLocationsQueryRequest(query);

            actual.GetUrl.Should()
                .Be($"api/search?query={HttpUtility.UrlEncode(query)}");
        }
    }
}