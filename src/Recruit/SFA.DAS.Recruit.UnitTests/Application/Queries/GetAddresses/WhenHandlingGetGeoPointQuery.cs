using SFA.DAS.Recruit.Application.Queries.GetGeoPoint;
using SFA.DAS.Recruit.Enums;
using SFA.DAS.Recruit.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.SharedOuterApi.Types.Models;

namespace SFA.DAS.Recruit.UnitTests.Application.Queries.GetAddresses
{
    public class WhenHandlingGetGeoPointQuery
    {
        [Test, MoqAutoData]
        public async Task Then_Calls_Location_Service_To_Get_GeoPoint(
            GetGeoPointQuery query,
            GetGeoPointResponse apiResponse,
            [Frozen] Mock<ILocationLookupService> mockLocationLookupService,
            GetGeoPointQueryHandler handler)
        {
            var result = await handler.Handle(query, CancellationToken.None);
            mockLocationLookupService.Verify(p => p.GetLocationInformation(query.Postcode, 0, 0, false), Times.Once);
        }

        [Test, MoqAutoData]
        public async Task When_Location_Is_Not_Found_Then_Return_Empty_GeoPoint(
          GetGeoPointQuery query,
          GetGeoPointResponse apiResponse,
          [Frozen] Mock<ILocationLookupService> mockLocationLookupService,
          GetGeoPointQueryHandler handler)
        {
            mockLocationLookupService.Setup(m => m.GetLocationInformation(query.Postcode, 0, 0, false)).ReturnsAsync((LocationItem)null!);

            var result = await handler.Handle(query, CancellationToken.None);

            result.GetPointResponse.Should().BeNull();
        }

        [Test]
        [MoqInlineAutoData(nameof(Country.NorthernIreland))]
        [MoqInlineAutoData(nameof(Country.Scotland))]
        [MoqInlineAutoData(nameof(Country.Wales))]
        public async Task When_Location_Is_Not_Found_When_Non_England_Postcode_Then_Return_Empty_GeoPoint(
            string country,
            LocationItem locationItem,
            GetGeoPointQuery query,
            GetGeoPointResponse apiResponse,
            [Frozen] Mock<ILocationLookupService> mockLocationLookupService,
            GetGeoPointQueryHandler handler)
        {
            locationItem = new LocationItem(query.Postcode, [], country)
            {
                Country = country
            };

            mockLocationLookupService.Setup(m => m.GetLocationInformation(query.Postcode, 0, 0, false)).ReturnsAsync(locationItem);

            var result = await handler.Handle(query, CancellationToken.None);

            result.GetPointResponse.Should().BeNull();
        }
    }
}