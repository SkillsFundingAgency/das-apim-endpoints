using SFA.DAS.FindAnApprenticeship.Application.Queries.Users.GetSavedSearches;
using SFA.DAS.FindAnApprenticeship.InnerApi.FindApprenticeApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.FindApprenticeApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindAnApprenticeship.UnitTests.Application.Queries;

public class WhenHandlingGetCandidateSavedSearchesQuery
{
    [Test, MoqAutoData]
    public async Task Then_The_Saved_Searches_Are_Returned(
        GetCandidateSavedSearchesQuery query,
        GetCandidateSavedSearchesApiResponse queryResponse,
        [Frozen] Mock<IFindApprenticeshipApiClient<FindApprenticeshipApiConfiguration>> findApprenticeshipApiClient,
        GetCandidateSavedSearchesQueryHandler sut
        )
    {
        // arrange
        GetCandidateSavedSearchesApiRequest? passedRequest = null;
        findApprenticeshipApiClient
            .Setup(x => x.Get<GetCandidateSavedSearchesApiResponse>(It.IsAny<GetCandidateSavedSearchesApiRequest>()))
            .Callback<IGetApiRequest>(x => passedRequest = x as GetCandidateSavedSearchesApiRequest)
            .ReturnsAsync(queryResponse);
        
        // act
        var result = await sut.Handle(query, default);

        // assert
        result.Should().BeEquivalentTo(queryResponse);
        passedRequest!.GetUrl.Should().Be($"api/Users/{query.CandidateId}/SavedSearches");
    }
}