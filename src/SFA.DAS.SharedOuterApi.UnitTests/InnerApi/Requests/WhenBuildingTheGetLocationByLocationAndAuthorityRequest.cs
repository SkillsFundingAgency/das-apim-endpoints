using System.Web;
using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;

namespace SFA.DAS.SharedOuterApi.UnitTests.InnerApi.Requests
{
    public class WhenBuildingTheGetLocationByLocationAndAuthorityRequest
    {
        [Test, AutoData]
        public void Then_The_Request_Is_Correctly_Built(string locationName, string authorityName)
        {
            var actual = new GetLocationByLocationAndAuthorityName(locationName, authorityName);

            actual.GetUrl.Should()
                .Be($"api/locations?locationName={locationName}&authorityName={authorityName}");
        }

        [Test, InlineAutoData("test/*6]'est&#", "test%'*'/@")]
        public void Then_The_Request_Is_Correctly_Encoded_For_Special_Characters(string locationName,
            string authorityName)
        {
            var actual = new GetLocationByLocationAndAuthorityName($"{locationName}&{locationName}", $"{authorityName}&{authorityName}");

            actual.GetUrl.Should()
                .Be($"api/locations?locationName={HttpUtility.UrlEncode($"{locationName}&{locationName}")}&authorityName={HttpUtility.UrlEncode($"{authorityName}&{authorityName}")}");
        }
    }
}