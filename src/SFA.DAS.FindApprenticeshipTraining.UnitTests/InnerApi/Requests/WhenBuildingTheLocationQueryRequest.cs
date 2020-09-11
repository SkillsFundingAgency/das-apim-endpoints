using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Requests;
using System.Web;

namespace SFA.DAS.FindApprenticeshipTraining.UnitTests.InnerApi.Requests
{
    public class WhenBuildingTheLocationQueryRequest
    {
        [Test, AutoData]
        public void Then_The_Request_Is_Correctly_Built(string baseUrl, string query)
        {
            var actual = new GetLocationsQueryRequest(query)
            {
                BaseUrl = baseUrl
            };

            actual.GetUrl.Should().Be($"{baseUrl}api/search?query={query}");   
        }

        [Test, InlineAutoData("test/*6]'est&#%'*'/@")]
        public void Then_The_Request_Is_Correctly_Encoded_For_Special_Characters(string query, string baseUrl)
        {
            var actual = new GetLocationsQueryRequest(query)
            {
                BaseUrl = baseUrl
            };

            actual.GetUrl.Should()
                .Be($"{baseUrl}api/search?query={HttpUtility.UrlEncode(query)}");
        }
    }
}