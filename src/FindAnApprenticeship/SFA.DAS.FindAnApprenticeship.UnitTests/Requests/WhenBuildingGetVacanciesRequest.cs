using SFA.DAS.FindAnApprenticeship.Domain.Models;
using SFA.DAS.FindAnApprenticeship.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Domain;

namespace SFA.DAS.FindAnApprenticeship.UnitTests.Requests;

public class WhenBuildingGetVacanciesRequest
{
    [Test, AutoData]
    public void Then_The_Request_Url_Is_Correctly_Built(
        double lat,
        double lon,
        int distance,
        VacancySort sort,
        WageType skipWageType,
        string whatSearchTerm,
        int pageNumber,
        int pageSize,
        List<string> categories,
        List<int> levels,
        bool disabilityConfident,
        List<VacancyDataSource> additionalDataSources,
        bool? excludeNational,
        List<ApprenticeshipTypes> apprenticeshipTypes)
    {
        // arrange
        var expectedUrl = $"/api/vacancies?" +
                          $"lat={lat}" +
                          $"&lon={lon}" +
                          $"&distanceInMiles={distance}" +
                          $"&sort={sort}" +
                          $"&pageNumber={pageNumber}" +
                          $"&pageSize={pageSize}" +
                          $"&categories={string.Join("&categories=", categories)}" +
                          $"&levels={string.Join("&levels=", levels)}" +
                          $"&searchTerm={whatSearchTerm}" +
                          $"&disabilityConfident={disabilityConfident}" +
                          $"&skipWageType={skipWageType}" +
                          $"&additionalDataSources={string.Join("&additionalDataSources=", additionalDataSources)}" +
                          $"&excludeNational={excludeNational}" +
                          $"&apprenticeshipTypes={string.Join("&apprenticeshipTypes=", apprenticeshipTypes)}";
        
        // act
        var actual = new GetVacanciesRequest(lat, lon, distance,whatSearchTerm, pageNumber, pageSize, categories, levels, sort, skipWageType, disabilityConfident, additionalDataSources, excludeNational, apprenticeshipTypes);

        actual.GetUrl.Should().Be(expectedUrl);
        actual.Version.Should().Be("2.0");
    }
}
