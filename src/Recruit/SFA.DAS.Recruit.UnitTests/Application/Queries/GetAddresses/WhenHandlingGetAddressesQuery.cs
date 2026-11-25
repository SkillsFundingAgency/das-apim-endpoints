using SFA.DAS.Recruit.Application.Queries.GetAddresses;
using SFA.DAS.Recruit.Enums;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.Location;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Responses.Location;

namespace SFA.DAS.Recruit.UnitTests.Application.Queries.GetAddresses
{
    public class WhenHandlingGetAddressesQuery
    {
        [Test, MoqAutoData]
        public async Task Then_Gets_Addresses_From_Locations_Api(
            GetAddressesQuery query,
            GetAddressesListResponse apiResponse,
            [Frozen] Mock<ILocationApiClient<LocationApiConfiguration>> mockApiClient,
            GetAddressesQueryHandler handler)
        {
            foreach (var addressesListItem in apiResponse.Addresses)
            {
                addressesListItem.Country = nameof(Country.England);
            }
            mockApiClient
                .Setup(client => client.Get<GetAddressesListResponse>(It.IsAny<GetAddressesQueryRequest>()))
                .ReturnsAsync(apiResponse);

            var result = await handler.Handle(query, CancellationToken.None);

            result.AddressesResponse.Addresses.Should().BeEquivalentTo(apiResponse.Addresses);
        }

        [Test]
        [MoqInlineAutoData(nameof(Country.NorthernIreland))]
        [MoqInlineAutoData(nameof(Country.Wales))]
        [MoqInlineAutoData(nameof(Country.Scotland))]
        public async Task Then_Returns_Null_Non_England_Gets_Addresses_From_Locations_Api(
            string country,
            GetAddressesQuery query,
            GetAddressesListResponse apiResponse,
            [Frozen] Mock<ILocationApiClient<LocationApiConfiguration>> mockApiClient,
            GetAddressesQueryHandler handler)
        {
            foreach (var addressesListItem in apiResponse.Addresses)
            {
                addressesListItem.Country = country;
            }
            mockApiClient
                .Setup(client => client.Get<GetAddressesListResponse>(It.IsAny<GetAddressesQueryRequest>()))
                .ReturnsAsync(apiResponse);

            var result = await handler.Handle(query, CancellationToken.None);

            result.AddressesResponse.Addresses.Should().BeNullOrEmpty();
        }
    }
}
