using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FindAnApprenticeship.InnerApi.Requests;

namespace SFA.DAS.FindAnApprenticeship.UnitTests.Requests;

public class WhenBuildingGetVacanciesRequest
{
    [Test, AutoData]
    public void Then_The_Request_Url_Is_Correctly_Built(
        double lat,
        double lon,
        int distance,
        string whatSearchTerm,
        string sort,
        int pageNumber,
        int pageSize,
        List<string> categories)
    {
        var actual = new GetVacanciesRequest(lat, lon, distance, sort, whatSearchTerm, pageNumber, pageSize, categories);

        actual.GetUrl.Should().Be($"/api/vacancies?lat={lat}&lon={lon}&distanceInMiles={distance}&sort={sort}&pageNumber={pageNumber}&pageSize={pageSize}&categories={string.Join("&categories=", categories)}&searchTerm={whatSearchTerm}");
        actual.Version.Should().Be("2.0");
    }
}
