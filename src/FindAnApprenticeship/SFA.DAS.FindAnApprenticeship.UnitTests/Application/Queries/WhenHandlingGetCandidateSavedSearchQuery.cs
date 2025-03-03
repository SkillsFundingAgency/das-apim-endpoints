using MediatR;
using SFA.DAS.FindAnApprenticeship.Application.Queries.Users.GetSavedSearch;
using SFA.DAS.FindAnApprenticeship.Application.Queries.Users.GetSavedSearches;
using SFA.DAS.FindAnApprenticeship.InnerApi.FindApprenticeApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.FindApprenticeApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindAnApprenticeship.UnitTests.Application.Queries;

public class WhenHandlingGetCandidateSavedSearchQuery
{
    [Test, MoqAutoData]
    public async Task Then_The_Saved_Search_Is_Returned(
        GetCandidateSavedSearchQuery query,
        GetCandidateSavedSearchApiResponse queryResponse,
        [Frozen] Mock<IFindApprenticeshipApiClient<FindApprenticeshipApiConfiguration>> findApprenticeshipApiClient,
        GetCandidateSavedSearchQueryHandler sut
        )
    {
        // arrange
        GetCandidateSavedSearchApiRequest? passedRequest = null;
        findApprenticeshipApiClient
            .Setup(x => x.Get<GetCandidateSavedSearchApiResponse>(It.IsAny<GetCandidateSavedSearchApiRequest>()))
            .Callback<IGetApiRequest>(x => passedRequest = x as GetCandidateSavedSearchApiRequest)
            .ReturnsAsync(queryResponse);
        
        // act
        var result = await sut.Handle(query, default);

        // assert
        result.Should().BeEquivalentTo(queryResponse);
        passedRequest!.GetUrl.Should().Be($"api/Users/{query.CandidateId}/SavedSearches/{query.Id}");
    }
}