using System.Net;
using Newtonsoft.Json;
using SFA.DAS.FindAnApprenticeship.Application.Commands.SaveSearch;
using SFA.DAS.FindAnApprenticeship.InnerApi.FindApprenticeApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.FindApprenticeApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;

namespace SFA.DAS.FindAnApprenticeship.UnitTests.Application.Commands.SaveSearch;

public class WhenHandlingSaveSearchCommand
{
    [Test, MoqAutoData]
    public async Task If_Inner_Api_Succeeds_Then_Valid_Response_Is_Returned(
        [Frozen] Mock<IFindApprenticeshipApiClient<FindApprenticeshipApiConfiguration>> apiClient,
        [Frozen] Mock<ILocationLookupService> locationLookupService,
        PostSavedSearchApiRequest savedSearchApiRequest,
        SaveSearchCommand saveSearchCommand,
        PostSavedSearchApiResponse savedSearchApiResponse,
        LocationItem locationItem,
        SaveSearchCommandHandler sut
    )
    {
        // arrange
        locationLookupService
            .Setup(x => x.GetLocationInformation(It.IsAny<string>(), default, default, false))
            .ReturnsAsync(locationItem);
        
        PostSavedSearchApiRequestData? savedSearchApiRequestData = null;
        apiClient
            .Setup(client => client.PostWithResponseCode<PostSavedSearchApiResponse>(It.IsAny<PostSavedSearchApiRequest>(), true))
            .Callback<IPostApiRequest, bool>((x, y) => savedSearchApiRequestData = x.Data as PostSavedSearchApiRequestData)
            .ReturnsAsync(new ApiResponse<PostSavedSearchApiResponse>(savedSearchApiResponse, HttpStatusCode.OK, string.Empty));

        // act
        var result = await sut.Handle(saveSearchCommand, CancellationToken.None);
        var savedSearchParameters = JsonConvert.DeserializeObject<SavedSearchParameters>(savedSearchApiRequestData?.SearchParameters);

        // assert
        result.Id.Should().Be(savedSearchApiResponse.Id);
        savedSearchParameters.Should()
            .BeEquivalentTo(saveSearchCommand, 
                options => options
                    .Excluding(x => x.CandidateId)
                    .Excluding(x => x.Location)
                );
        savedSearchParameters?.Location.Should().BeEquivalentTo(locationItem);
    }
}