using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Services;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.SharedOuterApi.UnitTests.Services
{
    public class WhenCallingGetLocationInformation
    {
        [Test, MoqAutoData]
        public async Task Then_Gets_The_Standard_And_The_List_Of_Providers_For_That_Course_From_Course_Delivery_Api_Client_With_No_Location_And_ShortlistItem_Count(
            [Frozen] Mock<ILocationApiClient<LocationApiConfiguration>> mockLocationApiClient,
            LocationLookupService service)
        {
            var location = "";
            var lat = 0;
            var lon = 0;

            var result = await service.GetLocationInformation(location, lat, lon);

            mockLocationApiClient.Verify(x=>x.Get<GetLocationsListItem>(It.IsAny<GetLocationByLocationAndAuthorityName>()), Times.Never);
        }

        [Test, MoqAutoData]
        public async Task Then_If_There_Is_A_Location_Supplied_It_Is_Searched_And_Passed_To_The_Provider_Search(
            string locationName,
            string authorityName,
            GetLocationsListItem apiLocationResponse,
            [Frozen] Mock<ILocationApiClient<LocationApiConfiguration>> mockLocationApiClient,
            LocationLookupService service)
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
        }

        [Test, MoqAutoData]
        public async Task Then_If_There_Is_A_Location_Name_With_A_Comma_Is_Is_Searched_And_Passed_To_The_Provider_Search(
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
        }
        
        [Test, MoqAutoData]
        public async Task Then_If_There_Is_A_Location_But_It_Does_Not_Have_Location_And_Authority_Supplied_It_Is_Not_Passed_To_The_Provider_Search(
            string locationName,
            GetLocationsListItem apiLocationResponse,
            [Frozen] Mock<ILocationApiClient<LocationApiConfiguration>> mockLocationApiClient,
            LocationLookupService service)
        {
            var location = $"{locationName} ";
            var lat = 0;
            var lon = 0;
            
            var result = await service.GetLocationInformation(location, lat, lon);

            mockLocationApiClient.Verify(x=>x.Get<GetLocationsListItem>(It.IsAny<GetLocationByLocationAndAuthorityName>()), Times.Never);
            result.Should().BeNull();
        }

        [Test, MoqAutoData]
        public async Task Then_If_There_Is_An_Outcode_Supplied_It_Is_Searched_And_Passed_To_The_Provider_Search(
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
        }

        [Test, MoqAutoData]
        public async Task Then_If_The_Outcode_Returns_No_Results_Then_No_Location_Is_Returned(
            GetLocationsListItem apiLocationResponse,
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

        [Test, MoqAutoData]
        public async Task Then_If_There_Is_An_Postcode_Supplied_It_Is_Searched_And_Passed_To_The_Provider_Search(
            GetLocationsListItem apiLocationResponse,
            [Frozen] Mock<ILocationApiClient<LocationApiConfiguration>> mockLocationApiClient,
            LocationLookupService service)
        {
            var postcode = "CV1 1AA";

            var location = $"{postcode}";
            var lat = 0;
            var lon = 0;
            mockLocationApiClient
                .Setup(client =>
                    client.Get<GetLocationsListItem>(
                        It.Is<GetLocationByFullPostcodeRequest>(c => c.GetUrl.Contains(postcode.Split().FirstOrDefault())
                                                                     && c.GetUrl.Contains(postcode.Split().LastOrDefault()))))
                .ReturnsAsync(apiLocationResponse);

            var result = await service.GetLocationInformation(location, lat, lon);
            
            result.Name.Should().Be(location);
            result.GeoPoint.Should().BeEquivalentTo(apiLocationResponse.Location.GeoPoint);
        }

        [Test, MoqAutoData]
        public async Task Then_If_There_Is_An_Outcode_And_District_Supplied_It_Is_Searched_And_Passed_To_The_Provider_Search(
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
        }

        [Test, MoqAutoData]
        public async Task Then_If_There_Is_A_Lat_Lon_In_The_Request_Then_The_Location_Is_Not_Searched(
            [Frozen] Mock<ILocationApiClient<LocationApiConfiguration>> mockLocationApiClient,
            LocationLookupService service)
        {
            var location = "somewhere nice";
            var lat = 25;
            var lon = 2;

            var result = await service.GetLocationInformation(location, lat, lon);
            
            mockLocationApiClient.Verify(x=>x.Get<GetLocationsListItem>(It.IsAny<IGetApiRequest>()), Times.Never);
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
                .ReturnsAsync(new GetLocationsListResponse{Locations = new List<GetLocationsListItem>{apiLocationResponse}});
            
            var result = await service.GetLocationInformation(location, lat, lon);
            
            result.Name.Should().Be($"{apiLocationResponse.LocationName}, {apiLocationResponse.LocalAuthorityName}");
            result.GeoPoint.Should().BeEquivalentTo(apiLocationResponse.Location.GeoPoint);
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
                .ReturnsAsync(new GetLocationsListResponse{Locations = new List<GetLocationsListItem>()});

            var result = await handler.GetLocationInformation(location, lat, lon);

            result.Should().BeNull();
        }
        
        [Test, MoqAutoData]
        public async Task Then_If_There_Is_A_Partial_Location_Name_Which_Is_Less_Than_Two_Characters_Then_Location_Is_Set_To_Null(
            GetLocationsListResponse apiLocationResponse,
            [Frozen] Mock<ILocationApiClient<LocationApiConfiguration>> mockLocationApiClient,
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
                .ReturnsAsync(new GetLocationsListResponse{Locations = new List<GetLocationsListItem>{apiLocationResponse}});

            var result = await service.GetLocationInformation(location, lat, lon);

            result.Should().NotBeNull();
            result.Name.Should().Be($"{apiLocationResponse.LocationName}, {apiLocationResponse.LocalAuthorityName}");
            result.GeoPoint.Should().BeEquivalentTo(apiLocationResponse.Location.GeoPoint);
        }
    }
}
