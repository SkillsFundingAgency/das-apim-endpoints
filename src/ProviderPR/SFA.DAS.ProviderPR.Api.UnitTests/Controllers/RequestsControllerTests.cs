using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SFA.DAS.ProviderPR.Api.Controllers;
using SFA.DAS.ProviderPR.Application.Requests.Commands.AddAccountRequest;
using SFA.DAS.ProviderPR.Infrastructure;
using SFA.DAS.ProviderPR.InnerApi.Responses;

namespace SFA.DAS.ProviderPR.Api.UnitTests.Controllers;

public class RequestsControllerTests
{
    [Test]
    [AutoData]
    public async Task RequestsController_AddAccount_ReturnsOkResponse(
        AddAccountRequestCommand command,
        AddAccountRequestCommandResult commandResult, 
        CancellationToken cancellationToken
    )
    {
        Mock<IMediator> mediatorMock = new();
        mediatorMock.Setup(c => c.Send(It.IsAny<AddAccountRequestCommand>(), cancellationToken)).ReturnsAsync(commandResult);

        RequestsController sut = new(mediatorMock.Object, new Mock<IProviderRelationshipsApiRestClient>().Object);

        var result = await sut.AddAccount(command, cancellationToken);

        result.Should().BeOfType<OkObjectResult>();
        result.As<OkObjectResult>().Value.As<AddAccountRequestCommandResult>().RequestId.Should().Be(commandResult.RequestId);
    }

    [Test]
    [AutoData]
    public async Task RequestsController_GetRequest_ReturnsOkResponse(
        Guid requestId,
        GetRequestResponse response
    )
    {
        Mock<IProviderRelationshipsApiRestClient> client = new();
        client.Setup(c => c.GetRequest(requestId, CancellationToken.None)).ReturnsAsync(response);

        RequestsController sut = new(new Mock<IMediator>().Object, client.Object);

        var result = await sut.GetRequest(requestId, CancellationToken.None);

        result.Should().BeOfType<OkObjectResult>();
    }

    [Test]
    [AutoData]
    public async Task RequestsController_GetRequest_ReturnsNotFoundResult(
        Guid requestId,
        GetRequestResponse response
    )
    {
        Mock<IProviderRelationshipsApiRestClient> client = new();
        client.Setup(c => c.GetRequest(requestId, CancellationToken.None)).ReturnsAsync((GetRequestResponse?)null);

        RequestsController sut = new(new Mock<IMediator>().Object, client.Object);

        var result = await sut.GetRequest(requestId, CancellationToken.None);

        result.Should().BeOfType<NotFoundResult>();
    }
}
