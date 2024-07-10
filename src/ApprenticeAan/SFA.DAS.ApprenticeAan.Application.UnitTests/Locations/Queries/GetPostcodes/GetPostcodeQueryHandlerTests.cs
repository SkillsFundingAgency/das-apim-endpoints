using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using SFA.DAS.ApprenticeAan.Application.Infrastructure;
using SFA.DAS.ApprenticeAan.Application.Locations.Queries.GetPostcodes;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.ApprenticeAan.Application.UnitTests.Locations.Queries.GetPostcodes;

public class GetPostcodeQueryHandlerTests
{
    [Test, MoqAutoData]
    public async Task Handle_ReturnsCoordinatesFromTheFirstAddress(
        string postCode,
        [Frozen] Mock<ILocationApiClient> apiClient,
        [Frozen(Matching.ImplementedInterfaces)] GetPostcodeQueryHandler handler,
        GetAddressesListResponse apiResponse)
    {
        var query = new GetPostcodeQuery(postCode);

        apiClient
            .Setup(x => x.GetAddresses(postCode, It.IsAny<double>()))
            .ReturnsAsync(apiResponse);

        var result = await handler.Handle(query, CancellationToken.None);

        result!.Longitude.Should().Be(apiResponse.Addresses.First().Longitude);
        result!.Latitude.Should().Be(apiResponse.Addresses.First().Latitude);
    }

    [Test, MoqAutoData]
    public async Task Handle_PostcodesNotFound_ReturnsEmptyResult(
        string postCode,
        [Frozen] Mock<ILocationApiClient> apiClient,
        [Frozen(Matching.ImplementedInterfaces)] GetPostcodeQueryHandler handler)
    {
        var query = new GetPostcodeQuery(postCode);

        var apiResponse = new GetAddressesListResponse { Addresses = Enumerable.Empty<GetAddressesListItem>() };
        apiClient
            .Setup(x => x.GetAddresses(postCode, It.IsAny<double>()))
            .ReturnsAsync(apiResponse);

        var result = await handler.Handle(query, CancellationToken.None);

        result.Should().BeNull();
    }

    [Test, MoqAutoData]
    public async Task Handle_PostcodesFoundWithLatitudeNull_ReturnsEmptyResult(
        string postCode,
        [Frozen] Mock<ILocationApiClient> apiClient,
        [Frozen(Matching.ImplementedInterfaces)] GetPostcodeQueryHandler handler)
    {
        var query = new GetPostcodeQuery(postCode);

        var addresses = new List<GetAddressesListItem>
        {
            new() { Postcode = postCode, Latitude = null, Longitude = double.MinValue }
        };

        var apiResponse = new GetAddressesListResponse { Addresses = addresses };
        apiClient
            .Setup(x => x.GetAddresses(postCode, It.IsAny<double>()))
            .ReturnsAsync(apiResponse);

        var result = await handler.Handle(query, CancellationToken.None);

        result.Should().BeNull();
    }

    [Test, MoqAutoData]
    public async Task Handle_PostcodesFoundWithLongitudeNull_ReturnsEmptyResult(
        string postCode,
        [Frozen] Mock<ILocationApiClient> apiClient,
        [Frozen(Matching.ImplementedInterfaces)] GetPostcodeQueryHandler handler)
    {
        var query = new GetPostcodeQuery(postCode);

        var addresses = new List<GetAddressesListItem>
        {
            new() { Postcode = postCode, Latitude = double.MinValue, Longitude = null }
        };

        var apiResponse = new GetAddressesListResponse { Addresses = addresses };
        apiClient
            .Setup(x => x.GetAddresses(postCode, It.IsAny<double>()))
            .ReturnsAsync(apiResponse);

        var result = await handler.Handle(query, CancellationToken.None);

        result.Should().BeNull();
    }
}
