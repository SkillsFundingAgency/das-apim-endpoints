using System.Web;
using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Requests;

namespace SFA.DAS.FindApprenticeshipTraining.UnitTests.InnerApi.Requests
{
    public class WhenBuildingTheGetLocationByLocationAndAuthorityRequest
    {
        [Test, AutoData]
        public void Then_The_Request_Is_Correctly_Built(string baseUrl, string locationName, string authorityName)
        {
            var actual = new GetLocationByLocationAndAuthorityName(locationName, authorityName)
            {
                BaseUrl = baseUrl
            };

            actual.GetUrl.Should()
                .Be($"{baseUrl}api/locations?locationName={locationName}&authorityName={authorityName}");
        }

        [Test, AutoData]
        public void Then_The_Request_Is_Correctly_Encoded_For_Special_Characters(string baseUrl, string locationName,
            string authorityName)
        {
            var actual = new GetLocationByLocationAndAuthorityName($"{locationName}&{locationName}", $"{authorityName}&{authorityName}")
            {
                BaseUrl = baseUrl
            };

            actual.GetUrl.Should()
                .Be($"{baseUrl}api/locations?locationName={HttpUtility.UrlEncode($"{locationName}&{locationName}")}&authorityName={HttpUtility.UrlEncode($"{authorityName}&{authorityName}")}");
        }
    }
}