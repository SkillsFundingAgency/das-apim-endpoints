using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using SFA.DAS.ApprenticeAan.Application.Locations.Queries.GetPostcodes;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.ApprenticeAan.Application.UnitTests.Locations.Queries.GetPostcodes;

public class GetPostcodeQueryHandlerTests
{
    [Test]
    [MoqAutoData]
    public async Task Handle_ReturnPostcodeBasedOnQuery(
        GetPostcodeQuery query,
        [Frozen] Mock<ILocationApiClient<LocationApiConfiguration>> apiClient,
        [Frozen(Matching.ImplementedInterfaces)] GetPostcodeQueryHandler handler,
        GetAddressesListResponse apiResponse)
    {
        apiClient
            .Setup(x => x.Get<GetAddressesListResponse>(It.IsAny<GetAddressesQueryRequest>()))
            .ReturnsAsync(apiResponse);

        var result = await handler.Handle(query, CancellationToken.None);

        result!.Coordinates.Should().NotBeNull();
    }

    [Test]
    [MoqAutoData]
    public async Task Handle_PostcodesNotFound_ReturnsEmptyResult(
        GetPostcodeQuery query,
        [Frozen] Mock<ILocationApiClient<LocationApiConfiguration>> apiClient,
        [Frozen(Matching.ImplementedInterfaces)] GetPostcodeQueryHandler handler)
    {
        var apiResponse = new GetAddressesListResponse { Addresses = Enumerable.Empty<GetAddressesListItem>() };
        apiClient
            .Setup(x => x.Get<GetAddressesListResponse>(It.IsAny<GetAddressesQueryRequest>()))
            .ReturnsAsync(apiResponse);

        var result = await handler.Handle(query, CancellationToken.None);

        result.Should().BeNull();
    }

    [Test]
    [MoqAutoData]
    public async Task Handle_PostcodesFoundWithLatitudeNull_ReturnsEmptyResult(
        GetPostcodeQuery query,
        [Frozen] Mock<ILocationApiClient<LocationApiConfiguration>> apiClient,
        [Frozen(Matching.ImplementedInterfaces)] GetPostcodeQueryHandler handler)
    {
        var addressess = new List<GetAddressesListItem>();
        addressess.Add(new GetAddressesListItem { Postcode = Guid.NewGuid().ToString(), Latitude = null, Longitude = double.MinValue });

        var apiResponse = new GetAddressesListResponse { Addresses = addressess };
        apiClient
            .Setup(x => x.Get<GetAddressesListResponse>(It.IsAny<GetAddressesQueryRequest>()))
            .ReturnsAsync(apiResponse);

        var result = await handler.Handle(query, CancellationToken.None);

        result.Should().BeNull();
    }

    [Test]
    [MoqAutoData]
    public async Task Handle_PostcodesFoundWithLongitudeNull_ReturnsEmptyResult(
        GetPostcodeQuery query,
        [Frozen] Mock<ILocationApiClient<LocationApiConfiguration>> apiClient,
        [Frozen(Matching.ImplementedInterfaces)] GetPostcodeQueryHandler handler)
    {
        var addressess = new List<GetAddressesListItem>();
        addressess.Add(new GetAddressesListItem { Postcode = Guid.NewGuid().ToString(), Latitude = double.MinValue, Longitude = null });

        var apiResponse = new GetAddressesListResponse { Addresses = addressess };
        apiClient
            .Setup(x => x.Get<GetAddressesListResponse>(It.IsAny<GetAddressesQueryRequest>()))
            .ReturnsAsync(apiResponse);

        var result = await handler.Handle(query, CancellationToken.None);

        result.Should().BeNull();
    }
}
