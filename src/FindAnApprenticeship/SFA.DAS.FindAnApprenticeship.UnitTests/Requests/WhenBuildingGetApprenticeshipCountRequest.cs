using SFA.DAS.FindAnApprenticeship.Domain.Models;
using SFA.DAS.FindAnApprenticeship.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Domain;

namespace SFA.DAS.FindAnApprenticeship.UnitTests.Requests;

public class WhenBuildingGetApprenticeshipCountRequest
{
    [Test, AutoData]
    public void Then_The_Request_Url_Is_Correctly_Built(double lat,
        double lon,
        int distance,
        VacancySort sort,
        WageType wageType,
        string whatSearchTerm,
        int pageNumber,
        int pageSize,
        List<string> categories,
        List<int> levels,
        bool disabilityConfident,
        List<VacancyDataSource> dataSources,
        bool? excludeNational,
        List<ApprenticeshipTypes> apprenticeshipTypes)
    {
        // arrange
        var expectedUrl = $"/api/vacancies/count?lat={lat}" +
                          $"&lon={lon}" +
                          $"&distanceInMiles={distance}" +
                          $"&pageNumber={pageNumber}" +
                          $"&pageSize={pageSize}" +
                          $"&categories={string.Join("&categories=", categories)}" +
                          $"&levels={string.Join("&levels=", levels)}" +
                          $"&searchTerm={whatSearchTerm}" +
                          $"&disabilityConfident={disabilityConfident}" +
                          $"&wageType={wageType}" +
                          $"&dataSources={string.Join("&dataSources=", dataSources)}" +
                          $"&excludeNational={excludeNational}" +
                          $"&apprenticeshipTypes={string.Join("&apprenticeshipTypes=", apprenticeshipTypes)}";
            
        // act
        var actual = new GetApprenticeshipCountRequest(lat, lon, distance, whatSearchTerm, pageNumber, pageSize, categories, levels, wageType, disabilityConfident, dataSources, excludeNational, apprenticeshipTypes);

        // assert
        actual.GetUrl.Should().Be(expectedUrl);
        actual.Version.Should().Be("2.0");
    }
}