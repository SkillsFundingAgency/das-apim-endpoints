using SFA.DAS.Apim.Shared.Interfaces;
using SFA.DAS.Apim.Shared.Models;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.Location;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Responses.Location;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.SharedOuterApi.Types.Models;
using SFA.DAS.SharedOuterApi.Types.Services;
using System.Net;
using System.Text.Encodings.Web;

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
        var primaryApiResponse = new ApiResponse<GetLookupPostcodeResponse>(null, HttpStatusCode.NotFound, string.Empty);
        GetLookupPostcodeRequest? primaryCapturedRequest = null;
        client
            .Setup(x => x.GetWithResponseCode<GetLookupPostcodeResponse>(It.IsAny<GetLookupPostcodeRequest>()))
            .Callback<IGetApiRequest>(x => primaryCapturedRequest = x as GetLookupPostcodeRequest)
            .ReturnsAsync(primaryApiResponse);

        var secondaryApiResponse = new ApiResponse<GetLookupPostcodeResponseV1>(null, HttpStatusCode.NotFound, string.Empty);
        GetLookupPostcodeRequestV1? secondaryCapturedRequest = null;
        client
            .Setup(x => x.GetWithResponseCode<GetLookupPostcodeResponseV1>(It.IsAny<GetLookupPostcodeRequestV1>()))
            .Callback<IGetApiRequest>(x => secondaryCapturedRequest = x as GetLookupPostcodeRequestV1)
            .ReturnsAsync(secondaryApiResponse);

        // act
        var result = await service.GetPostcodeInfoAsync(postcode);

        // assert
        primaryCapturedRequest.Version.Should().Be("2.0");
        primaryCapturedRequest.GetUrl.Should().Be($"api/postcodes?postcode={HtmlEncoder.Default.Encode(postcode)}");
        secondaryCapturedRequest.Version.Should().Be("1.0");
        secondaryCapturedRequest.GetUrl.Should().Be($"api/postcodes?postcode={HtmlEncoder.Default.Encode(postcode)}");
        result.Should().Be(null);
    }

    [Test, MoqAutoData]
    public async Task Then_The_Info_Is_Returned_When_The_Postcode_Is_Found_In_Primary_Endpoint(
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

    [Test, MoqAutoData]
    public async Task Then_The_Info_Is_Returned_When_The_Postcode_Is_Found_In_FallBack_Endpoint(
        string postcode,
        GetLookupPostcodeResponseV1 response,
        [Frozen] Mock<ILocationApiClient<LocationApiConfiguration>> client,
        [Greedy] LocationLookupService service)
    {
        // arrange
        var primaryApiResponse = new ApiResponse<GetLookupPostcodeResponse>(null, HttpStatusCode.NotFound, string.Empty);
        GetLookupPostcodeRequest? primaryCapturedRequest = null;
        client
            .Setup(x => x.GetWithResponseCode<GetLookupPostcodeResponse>(It.IsAny<GetLookupPostcodeRequest>()))
            .Callback<IGetApiRequest>(x => primaryCapturedRequest = x as GetLookupPostcodeRequest)
            .ReturnsAsync(primaryApiResponse);

        var secondaryApiResponse = new ApiResponse<GetLookupPostcodeResponseV1>(response, HttpStatusCode.OK, string.Empty);
        GetLookupPostcodeRequestV1? secondaryCapturedRequest = null;
        client
            .Setup(x => x.GetWithResponseCode<GetLookupPostcodeResponseV1>(It.IsAny<GetLookupPostcodeRequestV1>()))
            .Callback<IGetApiRequest>(x => secondaryCapturedRequest = x as GetLookupPostcodeRequestV1)
            .ReturnsAsync(secondaryApiResponse);


        // act
        var result = await service.GetPostcodeInfoAsync(postcode);

        // assert
        primaryCapturedRequest.Version.Should().Be("2.0");
        primaryCapturedRequest.GetUrl.Should().Be($"api/postcodes?postcode={HtmlEncoder.Default.Encode(postcode)}");
        secondaryCapturedRequest.Version.Should().Be("1.0");
        secondaryCapturedRequest.GetUrl.Should().Be($"api/postcodes?postcode={HtmlEncoder.Default.Encode(postcode)}");
        result.Should().BeEquivalentTo(response,
            opt => opt.WithMapping<GetLookupPostcodeResponseV1, PostcodeInfo>(x => x.DistrictName, x => x.AdminDistrict)
                .Excluding(x => x.Location)
                .Excluding(x => x.LocalAuthorityName));
    }
}