using AutoFixture.NUnit3;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Primitives;
using SFA.DAS.FindApprenticeshipJobs.Domain.Models;
using SFA.DAS.FindApprenticeshipJobs.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Domain;

namespace SFA.DAS.FindApprenticeshipJobs.UnitTests.InnerApi;

public class WhenBuildingGetVacanciesRequest
{
    [Test, AutoData]
    public void Then_The_Request_Url_Is_Correctly_Built(
        double lat,
        double lon,
        int distance,
        VacancySort sort,
        int pageNumber,
        int pageSize,
        List<string> categories,
        List<int>? levels,
        bool disabilityConfident,
        bool excludeNational,
        List<ApprenticeshipTypes> apprenticeshipTypes)
    {
        // arrange
        const string searchTerm = "Some search term";
           
        // assert
        var result = new GetVacanciesRequest(
            lat,
            lon,
            distance,
            searchTerm,
            pageNumber,
            pageSize,
            categories,
            levels,
            sort,
            disabilityConfident,
            excludeNational,
            new List<VacancyDataSource> { VacancyDataSource.Nhs },
            apprenticeshipTypes);
        
        var uri = new Uri(new Uri("https://localhost"), new Uri(result.GetUrl, UriKind.Relative));
        var qs = QueryHelpers.ParseQuery(uri.Query);

        // assert
        result.Version.Should().Be("2.0");
        result.GetUrl.Should().StartWith("/api/vacancies");
        qs.Should().ContainEquivalentOf(new KeyValuePair<string, StringValues>("lat", $"{lat}"));
        qs.Should().ContainEquivalentOf(new KeyValuePair<string, StringValues>("lon", $"{lon}"));
        qs.Should().ContainEquivalentOf(new KeyValuePair<string, StringValues>("distanceInMiles", $"{distance}"));
        qs.Should().ContainEquivalentOf(new KeyValuePair<string, StringValues>("pageNumber", $"{pageNumber}"));
        qs.Should().ContainEquivalentOf(new KeyValuePair<string, StringValues>("pageSize", $"{pageSize}"));
        qs.Should().ContainEquivalentOf(new KeyValuePair<string, StringValues>("categories", categories?.Select(x => $"{x}").ToArray()));
        qs.Should().ContainEquivalentOf(new KeyValuePair<string, StringValues>("levels", levels?.Select(x => $"{x}").ToArray()));
        qs.Should().ContainEquivalentOf(new KeyValuePair<string, StringValues>("searchTerm", searchTerm));
        qs.Should().ContainEquivalentOf(new KeyValuePair<string, StringValues>("disabilityConfident", $"{disabilityConfident}"));
        qs.Should().ContainEquivalentOf(new KeyValuePair<string, StringValues>("excludeNational", $"{excludeNational}"));
        qs.Should().ContainEquivalentOf(new KeyValuePair<string, StringValues>("additionalDataSources", $"Nhs"));
        qs.Should().ContainEquivalentOf(new KeyValuePair<string, StringValues>("postedInLastNumberOfDays", $"7"));
        qs.Should().ContainEquivalentOf(new KeyValuePair<string, StringValues>("apprenticeshipTypes", apprenticeshipTypes?.Select(x => $"{x}").ToArray()));
    }
}