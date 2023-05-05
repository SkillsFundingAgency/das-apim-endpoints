using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SFA.DAS.ApprenticeAan.Api.Controllers;
using SFA.DAS.ApprenticeAan.Application.Locations.Queries.GetAddresses;
using SFA.DAS.ApprenticeAan.Application.Locations.Queries.GetPostcodes;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.ApprenticeAan.Api.UnitTests.Controllers;

public class LocationsControllerTests
{
    [Test]
    [MoqAutoData]
    public async Task GetAddress_InvokesMediator(
        GetAddressesQueryResult response,
        [Frozen] Mock<IMediator> mockMediator,
        [Greedy] LocationsController sut,
        string query)
    {
        mockMediator
            .Setup(m => m.Send(It.Is<GetAddressesQuery>(q => q.Query == query), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);

        await sut.GetAddresses(query);

        mockMediator
            .Verify(m => m.Send(It.Is<GetAddressesQuery>(q => q.Query == query), It.IsAny<CancellationToken>()));
    }

    [Test]
    [MoqAutoData]
    public async Task GetAddresses_ReturnsAddresses(
        GetAddressesQueryResult response,
        [Frozen] Mock<IMediator> mockMediator,
        [Greedy] LocationsController sut)
    {
        mockMediator
            .Setup(m => m.Send(It.IsAny<GetAddressesQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);

        var result = await sut.GetAddresses("thisIsAQuery");

        result.As<OkObjectResult>().Should().NotBeNull();

        result.As<OkObjectResult>().Value.Should().BeEquivalentTo(response);
    }

    [Test]
    [MoqAutoData]
    public async Task GetAddresses_NoMatch_ReturnOkWithEmptyArray(
        [Frozen] Mock<IMediator> mockMediator,
        [Greedy] LocationsController sut)
    {
        var response = new GetAddressesQueryResult() { Addresses = Enumerable.Empty<AddressItem>() };
        mockMediator
            .Setup(m => m.Send(It.IsAny<GetAddressesQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);

        var result = await sut.GetAddresses("thisIsAQuery");

        result.As<OkObjectResult>().Should().NotBeNull();
        result.As<OkObjectResult>().Value.As<GetAddressesQueryResult>().Addresses.Should().BeEmpty();
    }

    [Test]
    [MoqAutoData]
    public async Task GetCoordinates_InvokesMediator(
        GetPostcodeQueryResult response,
        [Frozen] Mock<IMediator> mockMediator,
        [Greedy] LocationsController sut,
        string postCode)
    {
        mockMediator.Setup(m => m.Send(It.Is<GetPostcodeQuery>(q => q.PostCode == postCode), It.IsAny<CancellationToken>())).ReturnsAsync(response);

        await sut.GetCoordinates(postCode);

        mockMediator.Verify(m => m.Send(It.Is<GetPostcodeQuery>(q => q.PostCode == postCode), It.IsAny<CancellationToken>()));
    }

    [Test]
    [MoqAutoData]
    public async Task GetCoordinates_ReturnsCoordinates(
        GetPostcodeQueryResult response,
        [Frozen] Mock<IMediator> mockMediator,
        [Greedy] LocationsController sut)
    {
        mockMediator
            .Setup(m => m.Send(It.IsAny<GetPostcodeQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);

        var result = await sut.GetCoordinates(Guid.NewGuid().ToString());

        result.As<OkObjectResult>().Should().NotBeNull();

        result.As<OkObjectResult>().Value.Should().BeEquivalentTo(response);
    }

    [Test]
    [MoqAutoData]
    public async Task GetCoordinates_NoPostcodeMatch_ReturnNotFoundResponse(
        [Frozen] Mock<IMediator> mockMediator,
        [Greedy] LocationsController sut)
    {
        var response = new GetPostcodeQueryResult() { Coordinates = null! };
        mockMediator
            .Setup(m => m.Send(It.IsAny<GetPostcodeQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);

        var result = await sut.GetCoordinates(Guid.NewGuid().ToString());

        result.As<NotFoundResult>().Should().NotBeNull();
    }

}