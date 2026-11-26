using SFA.DAS.FindAnApprenticeship.Application.Queries.GetCandidateAddressesByPostcode;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.Location;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Responses.Location;

namespace SFA.DAS.FindAnApprenticeship.UnitTests.Application.Queries;
public class WhenHandlingGetCandidateAddressesByPostcodeQuery
{
    [Test, MoqAutoData]
    public async Task Then_Gets_Locations_From_Location_Api(
        GetCandidateAddressesByPostcodeQuery query,
        GetAddressesListResponse apiResponse,
        [Frozen] Mock<ILocationApiClient<LocationApiConfiguration>> mockApiClient,
        GetCandidateAddressesByPostcodeQueryHandler handler)
    {
        mockApiClient
            .Setup(client => client.Get<GetAddressesListResponse>(
                It.Is<GetAddressesQueryRequest>(c => c.GetUrl.Contains(query.Postcode))))
            .ReturnsAsync(apiResponse);

        var result = await handler.Handle(query, CancellationToken.None);

        result.Addresses.Count().Should().Be(apiResponse.Addresses.Count());
    }
}
