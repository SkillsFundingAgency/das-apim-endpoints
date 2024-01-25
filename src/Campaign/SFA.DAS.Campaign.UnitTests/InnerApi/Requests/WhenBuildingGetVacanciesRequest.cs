using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.Campaign.InnerApi.Requests;


namespace SFA.DAS.Campaign.UnitTests.Requests
{
    public class WhenBuildingGetVacanciesRequest
    {
        [Test, AutoData]
        public void Then_The_Request_Url_Is_Correctly_Built(
            double lat,
            double lon,
            uint distance,
            int pageNumber,
            int pageSize,
            string categories)
        {
            var actual = new GetVacanciesRequest(lat, lon, distance, pageNumber, pageSize, categories);

            actual.GetUrl.Should().Be($"/api/vacancies?lat={lat}&lon={lon}&distanceInMiles={distance}&pageNumber={pageNumber}&pageSize={pageSize}&categories={categories}");
            actual.Version.Should().Be("2.0");
        }
    }
}
