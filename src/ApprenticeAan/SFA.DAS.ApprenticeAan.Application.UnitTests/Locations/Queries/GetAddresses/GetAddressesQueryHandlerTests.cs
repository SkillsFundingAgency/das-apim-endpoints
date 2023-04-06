using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using SFA.DAS.ApprenticeAan.Application.Locations.Queries.GetAddresses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.ApprenticeAan.Application.UnitTests.Locations.Queries.GetAddresses;

public class GetAddressesQueryHandlerTests
{
    [Test]
    [MoqAutoData]
    public async Task Handle_ReturnAddressesBasedOnQuery(
        GetAddressesQuery query,
        [Frozen] Mock<ILocationApiClient<LocationApiConfiguration>> apiClient,
        [Frozen(Matching.ImplementedInterfaces)] GetAddressesQueryHandler handler,
        GetAddressesListResponse apiResponse)
    {
        apiClient
            .Setup(x => x.Get<GetAddressesListResponse>(It.IsAny<GetAddressesQueryRequest>()))
            .ReturnsAsync(apiResponse);

        var result = await handler.Handle(query, CancellationToken.None);

        result.Addresses.Count().Should().Be(apiResponse.Addresses.Count());
    }

    [Test]
    [MoqAutoData]
    public async Task Handle_AddressesNotFound_ReturnsEmptyResult(
        GetAddressesQuery query,
        [Frozen] Mock<ILocationApiClient<LocationApiConfiguration>> apiClient,
        [Frozen(Matching.ImplementedInterfaces)] GetAddressesQueryHandler handler)
    {
        var apiResponse = new GetAddressesListResponse { Addresses = Enumerable.Empty<GetAddressesListItem>() };
        apiClient
            .Setup(x => x.Get<GetAddressesListResponse>(It.IsAny<GetAddressesQueryRequest>()))
            .ReturnsAsync(apiResponse);

        var result = await handler.Handle(query, CancellationToken.None);

        result.Addresses.Count().Should().Be(0);
    }
}