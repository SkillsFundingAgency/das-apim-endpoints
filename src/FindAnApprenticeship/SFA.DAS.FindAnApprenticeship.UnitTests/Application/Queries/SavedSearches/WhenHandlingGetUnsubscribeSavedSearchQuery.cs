using SFA.DAS.FindAnApprenticeship.Application.Queries.SavedSearches;
using SFA.DAS.FindAnApprenticeship.InnerApi.FindApprenticeApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindAnApprenticeship.UnitTests.Application.Queries.SavedSearches;

public class WhenHandlingGetUnsubscribeSavedSearchQuery
{
    [Test, MoqAutoData]
    public async Task Then_The_Saved_Search_Is_Returned(
        GetUnsubscribeSavedSearchQuery query,
        [Frozen] Mock<ICourseService> courseService,
        [Frozen] Mock<IFindApprenticeshipApiClient<FindApprenticeshipApiConfiguration>> apiClient,
        GetUnsubscribeSavedSearchQueryHandler handler)
    {
        apiClient.Setup(x=>x.Get<GetCandidateSavedSearchApiResponse>())
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