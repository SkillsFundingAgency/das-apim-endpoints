using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindAnApprenticeship.Application.Queries.SearchIndex;
using SFA.DAS.FindAnApprenticeship.Domain.Models;
using SFA.DAS.FindAnApprenticeship.InnerApi.FindApprenticeApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.FindApprenticeApi.Responses;
using SFA.DAS.FindAnApprenticeship.Services;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FindAnApprenticeship.UnitTests.Application.Queries;

public class WhenHandlingSearchIndexQuery
{
    [Test, MoqAutoData]
    public async Task Then_The_Query_Is_Handled_And_Data_Returned(
        SearchIndexQuery query,
        long totalApprenticeshipsAvailable,
        LocationItem locationItem,
        [Frozen] Mock<ILocationLookupService> locationService,
        [Frozen] Mock<ITotalPositionsAvailableService> totalPositionsAvailableService,
        SearchIndexQueryHandler handler)
    {
        // Arrange
        query.CandidateId = null;
        totalPositionsAvailableService
            .Setup(x => x.GetTotalPositionsAvailable())
            .ReturnsAsync(totalApprenticeshipsAvailable);
        locationService.Setup(x => x.GetLocationInformation(query.LocationSearchTerm,0,0,false)).ReturnsAsync(locationItem);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(totalApprenticeshipsAvailable, Is.EqualTo(result.TotalApprenticeshipCount));
        Assert.That(result.LocationSearched, Is.True);
        Assert.That(locationItem, Is.EqualTo(result.LocationItem));
        result.Routes.Should().BeNull();
        result.SavedSearches.Should().BeNull();
    }
    
    [Test, MoqAutoData]
    public async Task Then_The_Query_Is_Handled_And_If_CandidateId_SavedSearches_And_Data_Returned(
        SearchIndexQuery query,
        long totalApprenticeshipsAvailable,
        LocationItem locationItem,
        GetRoutesListResponse routes,
        GetCandidateSavedSearchesApiResponse savedSearchesApiResponse,
        [Frozen] Mock<ICourseService> courseService,
        [Frozen] Mock<IFindApprenticeshipApiClient<FindApprenticeshipApiConfiguration>> findApprenticeshipApiClient,
        [Frozen] Mock<ILocationLookupService> locationService,
        [Frozen] Mock<ITotalPositionsAvailableService> totalPositionsAvailableService,
        SearchIndexQueryHandler handler)
    {
        // Arrange
        totalPositionsAvailableService
            .Setup(x => x.GetTotalPositionsAvailable())
            .ReturnsAsync(totalApprenticeshipsAvailable);
        locationService.Setup(x => x.GetLocationInformation(query.LocationSearchTerm,0,0,false)).ReturnsAsync(locationItem);
        courseService.Setup(x => x.GetRoutes()).ReturnsAsync(routes);
        findApprenticeshipApiClient
            .Setup(x => x.Get<GetCandidateSavedSearchesApiResponse>(
                It.Is<GetCandidateSavedSearchesApiRequest>(c => c.CandidateId.Equals(query.CandidateId))))
            .ReturnsAsync(savedSearchesApiResponse);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(totalApprenticeshipsAvailable, Is.EqualTo(result.TotalApprenticeshipCount));
        Assert.That(result.LocationSearched, Is.True);
        Assert.That(locationItem, Is.EqualTo(result.LocationItem));
        result.Routes.Should().BeEquivalentTo(routes.Routes);
        result.SavedSearches.Should()
            .BeEquivalentTo(savedSearchesApiResponse.SavedSearches.Select(c => c.MapSavedSearch()).ToList());
    }
}