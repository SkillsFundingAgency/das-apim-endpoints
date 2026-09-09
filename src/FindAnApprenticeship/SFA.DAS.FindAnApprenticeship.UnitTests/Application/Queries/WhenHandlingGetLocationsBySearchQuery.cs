using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.FindAnApprenticeship.Application.Queries.GetLocationsBySearch;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.Location;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Responses.Location;

namespace SFA.DAS.FindAnApprenticeship.UnitTests.Application.Queries
{
    public class WhenHandlingGetLocationsBySearchQuery
    {
        [Test, MoqAutoData]
        public async Task Then_Gets_Locations_From_Location_Api(
            GetLocationsBySearchQuery query,
            GetLocationsListResponse apiResponse,
            [Frozen] Mock<ILocationApiClient<LocationApiConfiguration>> mockApiClient,
            GetLocationsBySearchQueryHandler handler)
        {
            mockApiClient
                .Setup(client => client.Get<GetLocationsListResponse>(
                    It.Is<GetLocationsQueryRequest>(c => c.GetUrl.Contains(query.SearchTerm))))
                .ReturnsAsync(apiResponse);

            var result = await handler.Handle(query, CancellationToken.None);

            result.Locations.Should().BeEquivalentTo(apiResponse.Locations);
        }
        
        [Test, MoqAutoData]
        public async Task Then_Gets_Locations_From_Location_Api_And_Returns_Null_If_No_Items_Returned(
            GetLocationsBySearchQuery query,
            [Frozen] Mock<ILocationApiClient<LocationApiConfiguration>> mockApiClient,
            GetLocationsBySearchQueryHandler handler)
        {
            mockApiClient
                .Setup(client => client.Get<GetLocationsListResponse>(
                    It.Is<GetLocationsQueryRequest>(c => c.GetUrl.Contains(query.SearchTerm))))
                .ReturnsAsync((GetLocationsListResponse)null!);

            var result = await handler.Handle(query, CancellationToken.None);

            result.Should().BeEquivalentTo(new GetLocationsBySearchQueryResult());
        }
    }
}