using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using SFA.DAS.ApprenticeAan.Application.Infrastructure;
using SFA.DAS.ApprenticeAan.Application.Locations.Queries.GetAddresses;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.ApprenticeAan.Application.UnitTests.Locations.Queries.GetAddresses;

public class GetAddressesQueryHandlerTests
{
    [Test, MoqAutoData]
    public async Task Handle_ReturnAddressesBasedOnQuery(
        string searchTerm,
        [Frozen] Mock<ILocationApiClient> apiClient,
        [Frozen] GetAddressesQueryHandler handler,
        GetAddressesListResponse apiResponse)
    {
        var query = new GetAddressesQuery(searchTerm);

        apiClient
             .Setup(x => x.GetAddresses(searchTerm, It.IsAny<double>()))
             .ReturnsAsync(apiResponse);

        var result = await handler.Handle(query, CancellationToken.None);

        result.Addresses.Count().Should().Be(apiResponse.Addresses.Count());
    }

    [Test, MoqAutoData]
    public async Task Handle_AddressesNotFound_ReturnsEmptyResult(
        string searchTerm,
        [Frozen] Mock<ILocationApiClient> apiClient,
        [Frozen] GetAddressesQueryHandler handler)
    {
        var query = new GetAddressesQuery(searchTerm);

        apiClient
            .Setup(x => x.GetAddresses(searchTerm, It.IsAny<double>()))
            .ReturnsAsync(new GetAddressesListResponse());

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
        apiClient.Setup(x => x.GetAddresses(searchTerm, expectedMinMatch)).ReturnsAsync(new GetAddressesListResponse());

        await handler.Handle(query, new());

        apiClient.Verify(x => x.GetAddresses(searchTerm, expectedMinMatch));
    }
}