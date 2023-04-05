using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SFA.DAS.RoatpOversight.Api.Controllers;
using SFA.DAS.RoatpOversight.Application.Commands.CreateProvider;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.RoatpOversight.Api.UnitTests.Controllers;

public class ProvidersControllerTests
{
    [Test]
    [MoqAutoData]
    public async Task CreateProvider_InvokesHandler_ReturnsCreatedResponse(
        [Frozen] Mock<IMediator> mediatorMock,
        [Greedy] ProvidersController sut,
        CreateProviderCommand command,
        CancellationToken cancellationToken)
    {
        var result = await sut.CreateProvider(command, cancellationToken);

        mediatorMock.Verify(m => m.Send(command, cancellationToken));
        result.As<StatusCodeResult>().StatusCode.Should().Be(StatusCodes.Status201Created);
    }
}
