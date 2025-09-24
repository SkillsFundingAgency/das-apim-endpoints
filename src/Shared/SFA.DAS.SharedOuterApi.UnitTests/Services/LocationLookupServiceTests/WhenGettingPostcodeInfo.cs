using System.Net;
using System.Text.Encodings.Web;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.Location;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Location;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.SharedOuterApi.Services;

namespace SFA.DAS.SharedOuterApi.UnitTests.Services.LocationLookupServiceTests;

public class WhenGettingPostcodeInfo
{
    [Test, MoqAutoData]
    public async Task Then_Null_Is_Returned_When_The_Postcode_Could_Not_Be_Found(
        string postcode,
        [Frozen] Mock<ILocationApiClient<LocationApiConfiguration>> client,
        [Greedy] LocationLookupService service)
    {
        // arrange
        var apiResponse = new ApiResponse<GetLookupPostcodeResponse>(null, HttpStatusCode.NotFound, string.Empty);
        GetLookupPostcodeRequest? capturedRequest = null;
        client
            .Setup(x => x.GetWithResponseCode<GetLookupPostcodeResponse>(It.IsAny<GetLookupPostcodeRequest>()))
            .Callback<IGetApiRequest>(x => capturedRequest = x as GetLookupPostcodeRequest)
            .ReturnsAsync(apiResponse);

        // act
        var result = await service.GetPostcodeInfoAsync(postcode);

        // assert
        capturedRequest.Version.Should().Be("2.0");
        capturedRequest.GetUrl.Should().Be($"api/postcodes?postcode={HtmlEncoder.Default.Encode(postcode)}");
        result.Should().Be(null);
    }
    
    [Test, MoqAutoData]
    public async Task Then_The_Info_Is_Returned_When_The_Postcode_Is_Found(
        string postcode,
        GetLookupPostcodeResponse response,
        [Frozen] Mock<ILocationApiClient<LocationApiConfiguration>> client,
        [Greedy] LocationLookupService service)
    {
        // arrange
        var apiResponse = new ApiResponse<GetLookupPostcodeResponse>(response, HttpStatusCode.OK, string.Empty);
        GetLookupPostcodeRequest? capturedRequest = null;
        client
            .Setup(x => x.GetWithResponseCode<GetLookupPostcodeResponse>(It.IsAny<GetLookupPostcodeRequest>()))
            .Callback<IGetApiRequest>(x => capturedRequest = x as GetLookupPostcodeRequest)
            .ReturnsAsync(apiResponse);

        // act
        var result = await service.GetPostcodeInfoAsync(postcode);

        // assert
        capturedRequest.Version.Should().Be("2.0");
        capturedRequest.GetUrl.Should().Be($"api/postcodes?postcode={HtmlEncoder.Default.Encode(postcode)}");
        result.Should().BeEquivalentTo(response, 
            opt => opt.WithMapping<GetLookupPostcodeResponse, PostcodeInfo>(x => x.DistrictName, x => x.AdminDistrict));
    }
}