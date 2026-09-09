using SFA.DAS.FindAnApprenticeship.Application.Queries.GetAddresses;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.Location;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Responses.Location;

namespace SFA.DAS.FindAnApprenticeship.UnitTests.Application.Queries
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
            mockApiClient
                .Setup(client => client.Get<GetAddressesListResponse>(It.IsAny<GetAddressesQueryRequest>()))
                .ReturnsAsync(apiResponse);

            var result = await handler.Handle(query, CancellationToken.None);

            result.AddressesResponse.Addresses.Should().BeEquivalentTo(apiResponse.Addresses);
        }
    }
}