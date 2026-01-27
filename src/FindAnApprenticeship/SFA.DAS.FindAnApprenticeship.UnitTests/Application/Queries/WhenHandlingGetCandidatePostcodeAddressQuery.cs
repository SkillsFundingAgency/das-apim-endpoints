using System.Text.Encodings.Web;
using SFA.DAS.FindAnApprenticeship.Application.Queries.GetCandidatePostcodeAddress;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.Location;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Location;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindAnApprenticeship.UnitTests.Application.Queries;
public class WhenHandlingGetCandidatePostcodeAddressQuery
{
    [Test, MoqAutoData]
    public async Task Then_Gets_Locations_From_Location_Api(
        GetCandidatePostcodeAddressQuery query,
        GetLocationByFullPostcodeRequestV2Response response,
        [Frozen] Mock<ILocationApiClient<LocationApiConfiguration>> mockApiClient,
        GetCandidatePostcodeAddressQueryHandler handler)
    {
        // arrange
        GetLocationByFullPostcodeRequestV2? capturedRequest = null;
        mockApiClient
            .Setup(x => x.Get<GetLocationByFullPostcodeRequestV2Response>(It.IsAny<GetLocationByFullPostcodeRequestV2>()))
            .Callback<IGetApiRequest>(x => capturedRequest = x as GetLocationByFullPostcodeRequestV2)
            .ReturnsAsync(response);
        
        // act
        var result = await handler.Handle(query, CancellationToken.None);

        // act
        capturedRequest.Should().NotBeNull();
        capturedRequest.GetUrl.Should().Be($"api/postcodes?postcode={UrlEncoder.Default.Encode(query.Postcode)}");
        
        result.PostcodeExists.Should().BeTrue();
    }

    [Test, MoqAutoData]
    public async Task Then_No_Address_Returned_So_PostcodeExists_Is_False(
        GetCandidatePostcodeAddressQuery query,
        [Frozen] Mock<ILocationApiClient<LocationApiConfiguration>> mockApiClient,
        GetCandidatePostcodeAddressQueryHandler handler)
    {
        // arrange
        GetLocationByFullPostcodeRequestV2? capturedRequest = null;
        mockApiClient
            .Setup(x => x.Get<GetLocationByFullPostcodeRequestV2Response>(It.IsAny<GetLocationByFullPostcodeRequestV2>()))
            .Callback<IGetApiRequest>(x => capturedRequest = x as GetLocationByFullPostcodeRequestV2)
            .ReturnsAsync(() => null!);

        // act
        var result = await handler.Handle(query, CancellationToken.None);

        // act
        capturedRequest.Should().NotBeNull();
        capturedRequest.GetUrl.Should().Be($"api/postcodes?postcode={UrlEncoder.Default.Encode(query.Postcode)}");
        
        result.PostcodeExists.Should().BeFalse();
    }
}
