﻿using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using SFA.DAS.AdminAan.Application.Locations.Queries.GetAddresses;
using SFA.DAS.AdminAan.Domain.Location;
using SFA.DAS.AdminAan.Infrastructure;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.AdminAan.UnitTests.Application.Locations.Queries.GetAddresses;
public class GetAddressesQueryHandlerTests
{
    [Test]
    [MoqAutoData]
    public async Task Handle_ReturnAddressesBasedOnQuery(
        GetAddressesQuery query,
        [Frozen] Mock<ILocationApiClient<LocationApiConfiguration>> apiClient,
        [Frozen] GetAddressesQueryHandler handler,
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
        [Frozen] Mock<ILocationApiClient> apiClient,
        [Frozen] GetAddressesQueryHandler handler)
    {
        apiClient
            .Setup(x => x.GetAddresses(query.Query, It.IsAny<double>()))
            .ReturnsAsync(new GetAddressesResponse());

        var result = await handler.Handle(query, CancellationToken.None);

        result.Addresses.Count().Should().Be(0);
    }

    [Test]
    [MoqInlineAutoData("mk4", GetAddressesQueryHandler.MinimumMatch)]
    [MoqInlineAutoData("mk44et", GetAddressesQueryHandler.MaximumMatch)]
    public async Task Handle_ChangesMinMatchForFullPostcode(
        string searchTerm,
        double expectedMinMatch,
        [Frozen] Mock<ILocationApiClient> apiClient,
        [Frozen] GetAddressesQueryHandler handler)
    {
        GetAddressesQuery query = new(searchTerm);
        apiClient.Setup(x => x.GetAddresses(query.Query, expectedMinMatch)).ReturnsAsync(new GetAddressesResponse());

        await handler.Handle(query, new());

        apiClient.Verify(x => x.GetAddresses(query.Query, expectedMinMatch));
    }
}
