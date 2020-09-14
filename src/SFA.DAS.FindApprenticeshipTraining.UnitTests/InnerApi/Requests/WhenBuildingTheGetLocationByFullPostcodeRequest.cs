using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Requests;
using System.Web;

namespace SFA.DAS.FindApprenticeshipTraining.UnitTests.InnerApi.Requests
{
    public class WhenBuildingTheGetLocationByFullPostcodeRequest
    {
        [Test, AutoData]
        public void Then_The_Request_Is_Correctly_Built(string baseUrl, string fullPostcode)
        {
            var actual = new GetLocationByFullPostcodeRequest(fullPostcode)
            {
                BaseUrl = baseUrl
            };

            actual.GetUrl.Should().Be($"{baseUrl}api/postcodes?postcode={fullPostcode}");
        }

        [Test, InlineAutoData("test/*6]'est&#%'*'/@")]
        public void Then_The_Request_Is_Correctly_Encoded_For_Special_Characters(string fullPostcode, string baseUrl)
        {
            var actual = new GetLocationByFullPostcodeRequest(fullPostcode)
            {
                BaseUrl = baseUrl
            };

            actual.GetUrl.Should()
                .Be($"{baseUrl}api/postcodes?postcode={HttpUtility.UrlEncode(fullPostcode)}");
        }
    }
}
