using System.Net;
using SFA.DAS.FindAnApprenticeship.Application.Queries.SavedSearches;
using SFA.DAS.FindAnApprenticeship.InnerApi.FindApprenticeApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.FindAnApprenticeship.InnerApi.FindApprenticeApi.Requests;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Models;

namespace SFA.DAS.FindAnApprenticeship.UnitTests.Application.Queries.SavedSearches;

public class WhenHandlingGetUnsubscribeSavedSearchQuery
{
    [Test, MoqAutoData]
    public async Task Then_The_Saved_Search_Is_Returned(
        GetUnsubscribeSavedSearchQuery query,
        GetSavedSearchUnsubscribeApiResponse apiResponse,
        GetRoutesListResponse routesResponse,
        [Frozen] Mock<ICourseService> courseService,
        [Frozen] Mock<IFindApprenticeshipApiClient<FindApprenticeshipApiConfiguration>> apiClient,
        GetUnsubscribeSavedSearchQueryHandler sut)
    {
        // Arrange
        GetSavedSearchApiRequest? passedRequest = null;

        courseService.Setup(x => x.GetRoutes()).ReturnsAsync(routesResponse);
        apiClient
            .Setup(x => x.GetWithResponseCode<GetSavedSearchUnsubscribeApiResponse>(It.IsAny<GetSavedSearchApiRequest>()))
            .Callback<IGetApiRequest>(x => passedRequest = x as GetSavedSearchApiRequest)
            .ReturnsAsync(new ApiResponse<GetSavedSearchUnsubscribeApiResponse>(apiResponse, HttpStatusCode.OK, null));

        // Act
        var result = await sut.Handle(query, default);

        // Assert
        result.SavedSearch.Should().BeEquivalentTo(apiResponse.SavedSearch);
        result.Routes.Should().BeEquivalentTo(routesResponse.Routes);
        passedRequest!.GetUrl.Should().Be($"api/savedsearches/{query.SavedSearchId}");
    }

    [Test, MoqAutoData]
    public async Task Then_If_No_Saved_Search_Null_Returned(
        GetUnsubscribeSavedSearchQuery query,
        GetRoutesListResponse routesResponse,
        [Frozen] Mock<ICourseService> courseService,
        [Frozen] Mock<IFindApprenticeshipApiClient<FindApprenticeshipApiConfiguration>> apiClient,
        GetUnsubscribeSavedSearchQueryHandler handler)
    {
        courseService.Setup(x => x.GetRoutes()).ReturnsAsync(routesResponse);
        apiClient
            .Setup(x => x.GetWithResponseCode<GetSavedSearchUnsubscribeApiResponse>(It.Is<GetSavedSearchApiRequest>(c=>c.SearchId.Equals(query.SavedSearchId))))
            .ReturnsAsync(new ApiResponse<GetSavedSearchUnsubscribeApiResponse>(null!, HttpStatusCode.NotFound, null));
        
        var actual = await handler.Handle(query, default);
        
        actual.SavedSearch.Should().BeNull();
        actual.Routes.Should().BeEquivalentTo(routesResponse.Routes);
    }
}