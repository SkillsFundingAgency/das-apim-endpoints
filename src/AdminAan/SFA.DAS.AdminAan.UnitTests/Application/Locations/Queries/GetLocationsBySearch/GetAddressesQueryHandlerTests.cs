using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using SFA.DAS.AdminAan.Application.Locations.Queries.GetLocationsBySearch;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.AdminAan.UnitTests.Application.Locations.Queries.GetLocationsBySearch;

public class GetLocationsBySearchHandlerTests
{
    [Test]
    [MoqAutoData]
    public async Task Handle_ReturnAddressesBasedOnQuery(
    GetLocationsBySearchQuery query,
    [Frozen] Mock<ILocationApiClient<LocationApiConfiguration>> apiClient,
    [Frozen] GetLocationsBySearchQueryHandler handler,
    GetLocationsListResponse apiResponse)
    {
        var expectedApiRequest = new GetLocationsQueryRequest(query.SearchTerm);

        apiClient
            .Setup(x => x.Get<GetLocationsListResponse>(It.Is<GetLocationsQueryRequest>(r => r.GetUrl == expectedApiRequest.GetUrl)))
            .ReturnsAsync(apiResponse);

        var result = await handler.Handle(query, CancellationToken.None);

        result.Locations.Should().BeEquivalentTo(apiResponse.Locations.Select(x => new GetLocationsBySearchQueryResult.Location { Name =  x.DisplayName }));
    }
}
