using System.Web;
using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;

namespace SFA.DAS.SharedOuterApi.UnitTests.InnerApi.Requests
{
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
}
