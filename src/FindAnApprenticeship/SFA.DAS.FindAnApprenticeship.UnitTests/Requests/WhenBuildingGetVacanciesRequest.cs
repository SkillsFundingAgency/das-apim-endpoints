using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FindAnApprenticeship.InnerApi.Requests;

namespace SFA.DAS.FindAnApprenticeship.UnitTests.Requests;

public class WhenBuildingGetVacanciesRequest
{
    public class WhenBuildingGetApprenticeshipCountRequest
    {
        [Test, AutoData]
        public void Then_The_Request_Url_Is_Correctly_Built(double lat, double lon, List<string> routes, int distance)
        {
            var actual = new GetApprenticeshipCountRequest(lat, lon, routes, distance);

            actual.GetUrl.Should().Be($"/api/vacancies?lat={lat}&lon={lon}&routes={routes}&distance={distance}");
        }
    }
}
