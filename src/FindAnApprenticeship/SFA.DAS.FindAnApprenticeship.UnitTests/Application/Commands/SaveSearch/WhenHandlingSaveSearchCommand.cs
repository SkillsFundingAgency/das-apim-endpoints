using System.Globalization;
using System.Net;
using Newtonsoft.Json;
using SFA.DAS.FindAnApprenticeship.Application.Commands.SaveSearch;
using SFA.DAS.FindAnApprenticeship.Domain.Models;
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
        PutSavedSearchApiRequest savedSearchApiRequest,
        SaveSearchCommand saveSearchCommand,
        PutSavedSearchApiResponse savedSearchApiResponse,
        LocationItem locationItem,
        SaveSearchCommandHandler sut
    )
    {
        // arrange
        locationLookupService
            .Setup(x => x.GetLocationInformation(It.IsAny<string>(), default, default, false))
            .ReturnsAsync(locationItem);
        
        PutSavedSearchApiRequestData? savedSearchApiRequestData = null;
        apiClient
            .Setup(client => client.PutWithResponseCode<PutSavedSearchApiResponse>(It.IsAny<PutSavedSearchApiRequest>()))
            .Callback<IPutApiRequest>(x => savedSearchApiRequestData = x.Data as PutSavedSearchApiRequestData)
            .ReturnsAsync(new ApiResponse<PutSavedSearchApiResponse>(savedSearchApiResponse, HttpStatusCode.OK, string.Empty));

        // act
        var result = await sut.Handle(saveSearchCommand, CancellationToken.None);
        var savedSearchParameters = savedSearchApiRequestData?.SaveSearchRequest.SearchParameters;
        var unSubscribeToken = savedSearchApiRequestData.SaveSearchRequest.UnSubscribeToken;

        // assert
        result.Id.Should().Be(savedSearchApiResponse.Id);

        savedSearchParameters?.SearchTerm.Should().Be(saveSearchCommand.SearchTerm);
        savedSearchParameters?.Distance.Should().Be(saveSearchCommand.Distance);
        savedSearchParameters?.DisabilityConfident.Should().Be(saveSearchCommand.DisabilityConfident);
        savedSearchParameters?.SelectedLevelIds.Should().BeEquivalentTo(saveSearchCommand.SelectedLevelIds);
        savedSearchParameters?.SelectedRouteIds.Should().BeEquivalentTo(saveSearchCommand.SelectedRouteIds);
        savedSearchParameters?.Location.Should().BeEquivalentTo(saveSearchCommand.Location);
        savedSearchParameters?.Latitude.Should().Be(locationItem.GeoPoint[0].ToString(CultureInfo.InvariantCulture));
        savedSearchParameters?.Longitude.Should().Be(locationItem.GeoPoint[1].ToString(CultureInfo.InvariantCulture));
        unSubscribeToken.Should().Be(saveSearchCommand.UnSubscribeToken);
    }
}