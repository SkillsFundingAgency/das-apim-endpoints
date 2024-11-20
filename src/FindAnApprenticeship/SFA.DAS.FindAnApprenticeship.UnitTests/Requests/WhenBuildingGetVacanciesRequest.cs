using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FindAnApprenticeship.Domain.Models;
using SFA.DAS.FindAnApprenticeship.InnerApi.Requests;

namespace SFA.DAS.FindAnApprenticeship.UnitTests.Requests;

public class WhenBuildingGetVacanciesRequest
{
    [Test, AutoData]
    public void Then_The_Request_Url_Is_Correctly_Built(
        double lat,
        double lon,
        int distance,
        VacancySort sort,
        string whatSearchTerm,
        int pageNumber,
        int pageSize,
        List<string> categories,
        List<string> levels,
        bool disabilityConfident,
        List<VacancyDataSource> additionalDataSources)
    {
        var actual = new GetVacanciesRequest(lat, lon, distance,whatSearchTerm, pageNumber, pageSize, categories, levels, sort, disabilityConfident, additionalDataSources);

        actual.GetUrl.Should().Be($"/api/vacancies?lat={lat}&lon={lon}&distanceInMiles={distance}&sort={sort}&pageNumber={pageNumber}&pageSize={pageSize}&categories={string.Join("&categories=", categories)}&levels={string.Join("&levels=", levels)}&searchTerm={whatSearchTerm}&disabilityConfident={disabilityConfident}&additionalDataSources={string.Join("&additionalDataSources=", additionalDataSources)}");
        actual.Version.Should().Be("2.0");
    }
}
