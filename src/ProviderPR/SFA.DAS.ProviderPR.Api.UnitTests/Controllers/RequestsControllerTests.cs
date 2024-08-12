using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SFA.DAS.ProviderPR.Api.Controllers;
using SFA.DAS.ProviderPR.Application.Requests.Commands;

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

        RequestsController sut = new(mediatorMock.Object);

        var result = await sut.AddAccount(command, cancellationToken);

        result.Should().BeOfType<OkObjectResult>();
        result.As<OkObjectResult>().Value.As<AddAccountRequestCommandResult>().RequestId.Should().Be(commandResult.RequestId);
    }
}
