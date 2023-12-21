using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FindAnApprenticeship.InnerApi.Requests;

namespace SFA.DAS.FindAnApprenticeship.UnitTests.Requests
{
    public class WhenBuildingGetApprenticeshipCountRequest
    {
        [Test, AutoData]
        public void Then_The_Request_Url_Is_Correctly_Built(double lat, double lon, int distance, List<string> categories)
        {
            var actual = new GetApprenticeshipCountRequest(lat, lon, distance, categories);

            actual.GetUrl.Should().Be($"/api/vacancies/count?lat={lat}&lon={lon}&distance={distance}&categories={string.Join("&categories=",categories)}");
        }
    }
}