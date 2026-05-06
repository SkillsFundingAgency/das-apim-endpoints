using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SFA.DAS.AdminRoatp.Api.Controllers;
using SFA.DAS.AdminRoatp.Application.Queries.GetUkrlp;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.AdminRoatp.Api.UnitTests.Controllers.OrganisationsControllerTests;
public class OrganisationsControllerGetUkrlpTests
{
    [Test, MoqAutoData]
    public async Task GetUkrlp_ReturnSuccessfulResponse(
        [Frozen] Mock<IMediator> mediatorMock,
        [Greedy] OrganisationsController sut,
        GetUkrlpQuery request,
        GetUkrlpQueryResult expectedResponse)
    {
        mediatorMock.Setup(m => m.Send(It.IsAny<GetUkrlpQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(expectedResponse);

        var result = await sut.GetUkrlp(request.Ukprn, It.IsAny<CancellationToken>());
        var response = (OkObjectResult)result;

        mediatorMock.Verify(m => m.Send(It.Is<GetUkrlpQuery>(r => r.Ukprn == request.Ukprn), It.IsAny<CancellationToken>()), Times.Once());
        response.Value.Should().BeEquivalentTo(expectedResponse);
    }

    [Test, MoqAutoData]
    public async Task GetUkrlp_ReturnNotFoundResponse(
        [Frozen] Mock<IMediator> mediatorMock,
        [Greedy] OrganisationsController sut,
        GetUkrlpQuery request)
    {
        mediatorMock.Setup(m => m.Send(It.IsAny<GetUkrlpQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(() => null);

        var result = await sut.GetUkrlp(request.Ukprn, It.IsAny<CancellationToken>());

        mediatorMock.Verify(m => m.Send(It.Is<GetUkrlpQuery>(r => r.Ukprn == request.Ukprn), It.IsAny<CancellationToken>()), Times.Once());
        result.Should().BeOfType<NotFoundResult>();
    }
}