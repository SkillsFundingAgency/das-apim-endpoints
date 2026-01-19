using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Web;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.Location;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Location;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Services;

namespace SFA.DAS.SharedOuterApi.UnitTests.Services.LocationLookupServiceTests;

public class WhenCallingGetLocationInformation
{
    [Test, MoqAutoData]
    public async Task And_No_Location_Then_Does_Not_Call_Api(
        [Frozen] Mock<ILocationApiClient<LocationApiConfiguration>> mockLocationApiClient,
        LocationLookupService service)
    {
        var location = "";
        var lat = 0;
        var lon = 0;

        await service.GetLocationInformation(location, lat, lon);

        mockLocationApiClient.Verify(x => x.Get<GetLocationsListItem>(It.IsAny<GetLocationByLocationAndAuthorityName>()), Times.Never);
    }

    [Test, MoqAutoData]
    public async Task Then_If_There_Is_A_Location_Supplied_It_Is_Searched_And_Returned(
        string locationName,
        string authorityName,
        GetLocationsListItem apiLocationResponse,
        [Frozen] Mock<ILocationApiClient<LocationApiConfiguration>> mockLocationApiClient,
        LocationLookupService service
    )
    {
        var location = $"{locationName}, {authorityName} ";
        var lat = 0;
        var lon = 0;

        mockLocationApiClient
            .Setup(client =>
                client.Get<GetLocationsListItem>(
                    It.Is<GetLocationByLocationAndAuthorityName>(c => c.GetUrl.Contains(locationName.Trim()) && c.GetUrl.Contains(authorityName.Trim()))))
            .ReturnsAsync(apiLocationResponse);

        var result = await service.GetLocationInformation(location, lat, lon);

        result.Name.Should().Be(location);
        result.GeoPoint.Should().BeEquivalentTo(apiLocationResponse.Location.GeoPoint);
        result.Country.Should().Be(apiLocationResponse.Country);
    }

    [Test, MoqAutoData]
    public async Task Then_If_There_Is_A_Location_Name_With_A_Comma_Is_Is_Searched_And_Returned(
        string locationName,
        string authorityName,
        GetLocationsListItem apiLocationResponse,
        [Frozen] Mock<ILocationApiClient<LocationApiConfiguration>> mockLocationApiClient,
        LocationLookupService service)
    {
        var location = $"{locationName}, {locationName}, {authorityName} ";
        var lat = 0;
        var lon = 0;
        var encodedLocation = "?locationName=" + HttpUtility.UrlEncode($"{locationName}, {locationName}");

        mockLocationApiClient
            .Setup(client =>
                client.Get<GetLocationsListItem>(
                    It.Is<GetLocationByLocationAndAuthorityName>(c => c.GetUrl.Contains(encodedLocation) && c.GetUrl.Contains(authorityName.Trim()))))
            .ReturnsAsync(apiLocationResponse);

        var result = await service.GetLocationInformation(location, lat, lon);

        result.Name.Should().Be(location);
        result.GeoPoint.Should().BeEquivalentTo(apiLocationResponse.Location.GeoPoint);
        result.Country.Should().Be(apiLocationResponse.Country);
    }

    [Test, MoqAutoData]
    public async Task Then_If_There_Is_A_Location_But_It_Does_Not_Have_Location_And_Authority_Supplied_It_Is_Null(
        string locationName,
        GetLocationsListItem apiLocationResponse,
        [Frozen] Mock<ILocationApiClient<LocationApiConfiguration>> mockLocationApiClient,
        LocationLookupService service)
    {
        var location = $"{locationName} ";//no regex match
        var lat = 0;
        var lon = 0;

        var result = await service.GetLocationInformation(location, lat, lon);

        mockLocationApiClient.Verify(x => x.Get<GetLocationsListItem>(It.IsAny<GetLocationByLocationAndAuthorityName>()), Times.Never);
        result.Should().BeNull();
    }

    [Test, MoqAutoData]
    public async Task Then_If_There_Is_An_Outcode_Supplied_It_Is_Searched_And_Returned(
        GetLocationsListItem apiLocationResponse,
        [Frozen] Mock<ILocationApiClient<LocationApiConfiguration>> mockLocationApiClient,
        LocationLookupService service)
    {
        var outcode = "CV1";

        var location = $"{outcode}";
        var lat = 0;
        var lon = 0;

        mockLocationApiClient
            .Setup(client =>
                client.Get<GetLocationsListItem>(
                    It.Is<GetLocationByOutcodeRequest>(c => c.GetUrl.Contains(outcode))))
            .ReturnsAsync(apiLocationResponse);

        var result = await service.GetLocationInformation(location, lat, lon);

        result.Name.Should().Be(location);
        result.GeoPoint.Should().BeEquivalentTo(apiLocationResponse.Location.GeoPoint);
        result.Country.Should().Be(apiLocationResponse.Country);
    }

    [Test, MoqAutoData]
    public async Task Then_If_The_Outcode_Returns_No_Results_Then_No_Location_Is_Returned(
        [Frozen] Mock<ILocationApiClient<LocationApiConfiguration>> mockLocationApiClient,
        LocationLookupService service)
    {
        var outcode = "NO1SE";

        var location = $"{outcode}";
        var lat = 0;
        var lon = 0;

        mockLocationApiClient
            .Setup(client =>
                client.Get<GetLocationsListItem>(
                    It.Is<GetLocationByOutcodeRequest>(c => c.GetUrl.Contains(outcode))))
            .ReturnsAsync(new GetLocationsListItem
            {
                Location = null
            });

        var result = await service.GetLocationInformation(location, lat, lon);

        result.Should().BeNull();
    }

    [Test]
    [MoqInlineAutoData("CV1 1AA")]
    [MoqInlineAutoData("CV11AA")]
    public async Task Then_If_There_Is_An_Postcode_Supplied_It_Is_Searched_And_Returned(
        string postcode,
        GetLocationByFullPostcodeRequestV2Response response,
        [Frozen] Mock<ILocationApiClient<LocationApiConfiguration>> mockLocationApiClient,
        LocationLookupService service)
    {
        // arrange
        var location = $"{postcode}";
        response.Outcode = "";
        response.Postcode = postcode;
        
        GetLocationByFullPostcodeRequestV2? capturedRequest = null;
        mockLocationApiClient
            .Setup(x => x.Get<GetLocationByFullPostcodeRequestV2Response>(It.IsAny<GetLocationByFullPostcodeRequestV2>()))
            .Callback<IGetApiRequest>(x => capturedRequest = x as GetLocationByFullPostcodeRequestV2)
            .ReturnsAsync(response);

        // assert
        var result = await service.GetLocationInformation(location, 0, 0);
        
        // assert
        capturedRequest.Should().NotBeNull();
        capturedRequest.GetUrl.Should().Be($"api/postcodes?postcode={UrlEncoder.Default.Encode(postcode)}");
        
        result.Name.Should().Be(location);
        result.GeoPoint[0].Should().Be(response.Latitude);
        result.GeoPoint[1].Should().Be(response.Longitude);
        result.Country.Should().Be(response.Country);
    }

    [Test, MoqAutoData]
    public async Task Then_If_There_Is_A_Postcode_And_Include_District_Name_Is_Included_Option_Then_It_Is_Returned_As_Display_Name(
        GetLocationByFullPostcodeRequestV2Response response,
        [Frozen] Mock<ILocationApiClient<LocationApiConfiguration>> mockLocationApiClient,
        LocationLookupService service)
    {
        // arrange
        const string postcode = "CV1 1AA";
        response.Postcode = postcode;
        response.Outcode = null;
        var location = $"{response.Postcode}, {response.DistrictName}";

        GetLocationByFullPostcodeRequestV2? capturedRequest = null;
        mockLocationApiClient
            .Setup(x => x.Get<GetLocationByFullPostcodeRequestV2Response>(It.IsAny<GetLocationByFullPostcodeRequestV2>()))
            .Callback<IGetApiRequest>(x => capturedRequest = x as GetLocationByFullPostcodeRequestV2)
            .ReturnsAsync(response);
        
        // act
        var result = await service.GetLocationInformation(postcode, 0, 0, true);

        // assert
        capturedRequest.Should().NotBeNull();
        capturedRequest.GetUrl.Should().Be("api/postcodes?postcode=CV1%201AA");
        
        result.Name.Should().Be(location);
        result.GeoPoint[0].Should().Be(response.Latitude);
        result.GeoPoint[1].Should().Be(response.Longitude);
        result.Country.Should().Be(response.Country);
    }

    [Test, MoqAutoData]
    public async Task Then_If_There_Is_An_Outcode_And_District_Supplied_It_Is_Searched_And_Returned(
        GetLocationsListItem apiLocationResponse,
        [Frozen] Mock<ILocationApiClient<LocationApiConfiguration>> mockLocationApiClient,
        LocationLookupService service)
    {
        var outcode = "CV1";
        var location = $"{outcode} Birmingham, West Midlands";
        var lat = 0;
        var lon = 0;

        mockLocationApiClient
            .Setup(client =>
                client.Get<GetLocationsListItem>(
                    It.Is<GetLocationByOutcodeAndDistrictRequest>(c => c.GetUrl.Contains(outcode))))
            .ReturnsAsync(apiLocationResponse);

        var result = await service.GetLocationInformation(location, lat, lon);

        result.Name.Should().Be($"{apiLocationResponse.Outcode} {apiLocationResponse.DistrictName}");
        result.GeoPoint.Should().BeEquivalentTo(apiLocationResponse.Location.GeoPoint);
        result.Country.Should().Be(apiLocationResponse.Country);
    }

    [Test, MoqAutoData]
    public async Task Then_If_There_Is_A_Lat_Lon_In_The_Request_Then_The_Location_Is_Not_Searched(
        [Frozen] Mock<ILocationApiClient<LocationApiConfiguration>> mockLocationApiClient,
        LocationLookupService service)
    {
        var location = "somewhere nice";
        var lat = 25;
        var lon = 2;

        await service.GetLocationInformation(location, lat, lon);

        mockLocationApiClient.Verify(x => x.Get<GetLocationsListItem>(It.IsAny<IGetApiRequest>()), Times.Never);
    }

    [Test, MoqAutoData]
    public async Task Then_If_There_Is_A_Partial_Location_Name_Then_This_Is_Searched_And_Matched_On_The_First_Result(
        string location,
        GetLocationsListItem apiLocationResponse,
        [Frozen] Mock<ILocationApiClient<LocationApiConfiguration>> mockLocationApiClient,
        LocationLookupService service)
    {
        apiLocationResponse.Postcode = "";
        apiLocationResponse.DistrictName = "";
        var lat = 0;
        var lon = 0;

        mockLocationApiClient
            .Setup(client =>
                client.Get<GetLocationsListResponse>(
                    It.Is<GetLocationsQueryRequest>(c => c.GetUrl.Contains(location))))
            .ReturnsAsync(new GetLocationsListResponse { Locations = new List<GetLocationsListItem> { apiLocationResponse } });

        var result = await service.GetLocationInformation(location, lat, lon);

        result.Name.Should().Be($"{apiLocationResponse.LocationName}, {apiLocationResponse.LocalAuthorityName}");
        result.GeoPoint.Should().BeEquivalentTo(apiLocationResponse.Location.GeoPoint);
        result.Country.Should().Be(apiLocationResponse.Country);
    }

    [Test, MoqAutoData]
    public async Task Then_If_Doing_A_Partial_Search_And_No_Matches_Then_Null_Location_Returned(
        [Frozen] Mock<ILocationApiClient<LocationApiConfiguration>> mockLocationApiClient,
        LocationLookupService handler)
    {
        var location = "no-match";
        var lat = 0;
        var lon = 0;

        mockLocationApiClient
            .Setup(client =>
                client.Get<GetLocationsListResponse>(
                    It.Is<GetLocationsQueryRequest>(c => c.GetUrl.Contains(location))))
            .ReturnsAsync(new GetLocationsListResponse { Locations = new List<GetLocationsListItem>() });

        var result = await handler.GetLocationInformation(location, lat, lon);

        result.Should().BeNull();
    }

    [Test, MoqAutoData]
    public async Task Then_If_There_Is_A_Partial_Location_Name_Which_Is_Less_Than_Two_Characters_Then_Location_Is_Set_To_Null(
        LocationLookupService service)
    {
        var location = "C";
        var lat = 0;
        var lon = 0;

        var result = await service.GetLocationInformation(location, lat, lon);

        result.Should().BeNull();
    }

    [Test, MoqAutoData]
    public async Task Then_If_The_Location_Lookup_Returns_Empty_And_There_Is_A_Location_It_Is_Searched_And_First_Returned(
        GetLocationsListItem apiLocationResponse,
        [Frozen] Mock<ILocationApiClient<LocationApiConfiguration>> mockLocationApiClient,
        LocationLookupService service)
    {
        apiLocationResponse.Postcode = "";
        apiLocationResponse.DistrictName = "";
        var location = "LE1";
        var lat = 0;
        var lon = 0;

        mockLocationApiClient
            .Setup(client =>
                client.Get<GetLocationsListItem>(
                    It.Is<GetLocationByOutcodeRequest>(c => c.GetUrl.Contains(location))))
            .ReturnsAsync(new GetLocationsListItem
            {
                Location = null
            });
        mockLocationApiClient
            .Setup(client =>
                client.Get<GetLocationsListResponse>(
                    It.Is<GetLocationsQueryRequest>(c => c.GetUrl.Contains(location))))
            .ReturnsAsync(new GetLocationsListResponse { Locations = new List<GetLocationsListItem> { apiLocationResponse } });

        var result = await service.GetLocationInformation(location, lat, lon);

        result.Should().NotBeNull();
        result.Name.Should().Be($"{apiLocationResponse.LocationName}, {apiLocationResponse.LocalAuthorityName}");
        result.GeoPoint.Should().BeEquivalentTo(apiLocationResponse.Location.GeoPoint);
        result.Country.Should().Be(apiLocationResponse.Country);
    }
}