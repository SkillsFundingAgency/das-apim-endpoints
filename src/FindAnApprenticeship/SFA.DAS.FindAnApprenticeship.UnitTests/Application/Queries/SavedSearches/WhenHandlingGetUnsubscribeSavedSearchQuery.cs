using SFA.DAS.FindAnApprenticeship.Application.Queries.SavedSearches;
using SFA.DAS.FindAnApprenticeship.InnerApi.FindApprenticeApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.FindAnApprenticeship.InnerApi.FindApprenticeApi.Requests;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;

namespace SFA.DAS.FindAnApprenticeship.UnitTests.Application.Queries.SavedSearches;

public class WhenHandlingGetUnsubscribeSavedSearchQuery
{
    [Test, MoqAutoData]
    public async Task Then_The_Saved_Search_Is_Returned(
        GetUnsubscribeSavedSearchQuery query,
        [Frozen] Mock<ICourseService> courseService,
        [Frozen] Mock<IFindApprenticeshipApiClient<FindApprenticeshipApiConfiguration>> apiClient,
        GetUnsubscribeSavedSearchQueryHandler sut)
    {
        // Arrange
        GetSavedSearchUnsubscribeApiRequest? passedRequest = null;
        GetRoutesListResponse? passedRoutesResponse = null;
        GetSavedSearchUnsubscribeApiResponse? passedApiResponse = null;

        courseService.Setup(x => x.GetRoutes()).ReturnsAsync(passedRoutesResponse);
        apiClient
            .Setup(x => x.Get<GetSavedSearchUnsubscribeApiResponse>(It.IsAny<GetSavedSearchUnsubscribeApiRequest>()))
            .Callback<IGetApiRequest>(x => passedRequest = x as GetSavedSearchUnsubscribeApiRequest)
            !.ReturnsAsync(passedApiResponse);

        // Act
        var result = await sut.Handle(query, default);

        // Assert
        result.Should().BeEquivalentTo(passedRequest);
        result.Routes.Should().BeEquivalentTo(passedRoutesResponse!.Routes);
        passedRequest!.GetUrl.Should().Be($"saved-searches/{query.SavedSearchId}/unsubscribe");
    }

    [Test, MoqAutoData]
    public async Task Then_If_No_Saved_Search_Null_Returned(
        GetUnsubscribeSavedSearchQuery query,
        [Frozen] Mock<ICourseService> courseService,
        [Frozen] Mock<IFindApprenticeshipApiClient<FindApprenticeshipApiConfiguration>> apiClient,
        GetUnsubscribeSavedSearchQueryHandler handler)
    {
        
    }
}