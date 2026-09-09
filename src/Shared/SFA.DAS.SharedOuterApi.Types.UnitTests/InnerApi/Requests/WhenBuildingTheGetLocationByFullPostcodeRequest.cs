using SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.Location;
using System.Web;

namespace SFA.DAS.SharedOuterApi.UnitTests.InnerApi.Requests;

public class WhenBuildingTheGetLocationByFullPostcodeRequest
{
    [Test, AutoData]
    public void Then_The_Request_Is_Correctly_Built(string fullPostcode)
    {
        var actual = new GetLocationByFullPostcodeRequest(fullPostcode);

        actual.GetUrl.Should().Be($"api/postcodes?postcode={fullPostcode}");
    }

    [Test, InlineAutoData("test/*6]'est&#%'*'/@")]
    public void Then_The_Request_Is_Correctly_Encoded_For_Special_Characters(string fullPostcode)
    {
        var actual = new GetLocationByFullPostcodeRequest(fullPostcode);

        actual.GetUrl.Should()
            .Be($"api/postcodes?postcode={HttpUtility.UrlEncode(fullPostcode)}");
    }
}